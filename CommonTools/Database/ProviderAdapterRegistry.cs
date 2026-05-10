using CommonTools.Database;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace CommonTools
{
    /// <summary>
    /// 数据库提供者适配器注册表。
    /// <para>
    /// 支持静态注册和依赖注入两种方式。
    /// </para>
    /// </summary>
    /// <remarks>
    /// 使用方式：
    /// <list type="bullet">
    /// <item><description>静态注册：直接调用 <see cref="Register"/> 方法</description></item>
    /// <item><description>依赖注入：使用 <see cref="TryRegister"/> 方法，失败时不会抛出异常</description></item>
    /// </list>
    /// </remarks>
    public static class ProviderAdapterRegistry
    {
        private static readonly ConcurrentDictionary<DatabaseProvider, IDbProviderAdapter> _map = new();

        static ProviderAdapterRegistry()
        {
            TryRegisterBuiltInAdapters();
        }

        private static void TryRegisterBuiltInAdapters()
        {
            TryRegister<SqlServerAdapter>(DatabaseProvider.SqlServer);
            TryRegister<SqliteAdapter>(DatabaseProvider.Sqlite);
        }

        /// <summary>
        /// 尝试注册内置适配器类型。
        /// </summary>
        /// <typeparam name="TAdapter">适配器类型。</typeparam>
        /// <param name="provider">数据库提供者类型。</param>
        /// <returns>如果注册成功返回 true，否则返回 false。</returns>
        private static bool TryRegister<TAdapter>(DatabaseProvider provider) where TAdapter : IDbProviderAdapter, new()
        {
            try
            {
                var adapter = new TAdapter();
                _map.TryAdd(provider, adapter);
                return true;
            }
            catch (Exception ex) when (ex is DllNotFoundException or TypeLoadException or InvalidOperationException)
            {
                return false;
            }
        }

        /// <summary>
        /// 注册数据库提供者适配器。
        /// </summary>
        /// <param name="adapter">要注册的适配器实例。</param>
        /// <exception cref="ArgumentNullException"><paramref name="adapter"/> 为 null。</exception>
        public static void Register(IDbProviderAdapter adapter)
        {
            ArgumentNullException.ThrowIfNull(adapter);
            _map[adapter.Provider] = adapter;
        }

        /// <summary>
        /// 尝试注册数据库提供者适配器，如果失败则返回 false。
        /// </summary>
        /// <param name="adapter">要注册的适配器实例。</param>
        /// <returns>如果注册成功返回 true，否则返回 false。</returns>
        public static bool TryRegister(IDbProviderAdapter adapter)
        {
            if (adapter == null) return false;
            return _map.TryAdd(adapter.Provider, adapter);
        }

        /// <summary>
        /// 获取指定数据库提供者的适配器。
        /// </summary>
        /// <param name="provider">数据库提供者类型。</param>
        /// <returns>对应的数据库适配器。</returns>
        /// <exception cref="KeyNotFoundException">指定的提供者未注册。</exception>
        public static IDbProviderAdapter GetAdapter(DatabaseProvider provider)
        {
            if (_map.TryGetValue(provider, out var adapter)) return adapter;
            throw new KeyNotFoundException($"No adapter registered for provider {provider}. Available providers: {string.Join(", ", _map.Keys)}");
        }

        /// <summary>
        /// 尝试获取指定数据库提供者的适配器。
        /// </summary>
        /// <param name="provider">数据库提供者类型。</param>
        /// <param name="adapter">找到的适配器实例，如果未找到则为 null。</param>
        /// <returns>如果找到适配器返回 true，否则返回 false。</returns>
        public static bool TryGetAdapter(DatabaseProvider provider, out IDbProviderAdapter? adapter)
        {
            return _map.TryGetValue(provider, out adapter);
        }

        /// <summary>
        /// 检查指定数据库提供者是否已注册。
        /// </summary>
        /// <param name="provider">数据库提供者类型。</param>
        /// <returns>如果已注册返回 true，否则返回 false。</returns>
        public static bool IsRegistered(DatabaseProvider provider)
        {
            return _map.ContainsKey(provider);
        }

        /// <summary>
        /// 获取所有已注册的数据库提供者。
        /// </summary>
        /// <returns>已注册的提供者集合。</returns>
        public static IReadOnlyCollection<DatabaseProvider> GetRegisteredProviders()
        {
            return _map.Keys.ToArray();
        }

        /// <summary>
        /// 清除所有已注册的适配器（主要用于测试）。
        /// </summary>
        public static void Clear()
        {
            _map.Clear();
        }

        /// <summary>
        /// 重置为默认状态（重新注册内置适配器）。
        /// </summary>
        public static void Reset()
        {
            _map.Clear();
            TryRegisterBuiltInAdapters();
        }
    }
}
