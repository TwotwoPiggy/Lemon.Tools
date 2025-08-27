using CommonTools;
using SQLite.Core.interfaces;

namespace SQLite.Core.Repositories
{
	public abstract class RepositoryBase : IRepositoryBase
	{
		protected readonly SQLiteHelper _sqliteHelper;
		public RepositoryBase(SQLiteHelper sqliteHelper) => _sqliteHelper = sqliteHelper;

		public void Migrate<T>()
		{
			_sqliteHelper.Db.CreateTable<T>();
		}

		public void Dispose()
		{
			_sqliteHelper.Disconnect();
		}
	}
}
