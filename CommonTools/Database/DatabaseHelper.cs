using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using CommonTools.Base;

namespace CommonTools
{
    /// <summary>
    /// 通用 ADO.NET 数据库帮助类，支持多种数据库提供者。
    /// <para>
    /// 使用适配器模式（IDbProviderAdapter）实现多数据库支持，
    /// 通过 ProviderAdapterRegistry 注册和获取数据库适配器。
    /// </para>
    /// <para>
    /// 注意：此类与 SQLiteHelper 功能有重叠。SQLiteHelper 专用于 sqlite-net-pcl ORM，
    /// 提供更丰富的 ORM 功能；此类提供通用的 ADO.NET 访问，支持 SQL Server、SQLite 等。
    /// </para>
    /// </summary>
    /// <remarks>
    /// 设计说明：
    /// - 优先使用此类进行通用数据库操作
    /// - 如需 sqlite-net-pcl 的 ORM 功能（如自动建表、关系映射等），使用 SQLiteHelper
    /// - 两者可以共存，根据场景选择
    /// </remarks>
    public class DatabaseHelper : IDatabaseHelper
    {
        /// <inheritdoc/>
        public DatabaseProvider Provider => _adapter.Provider;

        /// <summary>
        /// 获取数据库连接字符串。
        /// </summary>
        public string ConnectionString { get; }

        /// <summary>
        /// 获取数据库提供者适配器（内部使用）。
        /// </summary>
        internal IDbProviderAdapter Adapter => _adapter;

        private readonly IDbProviderAdapter _adapter;
        private readonly SemaphoreSlim _connectionLock = new(1, 1);
        private DbConnection? _connection;
        private bool _disposed;

        /// <summary>
        /// 初始化 <see cref="DatabaseHelper"/> 类的新实例。
        /// </summary>
        /// <param name="adapter">数据库提供者适配器。</param>
        /// <param name="connectionString">数据库连接字符串。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="adapter"/> 或 <paramref name="connectionString"/> 为 null。
        /// </exception>
        public DatabaseHelper(IDbProviderAdapter adapter, string connectionString)
        {
            _adapter = adapter ?? throw new ArgumentNullException(nameof(adapter));
            ConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        /// <summary>
        /// 初始化 <see cref="DatabaseHelper"/> 类的新实例，使用注册表中的适配器。
        /// </summary>
        /// <param name="provider">数据库提供者类型。</param>
        /// <param name="connectionString">数据库连接字符串。</param>
        /// <exception cref="ArgumentNullException"><paramref name="connectionString"/> 为 null。</exception>
        /// <exception cref="KeyNotFoundException">指定的提供者未在注册表中注册。</exception>
        public DatabaseHelper(DatabaseProvider provider, string connectionString)
            : this(ProviderAdapterRegistry.GetAdapter(provider), connectionString)
        {
        }

        /// <summary>
        /// 异步获取打开的数据库连接。
        /// </summary>
        /// <returns>已打开的数据库连接。</returns>
        /// <exception cref="ObjectDisposedException">对象已被释放。</exception>
        /// <exception cref="InvalidOperationException">无法创建数据库连接。</exception>
        public async Task<DbConnection> GetOpenConnectionAsync()
        {
            if (_disposed) throw new ObjectDisposedException(nameof(DatabaseHelper));

            if (_connection != null && _connection.State == ConnectionState.Open)
                return _connection;

            await _connectionLock.WaitAsync().ConfigureAwait(false);
            try
            {
                if (_disposed) throw new ObjectDisposedException(nameof(DatabaseHelper));

                if (_connection == null)
                {
                    _connection = _adapter.CreateConnection();
                    if (_connection == null)
                        throw new InvalidOperationException("Unable to create a database connection for the selected provider. Ensure the appropriate ADO.NET provider is available at runtime.");
                    _connection.ConnectionString = ConnectionString;
                }

                if (_connection.State != ConnectionState.Open)
                {
                    await _connection.OpenAsync().ConfigureAwait(false);
                }

                return _connection;
            }
            finally
            {
                _connectionLock.Release();
            }
        }

        /// <summary>
        /// 异步执行非查询 SQL 语句。
        /// </summary>
        /// <param name="sql">SQL 语句。</param>
        /// <param name="parameters">参数集合（可选）。</param>
        /// <returns>受影响的行数。</returns>
        public async Task<int> ExecuteNonQueryAsync(string sql, IEnumerable<DbParameter>? parameters = null)
        {
            var conn = await GetOpenConnectionAsync().ConfigureAwait(false);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            if (parameters != null)
            {
                foreach (var p in parameters)
                {
                    cmd.Parameters.Add(p);
                }
            }
            return await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// 异步执行标量查询。
        /// </summary>
        /// <param name="sql">SQL 语句。</param>
        /// <param name="parameters">参数集合（可选）。</param>
        /// <returns>查询结果的第一行第一列，如果没有结果则返回 null。</returns>
        public async Task<object?> ExecuteScalarAsync(string sql, IEnumerable<DbParameter>? parameters = null)
        {
            var conn = await GetOpenConnectionAsync().ConfigureAwait(false);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            if (parameters != null)
            {
                foreach (var p in parameters)
                {
                    cmd.Parameters.Add(p);
                }
            }
            return await cmd.ExecuteScalarAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// 异步执行查询并返回字典列表。
        /// </summary>
        /// <param name="sql">SQL 语句。</param>
        /// <param name="parameters">参数集合（可选）。</param>
        /// <returns>查询结果的字典列表，每行一个字典。</returns>
        public async Task<List<Dictionary<string, object?>>> QueryAsync(string sql, IEnumerable<DbParameter>? parameters = null)
        {
            var conn = await GetOpenConnectionAsync().ConfigureAwait(false);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            if (parameters != null)
            {
                foreach (var p in parameters)
                {
                    cmd.Parameters.Add(p);
                }
            }

            using var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);
            var result = new List<Dictionary<string, object?>>();
            while (await reader.ReadAsync().ConfigureAwait(false))
            {
                var row = new Dictionary<string, object?>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    row[reader.GetName(i)] = await reader.IsDBNullAsync(i).ConfigureAwait(false) ? null : reader.GetValue(i);
                }
                result.Add(row);
            }
            return result;
        }

        /// <summary>
        /// 释放资源。
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
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
                    if (_connection != null)
                    {
                        _connection.Close();
                        _connection.Dispose();
                        _connection = null;
                    }
                }
                finally
                {
                    _connectionLock.Release();
                    _connectionLock.Dispose();
                }
            }
            _disposed = true;
        }
    }
}
