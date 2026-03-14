using System;
using Microsoft.Data.Sqlite;
using System.Threading;
using System.Threading.Tasks;

namespace CommonTools.Database
{
    // Adapter with lazy, thread-safe initialization and safe reinitialization/ disposal
    //public sealed class SqliteAdapter : IDisposable
    //{
    //    private readonly Lock _sync = new();
    //    private readonly string _initialConnectionString;
    //    private Lazy<SqliteConnection> _connLazy;
    //    private bool _disposed;

    //    public SqliteAdapter(string connectionString)
    //    {
    //        _initialConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    //        _connLazy = CreateLazy(_initialConnectionString);
    //    }

    //    private Lazy<SqliteConnection> CreateLazy(string connStr)
    //    {
    //        return new Lazy<SqliteConnection>(() => new SqliteConnection(connStr.Contains('=') ? connStr : $"Data Source={connStr}"), LazyThreadSafetyMode.ExecutionAndPublication);
    //    }

    //    public SqliteConnection Connection
    //    {
    //        get
    //        {
    //            Lazy<SqliteConnection> lazy;
    //            _sync.Enter();
    //            try
    //            {
    //                if (_disposed) throw new ObjectDisposedException(nameof(SqliteAdapter));
    //                if (_connLazy == null) throw new ObjectDisposedException(nameof(SqliteAdapter));
    //                lazy = _connLazy;
    //            }
    //            finally
    //            {
    //                _sync.Exit();
    //            }

    //            return lazy.Value;
    //        }
    //    }

    //    public bool IsValueCreated
    //    {
    //        get
    //        {
    //            _sync.Enter();
    //            try
    //            {
    //                return _connLazy != null && _connLazy.IsValueCreated;
    //            }
    //            finally
    //            {
    //                _sync.Exit();
    //            }
    //        }
    //    }

    //    public void Open()
    //    {
    //        var conn = Connection;
    //        if (conn.State != System.Data.ConnectionState.Open)
    //        {
    //            conn.Open();
    //        }
    //    }

    //    public async Task OpenAsync(CancellationToken cancellationToken = default)
    //    {
    //        var conn = Connection;
    //        if (conn.State != System.Data.ConnectionState.Open)
    //        {
    //            await conn.OpenAsync(cancellationToken).ConfigureAwait(false);
    //        }
    //    }

    //    public void Close()
    //    {
    //        Lazy<SqliteConnection>? old;
    //        _sync.Enter();
    //        try
    //        {
    //            if (_disposed) throw new ObjectDisposedException(nameof(SqliteAdapter));
    //            old = _connLazy;
    //            _connLazy = CreateLazy(_initialConnectionString);
    //        }
    //        finally
    //        {
    //            _sync.Exit();
    //        }

    //        SafeDispose(old);
    //    }

    //    public async Task CloseAsync(CancellationToken cancellationToken = default)
    //    {
    //        Lazy<SqliteConnection>? old;
    //        _sync.Enter();
    //        try
    //        {
    //            if (_disposed) throw new ObjectDisposedException(nameof(SqliteAdapter));
    //            old = _connLazy;
    //            _connLazy = CreateLazy(_initialConnectionString);
    //        }
    //        finally
    //        {
    //            _sync.Exit();
    //        }

    //        await SafeDisposeAsync(old, cancellationToken).ConfigureAwait(false);
    //    }

    //    public void SetConnectionString(string connectionString)
    //    {
    //        if (string.IsNullOrWhiteSpace(connectionString)) throw new ArgumentNullException(nameof(connectionString));
    //        Lazy<SqliteConnection>? old;
    //        _sync.Enter();
    //        try
    //        {
    //            if (_disposed) throw new ObjectDisposedException(nameof(SqliteAdapter));
    //            old = _connLazy;
    //            _connLazy = CreateLazy(connectionString);
    //        }
    //        finally
    //        {
    //            _sync.Exit();
    //        }

    //        SafeDispose(old);
    //    }

    //    public void Dispose()
    //    {
    //        if (_disposed) return;
    //        Lazy<SqliteConnection>? old;
    //        _sync.Enter();
    //        try
    //        {
    //            old = _connLazy;
    //            _connLazy = null;
    //            _disposed = true;
    //        }
    //        finally
    //        {
    //            _sync.Exit();
    //        }

    //        SafeDispose(old);
    //    }

    //    private static void SafeDispose(Lazy<SqliteConnection>? old)
    //    {
    //        if (old != null && old.IsValueCreated)
    //        {
    //            try { old.Value.Close(); } catch { }
    //            try { old.Value.Dispose(); } catch { }
    //        }
    //    }

    //    private static async Task SafeDisposeAsync(Lazy<SqliteConnection>? old, CancellationToken token)
    //    {
    //        if (old != null && old.IsValueCreated)
    //        {
    //            try { await old.Value.CloseAsync().ConfigureAwait(false); } catch { }
    //            try { old.Value.Dispose(); } catch { }
    //        }
    //    }
    //}
}
