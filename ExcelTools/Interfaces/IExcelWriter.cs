using DocumentFormat.OpenXml.Packaging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ExcelTools.Interfaces
{
	/// <summary>
	/// excel文件导出接口
	/// </summary>
	interface IExcelWriter
	{
		/// <summary>
		/// 创建excel文件, 并添加一个sheetName
		/// </summary>
		/// <param name="filePath"></param>
		/// <param name="sheetName"></param>
		/// <returns></returns>
		Task<bool> CreateExcel(string filePath, string sheetName, bool needCloseExcel);

		/// <summary>
		/// 创建excel文件, 并添加多个sheetName
		/// </summary>
		/// <param name="filePaht"></param>
		/// <param name="sheetNames"></param>
		/// <returns></returns>
		Task<bool> CreateExcel(string filePath, IEnumerable<string> sheetNames, bool needCloseExcel);

		Task<bool> InsertSheet(SpreadsheetDocument excel, string sheetName, bool needCloseExcel);

		Task<bool> InsertSheets(SpreadsheetDocument excel, IEnumerable<string> sheetNames, bool needCloseExcel);

		Task<bool> SetExcelHeader<T>(WorkbookPart workbookPart, string sheetName) where T : new();

		
	}
}
