using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExcelTools.ExcelAttributes
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
	public class ExcelHeaderAttribute : Attribute
	{
		public string HeaderName { get; }

		public int? HeaderIndex { get; }

		public string HeaderPosition { get; }
		/// <summary>
		/// excel表头特性
		/// </summary>
		/// <param name="headerName">表头名称</param>
		/// <param name="headerIndex">表头序号,从1开始,最大为16384</param>
		/// <param name="headerPosition">表头位置字母,ex:AA BB A B</param>
		public ExcelHeaderAttribute(string headerName, int headerIndex = -1, string headerPosition = null)
		{
			HeaderName = headerName;
			HeaderPosition = headerPosition;
			HeaderIndex = headerIndex < 0 ? default(int?) : headerIndex;
		}
	}

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
	public class BoolValueConvertAttribute : Attribute
	{
		public Dictionary<string, bool> BoolValues { get; }

		public bool DefaultBool { get; }
		/// <summary>
		/// 标识自定义字段类型为boolean类型的数据,(导入操作时使用)
		/// </summary>
		/// <param name="trueValues">true值对应的字符串类型数据,可传入多个</param>
		/// <param name="falseValues">false值对应的字符串类型数据,可传入多个</param>
		/// <param name="defaultBool">标识自定义boolean类型字段的默认值</param>
		public BoolValueConvertAttribute(string[] trueValues, string[] falseValues, bool defaultBool = false)
		{
			BoolValues = new Dictionary<string, bool>();
			if (trueValues.Length + falseValues.Length < 1)
			{
				throw new ArgumentNullException($"both params {nameof(trueValues)} and {nameof(falseValues)} are empty");
			}
			DefaultBool = defaultBool;
			try
			{
				BoolValues = trueValues.ToDictionary(key => key, value => true);
				foreach (var item in falseValues)
				{
					BoolValues.Add(item, false);
				}
			}
			catch (ArgumentException)
			{
				throw new ArgumentException("An element with Key already exists.");
			}

		}
	}


}
