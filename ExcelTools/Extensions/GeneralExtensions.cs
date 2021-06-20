using ExcelTools.ExcelAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace ExcelTools.Extensions
{
	/// <summary>
	/// 通用扩展方法
	/// </summary>
	public static class GeneralExtensions
	{
		public static bool ContainsString(this object obj, string strSecond)
		{
			if (obj == null || string.IsNullOrWhiteSpace(obj.ToString()))
			{
				throw new ArgumentNullException(nameof(obj));
			}
			return obj.ToString().Replace(" ", "").Contains(strSecond, StringComparison.CurrentCultureIgnoreCase);
		}

		/// <summary>
		/// 获取cell的位置
		/// </summary>
		/// <param name="position"></param>
		/// <returns></returns>
		public static string ToCellPosition(this string position)
		{
			var pattern = @"^[A-Za-z]*";
			return Regex.Match(position, pattern, RegexOptions.IgnoreCase)?.Value;
		}

		/// <summary>
		/// 获取对象的自定义特性
		/// </summary>
		/// <typeparam name="TProperty">自定义特性类型</typeparam>
		/// <param name="object"></param>
		/// <param name="headerName"></param>
		/// <returns></returns>
		public static PropertyInfo GetCustomProperty<TProperty>(this object @object, string headerName) where TProperty : ExcelHeaderAttribute
		{

			return @object
						.GetType()
						.GetProperties()
						.FirstOrDefault(_ => _.GetCustomAttributes(true)
												.OfType<TProperty>()
												.FirstOrDefault()
												.HeaderName.Trim() == headerName
										);
		}
	}
}
