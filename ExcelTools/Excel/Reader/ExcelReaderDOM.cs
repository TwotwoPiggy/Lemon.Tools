using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExcelTools.Excel
{
	/// <summary>
	/// 通过DOM方法读取excel文件
	/// </summary>
	class ExcelReaderDOM : ExcelReader
	{
		public override Task<IEnumerable<T>> ConvertExcelToEntityAsync<T>(WorksheetPart worksheetPart, SharedStringTablePart stringTable, Dictionary<string, string> excelHeaders)
		{
			if (worksheetPart == null)
			{
				throw new ArgumentNullException(nameof(worksheetPart));
			}
			var sheetData = worksheetPart.Worksheet.Elements<SheetData>().FirstOrDefault();
			var result = new List<T>();
			if (!sheetData.Any())
			{
				return null;
			}
			return Task.Run(() =>
			{
				foreach (var row in sheetData.Elements<Row>().Where(row => row.RowIndex != 1))
				{
					result.Add(FillEntityData<T>(row, stringTable, excelHeaders));
				}
				return result.AsEnumerable<T>();
			});
		}
	}
}
