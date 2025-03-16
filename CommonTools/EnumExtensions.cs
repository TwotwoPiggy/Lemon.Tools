using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTools
{
	public static class EnumExtensions
	{
		public static T GetEnumFromDescription<T>(this string description) where T : Enum
		{
			var values = typeof(T).GetEnumValues();
			return values.OfType<T>().FirstOrDefault(enumValue => enumValue.GetEnumDescription() == description);
		}

		public static string GetEnumDescription(this Enum enumValue)
		{
			var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
			var attributes = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
			if (attributes != null && attributes.Any())
			{
				return attributes.FirstOrDefault()?.Description;
			}
			return enumValue.ToString();
		}
	}
}
