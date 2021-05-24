using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using ExcelTools.ExcelAttributes;
using ExcelTools.Extensions;
using ExcelTools.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExcelTools.Excel
{
	/// <summary>
	/// 通过DOM方法读取excel文件
	/// </summary>
	public class ExcelReaderDOM : ExcelReader
	{
		public override Task<List<T>> ConvertExcelToEntityAsync<T>(WorksheetPart worksheetPart,Dictionary<string,string> excelHeaders)
		{
			if (worksheetPart == null)
			{
				throw new ArgumentNullException(nameof(worksheetPart));
			}
			var sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
			T entityData;
			PropertyInfo entityProperty;
			string cellValue;
			string cellPosition;
			Dictionary<string, bool> boolValueDic;
			BoolValueConvertAttribute boolValueConvertAttribute;
			var result = new List<T>();
			return Task.Run(()=>
			{
				foreach (var row in sheetData.Elements<Row>().Where(row => row.RowIndex != 1))
				{
					entityData = new T();
					foreach (var cell in row.Elements<Cell>())
					{
						cellPosition = cell.CellReference.Value.ToCellPosition();
						cellValue = cell.CellValue.Text;
						entityProperty = entityData.GetType()
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
						if (entityProperty.PropertyType == typeof(bool))
						{
							boolValueConvertAttribute = entityProperty.GetCustomAttributes(true).OfType<BoolValueConvertAttribute>().FirstOrDefault();
							boolValueDic = boolValueConvertAttribute.BoolValues;
							entityProperty.SetValue(entityData, boolValueDic.ContainsKey(cellValue) ? boolValueDic[cellValue] : boolValueConvertAttribute.DefaultBool);
						}
						else
						{
							entityProperty.SetValue(entityData, Convert.ChangeType(cellValue, entityProperty.PropertyType));
						}
					}
					result.Add(entityData);
				}
				return result;
			});
		}
	}
}
