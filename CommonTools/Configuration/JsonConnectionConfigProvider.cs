using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace CommonTools.Configuration
{
    /// <summary>
    /// 连接字符串配置提供者接口。
    /// </summary>
    public interface IConnectionConfigProvider
    {
        /// <summary>
        /// 获取指定名称的连接字符串。
        /// </summary>
        /// <param name="name">连接名称。</param>
        /// <returns>连接字符串，如果不存在则返回空字符串。</returns>
        string GetConnectionString(string name);

        /// <summary>
        /// 设置指定名称的连接字符串。
        /// </summary>
        /// <param name="name">连接名称。</param>
        /// <param name="connectionString">连接字符串。</param>
        void SetConnectionString(string name, string connectionString);

        /// <summary>
        /// 异步设置指定名称的连接字符串。
        /// </summary>
        /// <param name="name">连接名称。</param>
        /// <param name="connectionString">连接字符串。</param>
        /// <param name="cancellationToken">取消令牌。</param>
        Task SetConnectionStringAsync(string name, string connectionString, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// 基于 JSON 配置文件的连接字符串提供者。
    /// <para>
    /// 使用 System.Text.Json 进行 JSON 操作，支持内存缓存以减少文件 I/O。
    /// </para>
    /// </summary>
    public class JsonConnectionConfigProvider : IConnectionConfigProvider
    {
        private readonly IConfiguration _configuration;
        private readonly string _configFilePath;
        private readonly SemaphoreSlim _fileLock = new(1, 1);
        private readonly ConcurrentDictionary<string, string> _cache = new();
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        /// <summary>
        /// 初始化 <see cref="JsonConnectionConfigProvider"/> 类的新实例。
        /// </summary>
        /// <param name="configuration">配置对象。</param>
        /// <exception cref="ArgumentNullException"><paramref name="configuration"/> 为 null。</exception>
        public JsonConnectionConfigProvider(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _configFilePath = GetConfigFilePath();
        }

        /// <inheritdoc/>
        public string GetConnectionString(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            return _cache.GetOrAdd(name, n =>
            {
                var connectionString = _configuration.GetConnectionString(n);

                if (string.IsNullOrEmpty(connectionString))
                {
                    var section = _configuration.GetSection("ConnectionStrings");
                    connectionString = section[n];
                }

                return connectionString ?? string.Empty;
            });
        }

        /// <inheritdoc/>
        public void SetConnectionString(string name, string connectionString)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            _cache[name] = connectionString;

            _fileLock.Wait();
            try
            {
                PersistToFile(name, connectionString);
            }
            finally
            {
                _fileLock.Release();
            }
        }

        /// <inheritdoc/>
        public async Task SetConnectionStringAsync(string name, string connectionString, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            _cache[name] = connectionString;

            await _fileLock.WaitAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                await PersistToFileAsync(name, connectionString, cancellationToken).ConfigureAwait(false);
            }
            finally
            {
                _fileLock.Release();
            }
        }

        private void PersistToFile(string name, string connectionString)
        {
            var jsonContent = File.Exists(_configFilePath)
                ? File.ReadAllText(_configFilePath, Encoding.UTF8)
                : "{}";

            var jsonNode = JsonNode.Parse(jsonContent) ?? new JsonObject();

            var connectionStrings = jsonNode["ConnectionStrings"] as JsonObject;
            if (connectionStrings == null)
            {
                connectionStrings = new JsonObject();
                jsonNode["ConnectionStrings"] = connectionStrings;
            }

            connectionStrings[name] = connectionString;

            var outputJson = jsonNode.ToJsonString(_jsonOptions);
            File.WriteAllText(_configFilePath, outputJson, Encoding.UTF8);
        }

        private async Task PersistToFileAsync(string name, string connectionString, CancellationToken cancellationToken)
        {
            var jsonContent = File.Exists(_configFilePath)
                ? await File.ReadAllTextAsync(_configFilePath, Encoding.UTF8, cancellationToken).ConfigureAwait(false)
                : "{}";

            var jsonNode = JsonNode.Parse(jsonContent) ?? new JsonObject();

            var connectionStrings = jsonNode["ConnectionStrings"] as JsonObject;
            if (connectionStrings == null)
            {
                connectionStrings = new JsonObject();
                jsonNode["ConnectionStrings"] = connectionStrings;
            }

            connectionStrings[name] = connectionString;

            var outputJson = jsonNode.ToJsonString(_jsonOptions);
            await File.WriteAllTextAsync(_configFilePath, outputJson, Encoding.UTF8, cancellationToken).ConfigureAwait(false);
        }

        private static string GetConfigFilePath()
        {
            var basePath = AppContext.BaseDirectory;

            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                      ?? Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")
                      ?? "Production";

            var configFiles = new[]
            {
                $"appsettings.{env}.json",
                "appsettings.json"
            };

            foreach (var fileName in configFiles)
            {
                var path = Path.Combine(basePath, fileName);
                if (File.Exists(path))
                {
                    return path;
                }
            }

            return Path.Combine(basePath, "appsettings.json");
        }
    }

    /// <summary>
    /// 连接配置提供者工厂。
    /// <para>
    /// 提供单例模式的 <see cref="IConnectionConfigProvider"/> 实例。
    /// </para>
    /// </summary>
    public static class ConnectionConfigProviderFactory
    {
        private static IConnectionConfigProvider? _instance;
        private static readonly SemaphoreSlim _lock = new(1, 1);

        /// <summary>
        /// 创建或获取 <see cref="IConnectionConfigProvider"/> 实例。
        /// </summary>
        /// <param name="configuration">配置对象（可选，首次调用时需要）。</param>
        /// <returns><see cref="IConnectionConfigProvider"/> 实例。</returns>
        public static IConnectionConfigProvider Create(IConfiguration? configuration = null)
        {
            if (_instance != null)
            {
                return _instance;
            }

            _lock.Wait();
            try
            {
                if (_instance != null)
                {
                    return _instance;
                }

                configuration ??= BuildDefaultConfiguration();
                _instance = new JsonConnectionConfigProvider(configuration);
                return _instance;
            }
            finally
            {
                _lock.Release();
            }
        }

        /// <summary>
        /// 异步创建或获取 <see cref="IConnectionConfigProvider"/> 实例。
        /// </summary>
        /// <param name="configuration">配置对象（可选，首次调用时需要）。</param>
        /// <param name="cancellationToken">取消令牌。</param>
        /// <returns><see cref="IConnectionConfigProvider"/> 实例。</returns>
        public static async Task<IConnectionConfigProvider> CreateAsync(IConfiguration? configuration = null, CancellationToken cancellationToken = default)
        {
            if (_instance != null)
            {
                return _instance;
            }

            await _lock.WaitAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                if (_instance != null)
                {
                    return _instance;
                }

                configuration ??= BuildDefaultConfiguration();
                _instance = new JsonConnectionConfigProvider(configuration);
                return _instance;
            }
            finally
            {
                _lock.Release();
            }
        }

        /// <summary>
        /// 设置自定义的配置提供者实例。
        /// </summary>
        /// <param name="provider">配置提供者实例。</param>
        public static void SetProvider(IConnectionConfigProvider provider)
        {
            _lock.Wait();
            try
            {
                _instance = provider;
            }
            finally
            {
                _lock.Release();
            }
        }

        /// <summary>
        /// 重置配置提供者实例（主要用于测试）。
        /// </summary>
        public static void Reset()
        {
            _lock.Wait();
            try
            {
                _instance = null;
            }
            finally
            {
                _lock.Release();
            }
        }

        private static IConfiguration BuildDefaultConfiguration()
        {
            var basePath = AppContext.BaseDirectory;

            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                      ?? Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")
                      ?? "Production";

            return new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
        }
    }
}
