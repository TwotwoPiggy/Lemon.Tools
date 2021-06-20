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
			var result = new List<T>();
			return Task.Run(()=>
			{
				foreach (var row in sheetData.Elements<Row>().Where(row => row.RowIndex != 1))
				{
					result.Add(FillEntityData<T>(row, excelHeaders));
				}
				return result;
			});
		}
	}
}
