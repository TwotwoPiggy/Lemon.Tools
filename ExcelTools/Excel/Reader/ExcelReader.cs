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
	abstract class ExcelReader : IExcelReader
	{
		public WorksheetPart OpenExcelFiles(SpreadsheetDocument excel, string sheetName)
		{
			var workbookPart = excel.WorkbookPart;
			var sheet = workbookPart.Workbook.Descendants<Sheet>().FirstOrDefault(sheet => sheet.Name.ToString().EqualsString(sheetName));
			if (sheet == null)
			{
				throw new ArgumentException($"param {nameof(sheetName)} contains no specific sheet");
			}
			return workbookPart.GetPartById(sheet.Id) as WorksheetPart;
		}
		public Task<Dictionary<string, string>> GetExcelHeaderAsync(WorksheetPart worksheetPart, SharedStringTablePart stringTable)
		{
			if (worksheetPart == null)
			{
				throw new ArgumentNullException(nameof(worksheetPart));
			}
			var excelReader = OpenXmlReader.Create(worksheetPart);
			try
			{
				var excelHeaders = new Dictionary<string, string>();
				string position;
				string headerName;
				Row row;
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
							foreach (var cell in row.Elements<Cell>())
							{
								headerName = cell.GetCellValue(stringTable);
								if (string.IsNullOrWhiteSpace(headerName))
								{
									continue;
								}
								position = cell.CellReference.Value.GetCellPosition();
								excelHeaders.Add(position, headerName);
							}
							break;
						}
					}
					excelReader.Dispose();
					return excelHeaders;
				});
			}
			catch (Exception)
			{
				throw;
			}
		}

		/// <summary>
		/// 将excel的单行数据填充至指定的数据对象中
		/// </summary>
		/// <typeparam name="T">指定的自定义数据对象类型</typeparam>
		/// <param name="row"></param>
		/// <param name="excelHeaders"></param>
		/// <returns></returns>
		public T FillEntityData<T>(Row row, SharedStringTablePart stringTable, Dictionary<string, string> excelHeaders) where T : new()
		{
			var entityData = new T();
			PropertyInfo entityProperty;
			string cellValue;
			string cellPosition;
			Dictionary<string, bool> boolValueDic;
			BoolValueConvertAttribute boolValueConvertAttribute;
			foreach (var cell in row.Elements<Cell>())
			{
				cellPosition = cell.CellReference.Value.GetCellPosition();
				entityProperty = entityData.GetCustomProperty<ExcelHeaderAttribute>(excelHeaders[cellPosition]);
				if (entityProperty == null)
				{
					continue;
				}
				cellValue = cell.GetCellValue(stringTable);
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

		public abstract Task<IEnumerable<T>> ConvertExcelToEntityAsync<T>(WorksheetPart worksheetPart, SharedStringTablePart stringTable, Dictionary<string, string> excelHeaders) where T : new();
	}
}
