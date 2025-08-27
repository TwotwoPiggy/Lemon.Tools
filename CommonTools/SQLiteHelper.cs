using CommonTools.Base;
using SQLite;
using System;
using System.IO;
using System.Threading;
using EnvMananger = CommonTools.EnvironmentManager;

namespace CommonTools
{
	public class SQLiteHelper : DbBase
	{

		#region fields and properties
		public SQLiteConnection Db => _dbLazy.Value;
		public SQLiteAsyncConnection DbAsync => _dbAsyncLazy.Value;

		private string _connectStr = string.Empty;
		private readonly string _configName = string.Empty;
		private Lazy<SQLiteConnection> _dbLazy;
		private Lazy<SQLiteAsyncConnection> _dbAsyncLazy;
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
			_dbLazy = new (new SQLiteConnection(_connectStr));
			_dbAsyncLazy = new (new SQLiteAsyncConnection(_connectStr));
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
			Disconnect();
			DbAsync.CloseAsync().ConfigureAwait(false);
			_dbLazy = new(new SQLiteConnection(connectionString));
			_dbAsyncLazy = new(new SQLiteAsyncConnection(connectionString));
		}
		#endregion

		public override void Disconnect()
		{
			try
			{
				Db.Close();
				Db.Dispose();
				DbAsync.CloseAsync();
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
