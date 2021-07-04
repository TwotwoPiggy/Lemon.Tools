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
