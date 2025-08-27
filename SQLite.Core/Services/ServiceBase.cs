using SQLite.Core.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLite.Core.Services
{
	public class ServiceBase
	{
		protected readonly IRepository _repo;
		public ServiceBase(IRepository repo)
		{
			_repo = repo;
		}

	}
}
