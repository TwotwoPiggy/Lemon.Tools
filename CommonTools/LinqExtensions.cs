using System;
using System.Collections.Generic;
using System.Data;
using System.Collections;
using System.Linq.Expressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTools
{
	public static class LinqExtensions
	{
		public static Dictionary<TKey, TValue> MergeDictionaries<TKey, TValue>(params IDictionary<TKey, TValue>[] dictionaries)
		{
			if (dictionaries == null || dictionaries.Length == 0)
				return [];

			// Try to pre-allocate capacity based on counts of provided dictionaries when available
			int capacity = 0;
			foreach (var dic in dictionaries)
			{
				if (dic is ICollection<KeyValuePair<TKey, TValue>> coll)
					capacity += coll.Count;
			}
			var mergedDictionary = capacity > 0 ? new Dictionary<TKey, TValue>(capacity) : new Dictionary<TKey, TValue>();

			foreach (var dictionary in dictionaries)
			{
				if (dictionary == null) continue;
				foreach (var kv in dictionary)
				{
					mergedDictionary[kv.Key] = kv.Value;
				}
			}
			return mergedDictionary;
		}

		public static void ConcatDictionary<TKey, TValue>(this IDictionary<TKey, TValue> originalDic, params IDictionary<TKey, object>[] dictionaries)
		{
			if (originalDic == null) throw new ArgumentNullException(nameof(originalDic));
			if (dictionaries == null || dictionaries.Length == 0) return;

			var targetIsString = typeof(TValue) == typeof(string);
			var targetType = typeof(TValue);

			foreach (var dictionary in dictionaries)
			{
				if (dictionary == null) continue;
				foreach (var kv in dictionary)
				{
					var key = kv.Key;
					var val = kv.Value;
					if (val == null)
					{
						originalDic[key] = default!;
						continue;
					}

					if (targetIsString)
					{
						// common fast paths for strings
						if (val is bool b)
						{
							originalDic[key] = (TValue)(object)b.ToString().ToLowerInvariant();
							continue;
						}
						if (val is string s)
						{
							originalDic[key] = (TValue)(object)s;
							continue;
						}
						// fallback to Convert for other types
						originalDic[key] = (TValue)Convert.ChangeType(val, targetType);
					}
					// If value already has the target type, avoid conversion
					if (val is TValue tv)
					{
						originalDic[key] = tv;
						continue;
					}
					// last resort: try Convert.ChangeType
					originalDic[key] = (TValue)Convert.ChangeType(val, targetType);
				}
			}
		}

		/// <summary>
		/// Apply a where predicate only when the provided condition value is not considered empty.
		/// If the value is null, an empty/whitespace string, an empty enumerable, or a value-type equal to its default
		/// the original source is returned without applying the predicate.
		/// </summary>
		public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, object conditionValue, Expression<Func<T, bool>> predicate)
		{
			if (IsNullOrEmptyValue(conditionValue)) return source;
			return source.Where(predicate);
		}

		/// <summary>
		/// Faster overload when you already have a boolean condition. Avoids evaluating emptiness checks.
		/// </summary>
		public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, bool condition, Expression<Func<T, bool>> predicate)
		{
			return condition ? source.Where(predicate) : source;
		}

		/// <summary>
		/// IEnumerable overload of WhereIf.
		/// </summary>
		public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, object conditionValue, Func<T, bool> predicate)
		{
			if (IsNullOrEmptyValue(conditionValue)) return source;
			return source.Where(predicate);
		}

		/// <summary>
		/// Faster overload when you already have a boolean condition. Avoids evaluating emptiness checks.
		/// </summary>
		public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, bool condition, Func<T, bool> predicate)
		{
			return condition ? source.Where(predicate) : source;
		}

		private static bool IsNullOrEmptyValue(object value)
		{
			if (value == null) return true;
			if (value is string s) return string.IsNullOrWhiteSpace(s);
			if (value is Guid g) return g == Guid.Empty;
			// handle enumerables (but exclude string which is handled above)
			if (value is IEnumerable en)
			{
				// fast path for collections
				if (value is System.Collections.ICollection coll) return coll.Count == 0;
				// fallback: try to iterate one element
				var enumerator = en.GetEnumerator();
				try { return !enumerator.MoveNext(); }
				finally { if (enumerator is IDisposable d) d.Dispose(); }
			}
			var type = value.GetType();
			if (type.IsValueType)
			{
				var defaultValue = Activator.CreateInstance(type);
				return value.Equals(defaultValue);
			}
			return false;
		}


	}
}
