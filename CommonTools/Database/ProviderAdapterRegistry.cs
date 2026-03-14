using CommonTools.Database;
using System;
using System.Collections.Generic;

namespace CommonTools
{
    // Registry to allow registration of adapters (no switch)
    public static class ProviderAdapterRegistry
    {
        private static readonly Dictionary<DatabaseProvider, IDbProviderAdapter> _map = new();

        static ProviderAdapterRegistry()
        {
            // register concrete adapters (polymorphic, no reflection)
            // Note: adapters must exist in the project. If not, callers should register their own.
            try { Register(new SqlServerAdapter()); } catch { }
            try { Register(new SqliteAdapter()); } catch { }
        }

        public static void Register(IDbProviderAdapter adapter)
        {
            if (adapter == null) throw new ArgumentNullException(nameof(adapter));
            _map[adapter.Provider] = adapter;
        }

        public static IDbProviderAdapter GetAdapter(DatabaseProvider provider)
        {
            if (_map.TryGetValue(provider, out var adapter)) return adapter;
            throw new KeyNotFoundException($"No adapter registered for provider {provider}");
        }
    }
}
