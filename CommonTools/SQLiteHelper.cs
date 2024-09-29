using CommonTools.Base;
using SQLite;
using System;
using System.IO;
using EnvMananger = CommonTools.EnvironmentManager;

namespace CommonTools
{
	public class SQLiteHelper : DbBase
	{

		#region fields and properties
		//private static readonly Lazy<SQLiteHelper> _instance = new(() => new());
		//public static SQLiteHelper Instance => _instance.Value;
		public SQLiteConnection Db => _dbLazy.Value;
		public SQLiteAsyncConnection DbAsync => _dbAsyncLazy.Value;

		private string _connectStr = string.Empty;
		private Lazy<SQLiteConnection> _dbLazy;
		private Lazy<SQLiteAsyncConnection> _dbAsyncLazy;
		#endregion

		public SQLiteHelper()
		{
			_connectStr = ConfigManager.GetConnectionString("SQLite");
			if (string.IsNullOrWhiteSpace(_connectStr))
			{
				_connectStr = @".\default.db";
				ConfigManager.SetConnectionString("SQLite", _connectStr, SqlLiteProvider);
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
			var currentConnStr = ConfigManager.GetConnectionString("SQLite");
			if (connectionString.Equals(currentConnStr))
			{
				return;
			}
			ConfigManager.SetConnectionString("SQLite", connectionString, SqlLiteProvider);
			Disconnect();
			DbAsync.CloseAsync().ConfigureAwait(false);
			_dbLazy = new(new SQLiteConnection(connectionString));
			_dbAsyncLazy = new(new SQLiteAsyncConnection(connectionString));
		}
		#endregion

		protected override void Disconnect()
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
