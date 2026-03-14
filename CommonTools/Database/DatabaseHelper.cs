using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using CommonTools.Base;

namespace CommonTools
{
    // Base DatabaseHelper now uses adapter (polymorphism)
    public class DatabaseHelper : IDatabaseHelper
    {
        public DatabaseProvider Provider => _adapter.Provider;
        public string ConnectionString { get; }
        internal IDbProviderAdapter Adapter => _adapter;

        private readonly IDbProviderAdapter _adapter;
        private readonly object _sync = new();
        private DbConnection? _connection;
        private bool _disposed;

        public DatabaseHelper(IDbProviderAdapter adapter, string connectionString)
        {
            _adapter = adapter ?? throw new ArgumentNullException(nameof(adapter));
            ConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        // Backwards-compatible constructor: uses registry (no switch in runtime code)
        public DatabaseHelper(DatabaseProvider provider, string connectionString)
            : this(ProviderAdapterRegistry.GetAdapter(provider), connectionString)
        {
        }

        public async Task<DbConnection> GetOpenConnectionAsync()
        {
            if (_disposed) throw new ObjectDisposedException(nameof(DatabaseHelper));

            if (_connection != null && _connection.State == ConnectionState.Open)
                return _connection;

            lock (_sync)
            {
                if (_connection == null)
                {
                    _connection = _adapter.CreateConnection();
                    if (_connection == null)
                        throw new InvalidOperationException("Unable to create a database connection for the selected provider. Ensure the appropriate ADO.NET provider is available at runtime.");
                    _connection.ConnectionString = ConnectionString;
                }
            }

            if (_connection.State != ConnectionState.Open)
            {
                await _connection.OpenAsync();
            }

            return _connection;
        }

        public async Task<int> ExecuteNonQueryAsync(string sql, IEnumerable<DbParameter>? parameters = null)
        {
            var conn = await GetOpenConnectionAsync();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            if (parameters != null)
            {
                foreach (var p in parameters)
                {
                    cmd.Parameters.Add(p);
                }
            }
            return await cmd.ExecuteNonQueryAsync();
        }

        public async Task<object?> ExecuteScalarAsync(string sql, IEnumerable<DbParameter>? parameters = null)
        {
            var conn = await GetOpenConnectionAsync();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            if (parameters != null)
            {
                foreach (var p in parameters)
                {
                    cmd.Parameters.Add(p);
                }
            }
            return await cmd.ExecuteScalarAsync();
        }

        public async Task<List<Dictionary<string, object?>>> QueryAsync(string sql, IEnumerable<DbParameter>? parameters = null)
        {
            var conn = await GetOpenConnectionAsync();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            if (parameters != null)
            {
                foreach (var p in parameters)
                {
                    cmd.Parameters.Add(p);
                }
            }

            using var reader = await cmd.ExecuteReaderAsync();
            var result = new List<Dictionary<string, object?>>();
            while (await reader.ReadAsync())
            {
                var row = new Dictionary<string, object?>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    row[reader.GetName(i)] = await reader.IsDBNullAsync(i) ? null : reader.GetValue(i);
                }
                result.Add(row);
            }
            return result;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
                try
                {
                    if (_connection != null)
                    {
                        try { _connection.Close(); } catch { }
                        try { _connection.Dispose(); } catch { }
                        _connection = null;
                    }
                }
                catch { }
            }
            _disposed = true;
        }

    }
}
