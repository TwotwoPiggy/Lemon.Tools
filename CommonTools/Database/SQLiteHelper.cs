using CommonTools.Base;
using SQLite;
using System;
using System.Threading;

namespace CommonTools.Database
{
    public class SQLiteHelper : DbBase, IDisposable
	{

		#region fields and properties
        public SQLiteConnection Db => _dbLazy?.Value ?? throw new ObjectDisposedException(nameof(SQLiteHelper));
        public SQLiteAsyncConnection DbAsync => _dbAsyncLazy?.Value ?? throw new ObjectDisposedException(nameof(SQLiteHelper));

        private string _connectStr = string.Empty;
        private readonly string _configName = string.Empty;
        private Lazy<SQLiteConnection> _dbLazy;
        private Lazy<SQLiteAsyncConnection> _dbAsyncLazy;
        private readonly Lock _sync = new();
        private bool _disposed;
		#endregion

		public SQLiteHelper(string configName = "SQLite")
		{
            _configName = configName;
            _connectStr = ConfigManager.GetConnectionString(_configName);
            if (string.IsNullOrWhiteSpace(_connectStr))
            {
                _connectStr = @".\default.db";
                ConfigManager.SetConnectionString(_configName, _connectStr, SqlLiteProvider);
            }

            // Initialize lazy connections with thread-safety
            _dbLazy = new Lazy<SQLiteConnection>(() => new SQLiteConnection(_connectStr), LazyThreadSafetyMode.ExecutionAndPublication);
            _dbAsyncLazy = new Lazy<SQLiteAsyncConnection>(() => new SQLiteAsyncConnection(_connectStr), LazyThreadSafetyMode.ExecutionAndPublication);
		}

		#region ConnecionString
        public void SetConnectionString(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            var currentConnStr = ConfigManager.GetConnectionString(_configName);
            if (connectionString.Equals(currentConnStr))
            {
                return;
            }

            ConfigManager.SetConnectionString(_configName, connectionString, SqlLiteProvider);

            // Dispose existing connections and reinitialize lazies
            ReinitializeConnections(connectionString);
        }
		#endregion

        public override void Disconnect()
        {
            ReinitializeConnections(_connectStr);
        }

        private void ReinitializeConnections(string newConnectionString)
        {
            lock (_sync)
            {
                // Dispose existing created connections
                try
                {
                    if (_dbLazy != null && _dbLazy.IsValueCreated)
                    {
                        try { _dbLazy.Value.Close(); } catch { }
                        try { _dbLazy.Value.Dispose(); } catch { }
                    }
                }
                finally { }

                try
                {
                    if (_dbAsyncLazy != null && _dbAsyncLazy.IsValueCreated)
                    {
                        try { _dbAsyncLazy.Value.CloseAsync().GetAwaiter().GetResult(); } catch { }
                    }
                }
                finally { }

                // Replace with new Lazy instances (keep using current _connectStr if null passed)
                if (!string.IsNullOrWhiteSpace(newConnectionString))
                {
                    _connectStr = newConnectionString!;
                }

                _dbLazy = new Lazy<SQLiteConnection>(() => new SQLiteConnection(_connectStr), LazyThreadSafetyMode.ExecutionAndPublication);
                _dbAsyncLazy = new Lazy<SQLiteAsyncConnection>(() => new SQLiteAsyncConnection(_connectStr), LazyThreadSafetyMode.ExecutionAndPublication);
            }
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
                // dispose managed
                ReinitializeConnections(null);
            }
            _disposed = true;
        }
	}
}
