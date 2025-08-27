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

		public static void ConcatDictionary<TKey, TValue>(this IDictionary<TKey, TValue> originalDic, params IDictionary<TKey, object>[] dictionaries)
		{
			var needToLower = false;
			foreach (var dictionary in dictionaries)
			{
				foreach (var kv in dictionary)
				{
					//when an object with Boolean converted to a string type, the value true/false will be capitalized => True/False
					needToLower = typeof(TValue) == typeof(string) && kv.Value.GetType() == typeof(bool);
					originalDic[kv.Key] = needToLower 
										? (TValue)Convert.ChangeType(kv.Value.ToString().ToLower(), typeof(TValue)) 
										: (TValue)Convert.ChangeType(kv.Value, typeof(TValue));
				}
			}
		}
	}
}
