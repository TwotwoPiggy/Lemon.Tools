using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace CommonTools
{
    public static class DatabaseHelperExtensions
    {
        public static DbParameter CreateParameter(this DatabaseHelper helper, string name, object? value, DbType? dbType = null, System.Data.ParameterDirection direction = System.Data.ParameterDirection.Input, int? size = null)
        {
            if (helper == null) throw new ArgumentNullException(nameof(helper));
            return helper.Adapter.CreateParameter(name, value, dbType, direction, size);
        }

        public static IEnumerable<DbParameter> CreateParameters(this DatabaseHelper helper, IEnumerable<KeyValuePair<string, object?>> values)
        {
            if (helper == null) throw new ArgumentNullException(nameof(helper));
            return helper.Adapter.CreateParameters(values);
        }
    }
}
