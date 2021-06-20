using DocumentFormat.OpenXml;
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
	/// excel读取抽象类模块
	/// </summary>
	public abstract class ExcelReader : IExcelReader
	{
		public WorksheetPart OpenExcelFiles(SpreadsheetDocument excel, string sheetName)
		{
			var workbookPart = excel.WorkbookPart;
			var sheet = workbookPart.Workbook.Descendants<Sheet>().FirstOrDefault(sheet => sheet.Name == sheetName);
			if (sheet == null)
			{
				throw new ArgumentException($"param {nameof(sheetName)} contains no specific sheet");
			}
			return workbookPart.GetPartById(sheet.Id) as WorksheetPart;
		}


		public Task<Dictionary<string, string>> GetExcelHeaderAsync(WorksheetPart worksheetPart, SpreadsheetDocument document)
		{
			if (worksheetPart == null)
			{
				throw new ArgumentNullException(nameof(worksheetPart));
			}
			var excelHeaders = new Dictionary<string, string>();
			string position;
			string headerName;
			Row row;
			var excelReader = OpenXmlReader.Create(worksheetPart);
			return Task.Run(() =>
			{
				while (excelReader.Read())
				{
					if (excelReader.ElementType != typeof(Row))
					{
						continue;
					}
					row = excelReader.LoadCurrentElement() as Row;
					if (row.RowIndex == 1)
					{
						var stringTable = document.WorkbookPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();
						foreach (var cell in row.Elements<Cell>())
						{
							position = cell.CellReference.Value.ToCellPosition();
							headerName = cell.InnerText;
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
											headerName = stringTable.SharedStringTable.ElementAt(int.Parse(headerName)).InnerText.Trim();
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
							if (string.IsNullOrWhiteSpace(headerName))
							{
								continue;
							}
							excelHeaders.Add(position, headerName);
						}
						break;
					}
				}
				return excelHeaders;
			});
		}

		/// <summary>
		/// 将excel的单行数据填充至指定的数据对象中
		/// </summary>
		/// <typeparam name="T">指定的自定义数据对象类型</typeparam>
		/// <param name="row"></param>
		/// <param name="excelHeaders"></param>
		/// <returns></returns>
		public T FillEntityData<T>(Row row, Dictionary<string, string> excelHeaders) where T : new()
		{
			var entityData = new T();
			PropertyInfo entityProperty;
			string cellValue;
			string cellPosition;
			Dictionary<string, bool> boolValueDic;
			BoolValueConvertAttribute boolValueConvertAttribute;
			foreach (var cell in row.Elements<Cell>())
			{
				cellPosition = cell.CellReference.Value.ToCellPosition();
				cellValue = cell.CellValue.Text;
				entityProperty = entityData.GetCustomProperty<ExcelHeaderAttribute>(excelHeaders[cellPosition]);
				if (entityProperty == null)
				{
					continue;
				}
				if (entityProperty.PropertyType == typeof(bool))
				{
					boolValueConvertAttribute = entityProperty
										.GetCustomAttributes(true)
										.OfType<BoolValueConvertAttribute>()
										.FirstOrDefault();
					boolValueDic = boolValueConvertAttribute?.BoolValues;
					entityProperty.SetValue(
								entityData,
								boolValueDic.TryGetValue(cellValue, out var result)
								? result
								: boolValueConvertAttribute.DefaultBool
							);
				}
				else
				{
					entityProperty.SetValue(entityData, Convert.ChangeType(cellValue, entityProperty.PropertyType));
				}
			}
			return entityData;
		}

		public abstract Task<List<T>> ConvertExcelToEntityAsync<T>(WorksheetPart worksheetPart, Dictionary<string, string> excelHeaders) where T : new();
	}
}
