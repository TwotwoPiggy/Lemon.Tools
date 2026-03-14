using CommonTools.Base;
using CommonTools.Configuration;
using Microsoft.Extensions.Configuration;
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
        private readonly string _connectionName;
        private Lazy<SQLiteConnection> _dbLazy;
        private Lazy<SQLiteAsyncConnection> _dbAsyncLazy;
        private readonly Lock _sync = new();
        private bool _disposed;
        private readonly IConnectionConfigProvider _configProvider;
        #endregion

        public SQLiteHelper(string connectionName = "Default", IConfiguration? configuration = null)
        {
            _connectionName = connectionName;
            _configProvider = ConnectionConfigProviderFactory.Create(configuration);
            
            InitializeConnections();
        }

        public SQLiteHelper(IConnectionConfigProvider configProvider, string connectionName = "Default")
        {
            _connectionName = connectionName;
            _configProvider = configProvider ?? throw new ArgumentNullException(nameof(configProvider));
            
            InitializeConnections();
        }

        private void InitializeConnections()
        {
            _connectStr = _configProvider.GetConnectionString(_connectionName);
            
            if (string.IsNullOrWhiteSpace(_connectStr))
            {
                _connectStr = @".\default.db";
                _configProvider.SetConnectionString(_connectionName, _connectStr);
            }

            _dbLazy = new Lazy<SQLiteConnection>(
                () => new SQLiteConnection(_connectStr), 
                LazyThreadSafetyMode.ExecutionAndPublication);
            
            _dbAsyncLazy = new Lazy<SQLiteAsyncConnection>(
                () => new SQLiteAsyncConnection(_connectStr), 
                LazyThreadSafetyMode.ExecutionAndPublication);
        }

        #region ConnectionString
        public void SetConnectionString(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            var currentConnStr = _configProvider.GetConnectionString(_connectionName);
            if (connectionString.Equals(currentConnStr))
            {
                return;
            }

            _configProvider.SetConnectionString(_connectionName, connectionString);
            ReinitializeConnections(connectionString);
        }
        #endregion

        public override void Disconnect()
        {
            ReinitializeConnections(_connectStr);
        }

        private void ReinitializeConnections(string? newConnectionString)
        {
            lock (_sync)
            {
                DisposeExistingConnections();

                if (!string.IsNullOrWhiteSpace(newConnectionString))
                {
                    _connectStr = newConnectionString;
                }

                _dbLazy = new Lazy<SQLiteConnection>(
                    () => new SQLiteConnection(_connectStr), 
                    LazyThreadSafetyMode.ExecutionAndPublication);
                
                _dbAsyncLazy = new Lazy<SQLiteAsyncConnection>(
                    () => new SQLiteAsyncConnection(_connectStr), 
                    LazyThreadSafetyMode.ExecutionAndPublication);
            }
        }

        private void DisposeExistingConnections()
        {
            try
            {
                if (_dbLazy != null && _dbLazy.IsValueCreated)
                {
                    try { _dbLazy.Value.Close(); } catch { }
                    try { _dbLazy.Value.Dispose(); } catch { }
                }
            }
            catch { }

            try
            {
                if (_dbAsyncLazy != null && _dbAsyncLazy.IsValueCreated)
                {
                    try { _dbAsyncLazy.Value.CloseAsync().GetAwaiter().GetResult(); } catch { }
                }
            }
            catch { }
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
                DisposeExistingConnections();
                _dbLazy = null!;
                _dbAsyncLazy = null!;
            }
            _disposed = true;
        }
    }
}
