using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTools
{
	public static class LinqExtensions
	{
		public static Dictionary<TKey, TValue> MergeDictionaries<TKey, TValue>(params IDictionary<TKey, TValue>[] dictionaries)
		{
			var mergedDictionary = new Dictionary<TKey, TValue>();
			foreach (var dictionary in dictionaries)
			{
				foreach (var kv in dictionary)
				{
					mergedDictionary[kv.Key] = kv.Value;
				}
			}
			return mergedDictionary;
		}

		public static void ConcatDictionary<TKey, TValue>(this IDictionary<TKey, TValue> originalDic, params IDictionary<TKey, TValue>[] dictionaries)
		{
			foreach (var dictionary in dictionaries)
			{
				foreach (var kv in dictionary)
				{
					originalDic[kv.Key] = kv.Value;
				}
			}
		}
	}
}
