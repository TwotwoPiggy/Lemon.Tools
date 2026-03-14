using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.IO;

namespace CommonTools.Configuration
{
    public interface IConnectionConfigProvider
    {
        string GetConnectionString(string name);
        void SetConnectionString(string name, string connectionString);
    }

    public class JsonConnectionConfigProvider : IConnectionConfigProvider
    {
        private readonly IConfiguration _configuration;
        private readonly string _configFilePath;
        private readonly object _syncLock = new();

        public JsonConnectionConfigProvider(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _configFilePath = GetConfigFilePath();
        }

        public string GetConnectionString(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            var connectionString = _configuration.GetConnectionString(name);
            
            if (string.IsNullOrEmpty(connectionString))
            {
                var section = _configuration.GetSection("ConnectionStrings");
                connectionString = section[name];
            }

            return connectionString ?? string.Empty;
        }

        public void SetConnectionString(string name, string connectionString)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            lock (_syncLock)
            {
                var jsonContent = File.Exists(_configFilePath) 
                    ? File.ReadAllText(_configFilePath) 
                    : "{}";

                var json = Newtonsoft.Json.Linq.JObject.Parse(jsonContent);
                
                if (json["ConnectionStrings"] == null)
                {
                    json["ConnectionStrings"] = new Newtonsoft.Json.Linq.JObject();
                }

                json["ConnectionStrings"]![name] = connectionString;
                
                File.WriteAllText(_configFilePath, json.ToString(Newtonsoft.Json.Formatting.Indented));
            }
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

    public static class ConnectionConfigProviderFactory
    {
        private static IConnectionConfigProvider? _instance;
        private static readonly object _syncLock = new();

        public static IConnectionConfigProvider Create(IConfiguration? configuration = null)
        {
            if (_instance != null)
            {
                return _instance;
            }

            lock (_syncLock)
            {
                if (_instance != null)
                {
                    return _instance;
                }

                configuration ??= BuildDefaultConfiguration();
                _instance = new JsonConnectionConfigProvider(configuration);
                return _instance;
            }
        }

        public static void SetProvider(IConnectionConfigProvider provider)
        {
            lock (_syncLock)
            {
                _instance = provider;
            }
        }

        public static void Reset()
        {
            lock (_syncLock)
            {
                _instance = null;
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
