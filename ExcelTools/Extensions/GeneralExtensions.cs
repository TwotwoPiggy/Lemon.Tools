using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
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

		public static bool EqualsString(this string strEqualed, string strToEqual)
		{
			return strEqualed.Trim().Equals(strToEqual, StringComparison.CurrentCultureIgnoreCase);
		}

		/// <summary>
		/// 获取cell的位置
		/// </summary>
		/// <param name="position"></param>
		/// <returns></returns>
		public static string GetCellPosition(this string position)
		{
			var pattern = @"^[A-Za-z]*";
			return Regex.Match(position, pattern, RegexOptions.IgnoreCase)?.Value;
		}

		/// <summary>
		/// 获取cell值
		/// </summary>
		/// <param name="cell"></param>
		/// <param name="stringTable"></param>
		/// <returns></returns>
		public static string GetCellValue(this Cell cell, SharedStringTablePart stringTable)
		{
			var cellValue = cell.CellValue.InnerText;

			if (cell.DataType != null)
			{
				switch (cell.DataType.Value)
				{
					case CellValues.Boolean:
						break;
					case CellValues.Number:
						break;
					case CellValues.Error:
						break;
					case CellValues.SharedString:
						if (stringTable != null)
						{
							cellValue = stringTable.SharedStringTable.ElementAt(int.Parse(cellValue)).InnerText.Trim();
						}
						break;
					case CellValues.String:
						break;
					case CellValues.InlineString:
						break;
					case CellValues.Date:
						break;
					default:
						break;
				}
			}
			return cellValue;
		}


		/// <summary>
		/// 根据自定义特性来获取对象属性
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
