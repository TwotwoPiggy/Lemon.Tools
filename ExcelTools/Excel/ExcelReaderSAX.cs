using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using ExcelTools.ExcelAttributes;
using ExcelTools.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExcelTools.Excel
{
	/// <summary>
	/// 通过SAX方法读取excel文件
	/// </summary>
	public class ExcelReaderSAX : ExcelReader
	{
		public override Task<List<T>> ConvertExcelToEntityAsync<T>(WorksheetPart worksheetPart, Dictionary<string, string> excelHeaders)
		{
			if (worksheetPart == null)
			{
				throw new ArgumentNullException(nameof(worksheetPart));
			}
			var excelReader = OpenXmlReader.Create(worksheetPart);

			T entityData;
			PropertyInfo entityProperty;
			Row row;
			string cellPosition;
			string cellValue;
			Dictionary<string, bool> boolValueDic;
			BoolValueConvertAttribute boolValueConvertAttribute;
			var result = new List<T>();
			return Task.Run(()=>
			{
				while (excelReader.Read())
				{
					entityData = new T();
					if (excelReader.ElementType != typeof(Row))
					{
						continue;
					}
					row = excelReader.LoadCurrentElement() as Row;
					if (row.RowIndex == 1)
					{
						continue;
					}
					foreach (var cell in row.Elements<Cell>())
					{
						cellPosition = cell.CellReference.Value.ToCellPosition();
						cellValue = cell.CellValue.Text;
						entityProperty = entityData
												.GetType()
												.GetProperties()
												.FirstOrDefault(property =>
													property
														.GetCustomAttributes(true)
														.OfType<ExcelHeaderAttribute>()
														.FirstOrDefault()
														.HeaderName.Trim() == excelHeaders[cellPosition]
												);
						if (entityProperty == null)
						{
							continue;
						}
						if (entityProperty.PropertyType != typeof(bool))
						{
							entityProperty.SetValue(entityData, Convert.ChangeType(cellValue, entityProperty.PropertyType));
						}
						else
						{
							boolValueConvertAttribute = entityProperty.GetCustomAttributes(true).OfType<BoolValueConvertAttribute>().FirstOrDefault();
							boolValueDic = boolValueConvertAttribute?.BoolValues;
							entityProperty.SetValue(entityData, boolValueDic.ContainsKey(cellValue) ? boolValueDic[cellValue] : boolValueConvertAttribute.DefaultBool);
						}
					}
					result.Add(entityData);
				}
				return result;
			});
		}
	}
}
