using SQLite.Core.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLite.Core.Services
{
	public class GenericServiceBase<T> : ServiceBase, IServiceBase
	{
		public GenericServiceBase(IRepository repo, bool needMigrate) : base(repo)
		{
			if (needMigrate)
				Migrate<T>();
		}

		public void Migrate<U>()
		{
			_repo.Migrate<U>();
		}
	}
}
