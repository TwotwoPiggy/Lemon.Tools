using CommonTools.Base;
using CommonTools.Configuration;
using Microsoft.Extensions.Configuration;
using SQLite;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CommonTools.Database
{
    /// <summary>
    /// SQLite 数据库帮助类，基于 sqlite-net-pcl ORM。
    /// <para>
    /// 提供同步和异步的 SQLite 数据库访问，支持 ORM 功能（自动建表、关系映射等）。
    /// </para>
    /// <para>
    /// 注意：此类与 DatabaseHelper 功能有重叠。DatabaseHelper 提供通用 ADO.NET 访问，
    /// 支持多种数据库；此类专用于 sqlite-net-pcl，提供更丰富的 ORM 功能。
    /// </para>
    /// </summary>
    /// <remarks>
    /// 设计说明：
    /// - 如需 sqlite-net-pcl 的 ORM 功能，使用此类
    /// - 如需通用数据库访问或支持其他数据库，使用 DatabaseHelper
    /// - 两者可以共存，根据场景选择
    /// </remarks>
    public class SQLiteHelper : DbBase, IDisposable, IAsyncDisposable
    {
        #region Properties

        /// <summary>
        /// 获取同步数据库连接。
        /// </summary>
        /// <exception cref="ObjectDisposedException">对象已被释放。</exception>
        public SQLiteConnection Db
        {
            get
            {
                if (_disposed) throw new ObjectDisposedException(nameof(SQLiteHelper));
                return _dbLazy?.Value ?? throw new ObjectDisposedException(nameof(SQLiteHelper));
            }
        }

        /// <summary>
        /// 获取异步数据库连接。
        /// </summary>
        /// <exception cref="ObjectDisposedException">对象已被释放。</exception>
        public SQLiteAsyncConnection DbAsync
        {
            get
            {
                if (_disposed) throw new ObjectDisposedException(nameof(SQLiteHelper));
                return _dbAsyncLazy?.Value ?? throw new ObjectDisposedException(nameof(SQLiteHelper));
            }
        }

        private string _connectStr = string.Empty;
        private readonly string _connectionName;
        private Lazy<SQLiteConnection>? _dbLazy;
        private Lazy<SQLiteAsyncConnection>? _dbAsyncLazy;
        private readonly SemaphoreSlim _connectionLock = new(1, 1);
        private volatile bool _disposed;
        private readonly IConnectionConfigProvider _configProvider;

        #endregion

        #region Constructors

        /// <summary>
        /// 初始化 <see cref="SQLiteHelper"/> 类的新实例。
        /// </summary>
        /// <param name="connectionName">连接名称（默认为 "Default"）。</param>
        /// <param name="configuration">配置对象（可选）。</param>
        public SQLiteHelper(string connectionName = "Default", IConfiguration? configuration = null)
        {
            _connectionName = connectionName;
            _configProvider = ConnectionConfigProviderFactory.Create(configuration);

            InitializeConnections();
        }

        /// <summary>
        /// 初始化 <see cref="SQLiteHelper"/> 类的新实例。
        /// </summary>
        /// <param name="configProvider">连接配置提供者。</param>
        /// <param name="connectionName">连接名称（默认为 "Default"）。</param>
        /// <exception cref="ArgumentNullException"><paramref name="configProvider"/> 为 null。</exception>
        public SQLiteHelper(IConnectionConfigProvider configProvider, string connectionName = "Default")
        {
            _connectionName = connectionName;
            _configProvider = configProvider ?? throw new ArgumentNullException(nameof(configProvider));

            InitializeConnections();
        }

        #endregion

        #region Private Methods

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

        private void ReinitializeConnections(string? newConnectionString)
        {
            _connectionLock.Wait();
            try
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
            finally
            {
                _connectionLock.Release();
            }
        }

        private async Task ReinitializeConnectionsAsync(string? newConnectionString)
        {
            await _connectionLock.WaitAsync().ConfigureAwait(false);
            try
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
            finally
            {
                _connectionLock.Release();
            }
        }

        private void DisposeExistingConnections()
        {
            if (_dbLazy?.IsValueCreated == true)
            {
                _dbLazy.Value.Close();
                _dbLazy.Value.Dispose();
            }

            if (_dbAsyncLazy?.IsValueCreated == true)
            {
                _dbAsyncLazy.Value.CloseAsync().GetAwaiter().GetResult();
            }
        }

        private async Task DisposeExistingConnectionsAsync()
        {
            if (_dbLazy?.IsValueCreated == true)
            {
                _dbLazy.Value.Close();
                _dbLazy.Value.Dispose();
            }

            if (_dbAsyncLazy?.IsValueCreated == true)
            {
                await _dbAsyncLazy.Value.CloseAsync().ConfigureAwait(false);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 设置新的连接字符串并重新初始化连接。
        /// </summary>
        /// <param name="connectionString">新的连接字符串。</param>
        /// <exception cref="ArgumentNullException"><paramref name="connectionString"/> 为 null 或空白。</exception>
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

        /// <summary>
        /// 异步设置新的连接字符串并重新初始化连接。
        /// </summary>
        /// <param name="connectionString">新的连接字符串。</param>
        /// <param name="cancellationToken">取消令牌。</param>
        /// <exception cref="ArgumentNullException"><paramref name="connectionString"/> 为 null 或空白。</exception>
        public async Task SetConnectionStringAsync(string connectionString, CancellationToken cancellationToken = default)
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
            await ReinitializeConnectionsAsync(connectionString).ConfigureAwait(false);
        }

        /// <summary>
        /// 断开并重新初始化连接。
        /// </summary>
        public override void Disconnect()
        {
            ReinitializeConnections(_connectStr);
        }

        #endregion

        #region IDisposable

        /// <summary>
        /// 释放资源。
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 异步释放资源。
        /// </summary>
        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore().ConfigureAwait(false);
            Dispose(false);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放资源的核心方法。
        /// </summary>
        /// <param name="disposing">是否释放托管资源。</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
                _connectionLock.Wait();
                try
                {
                    DisposeExistingConnections();
                    _dbLazy = null;
                    _dbAsyncLazy = null;
                }
                finally
                {
                    _connectionLock.Release();
                    _connectionLock.Dispose();
                }
            }
            _disposed = true;
        }

        /// <summary>
        /// 异步释放资源的核心方法。
        /// </summary>
        protected virtual async ValueTask DisposeAsyncCore()
        {
            if (_disposed) return;

            await _connectionLock.WaitAsync().ConfigureAwait(false);
            try
            {
                await DisposeExistingConnectionsAsync().ConfigureAwait(false);
                _dbLazy = null;
                _dbAsyncLazy = null;
            }
            finally
            {
                _connectionLock.Release();
            }

            _disposed = true;
        }

        #endregion
    }
}
