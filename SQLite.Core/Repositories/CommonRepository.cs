﻿using CommonTools;
using SQLite.Core.interfaces;
using SQLite.Core.Utils;
using SQLiteNetExtensions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SQLite.Core.Repositories
{
	public class CommonRepository(SQLiteHelper sqliteHelper) : RepositoryBase(sqliteHelper), IRepository
	{
		public void Add<T>(T entity) => _sqliteHelper.Db.Insert(entity);

		public void BatchAdd<T>(IEnumerable<T> entities) => _sqliteHelper.Db.InsertAll(entities);

		public void Update<T>(T entity) where T : new() => _sqliteHelper.Db.Update(entity);

		public T? Query<T>(Expression<Func<T, bool>> predExpr, bool loadChildren = false, bool recursive = false) where T : new()
		{
			if (loadChildren)
			{
				return _sqliteHelper.Db.GetAllWithChildren(predExpr, recursive: recursive)!.AsQueryable().FirstOrDefault(predExpr);
			}
			else
			{
				return _sqliteHelper.Db.Table<T>().FirstOrDefault(predExpr);
			}
		}

		public IEnumerable<T> QueryList<T>(Expression<Func<T, bool>>? predExpr = null, bool loadChildren = false, bool recursive = false) where T : new()
		{
			if (loadChildren)
			{
				return predExpr == null ? _sqliteHelper.Db.GetAllWithChildren<T>(recursive: recursive) : _sqliteHelper.Db.GetAllWithChildren<T>(predExpr, recursive: recursive);
			}
			else
			{
				return predExpr == null ? _sqliteHelper.Db.Table<T>().AsQueryable() : _sqliteHelper.Db.Table<T>().Where(predExpr);
			}
		}

		public IEnumerable<T> FuzzyQuery<T>(string query, bool loadChilden = false, bool recursive = false) where T : new()
		{
			var queryResults = _sqliteHelper.Db.Query<T>(query);
			if (loadChilden)
			{
				queryResults.GetChildren(_sqliteHelper.Db);
			}
			return queryResults;
		}

		public IEnumerable<T> FuzzyQueryWithChildren<T>(string query, bool loadChilden = false, bool recursive = false, params object[] parameters) where T : new()
		{
			var queryResults = _sqliteHelper.Db.Query<T>(query, parameters);
			if (loadChilden)
			{
				queryResults.GetChildren(_sqliteHelper.Db);
			}
			return queryResults;
		}

		public void Delete<T>(object key) => _sqliteHelper.Db.Delete<T>(key);

	}
}
