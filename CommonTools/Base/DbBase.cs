using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTools.Base
{
	public abstract class DbBase
	{
		#region const fields
		protected static readonly string OdbcProvider = "System.Data.Odbc";
		protected static readonly string AccessProvider = "System.Data.OleDb";
		protected static readonly string SqlLiteProvider = "System.Data.SqlLite";
		protected static readonly string MSSQLProvider = "System.Data.SqlClient";
		protected static readonly string OracleProvider = "System.Data.OracleClient";
		#endregion

		protected abstract void Disconnect();
	}
}
