using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SQLite.Core.interfaces
{
	public interface IRepository : IRepositoryBase
	{
		void Add<T>(T entity);
		void BatchAdd<T>(IEnumerable<T> entities);
		public T? Query<T>(Expression<Func<T, bool>> predExpr, bool loadChildren = false, bool recursive = false) where T : new();
		void Update<T>(T entity) where T : new();
		IEnumerable<T> QueryList<T>(Expression<Func<T, bool>>? predExpr = null, bool loadChildren = false, bool recursive = false) where T : new();
		IEnumerable<T> FuzzyQuery<T>(string query, bool loadChilden = false, bool recursive = false) where T : new();
		IEnumerable<T> FuzzyQueryWithChildren<T>(string query, bool loadChilden = false, bool recursive = false, params object[] parameters) where T : new();
		void Delete<T>(object key);
	}
}
