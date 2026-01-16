using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTools
{
	public static class EnumExtensions
	{
		private static readonly ConcurrentDictionary<Enum, string> _descriptionCache = new();

		public static T GetEnumFromDescription<T>(this string description) where T : Enum
		{
			var values = typeof(T).GetEnumValues();
			return values.OfType<T>().FirstOrDefault(enumValue => enumValue.GetEnumDescription() == description);
		}

		public static string GetEnumDescription(this Enum enumValue)
		{
			return _descriptionCache.GetOrAdd(enumValue, e =>
			{
				var fieldInfo = e.GetType().GetField(e.ToString());
				var attributes = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
				if (attributes != null && attributes.Any())
				{
					return attributes.FirstOrDefault()?.Description;
				}
				return e.ToString();
			});
		}
	}
}
