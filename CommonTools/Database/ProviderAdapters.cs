using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace CommonTools.Database
{
    public class SqlServerAdapter : IDbProviderAdapter
    {
        public DatabaseProvider Provider => DatabaseProvider.SqlServer;

        private const string InvariantName = "Microsoft.Data.SqlClient";

        public DbConnection? CreateConnection()
        {
            try
            {
                var factory = DbProviderFactories.GetFactory(InvariantName);
                return factory.CreateConnection();
            }
            catch
            {
                throw new InvalidOperationException($"Provider '{InvariantName}' is not available. Add Microsoft.Data.SqlClient or register the provider.");
            }
        }

        public DbParameter CreateParameter(string name, object? value, DbType? dbType = null, ParameterDirection direction = ParameterDirection.Input, int? size = null)
        {
            var factory = DbProviderFactories.GetFactory(InvariantName);
            var p = factory.CreateParameter();
            p.ParameterName = name;
            p.Value = value ?? DBNull.Value;
            p.Direction = direction;
            if (dbType.HasValue) p.DbType = dbType.Value;
            if (size.HasValue) p.Size = size.Value;
            return p;
        }
    }

    public class SqliteAdapter : IDbProviderAdapter
    {
        public DatabaseProvider Provider => DatabaseProvider.Sqlite;

        private const string InvariantName = "Microsoft.Data.Sqlite";

        public DbConnection? CreateConnection()
        {
            try
            {
                var factory = DbProviderFactories.GetFactory(InvariantName);
                return factory.CreateConnection();
            }
            catch
            {
                throw new InvalidOperationException($"Provider '{InvariantName}' is not available. Add Microsoft.Data.Sqlite or register the provider.");
            }
        }

        public DbParameter CreateParameter(string name, object? value, DbType? dbType = null, ParameterDirection direction = ParameterDirection.Input, int? size = null)
        {
            var factory = DbProviderFactories.GetFactory(InvariantName);
            var p = factory.CreateParameter();
            p.ParameterName = name;
            p.Value = value ?? DBNull.Value;
            p.Direction = direction;
            if (dbType.HasValue) p.DbType = dbType.Value;
            if (size.HasValue) p.Size = size.Value;
            return p;
        }
    }
}
