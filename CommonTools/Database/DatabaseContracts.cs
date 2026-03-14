using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace CommonTools
{
    public enum DatabaseProvider
    {
        Unknown,
        SqlServer,
        MySql,
        Sqlite
    }

    public interface IDatabaseHelper : IDisposable
    {
        DatabaseProvider Provider { get; }
        string ConnectionString { get; }
        Task<DbConnection> GetOpenConnectionAsync();
        Task<int> ExecuteNonQueryAsync(string sql, IEnumerable<DbParameter>? parameters = null);
        Task<object?> ExecuteScalarAsync(string sql, IEnumerable<DbParameter>? parameters = null);
        Task<List<Dictionary<string, object?>>> QueryAsync(string sql, IEnumerable<DbParameter>? parameters = null);
    }

    // Adapter interface - implement per-provider without switching
    public interface IDbProviderAdapter
    {
        DatabaseProvider Provider { get; }
        DbConnection? CreateConnection();
        DbParameter CreateParameter(string name, object? value, DbType? dbType = null, ParameterDirection direction = ParameterDirection.Input, int? size = null);
        IEnumerable<DbParameter> CreateParameters(IEnumerable<KeyValuePair<string, object?>> values) => values.Select(kv => CreateParameter(kv.Key, kv.Value));
    }
}
