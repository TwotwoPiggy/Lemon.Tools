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
		Task<bool> CreateExcelAsync(string filePath, string sheetName);

		/// <summary>
		/// 创建excel文件, 并添加多个sheetName
		/// </summary>
		/// <param name="filePath"></param>
		/// <param name="sheetNames"></param>
		/// <returns></returns>
		Task<bool> CreateExcelAsync(string filePath, IEnumerable<string> sheetNames);

		/// <summary>
		/// 添加一个sheet
		/// </summary>
		/// <param name="excel"></param>
		/// <param name="sheetName"></param>
		/// <returns></returns>
		Task<uint> InsertSheetAsync(SpreadsheetDocument excel, string sheetName);

		/// <summary>
		/// 添加多个sheet
		/// </summary>
		/// <param name="excel"></param>
		/// <param name="sheetNames"></param>
		/// <returns></returns>
		Task<Dictionary<string, uint>> InsertSheetsAsync(SpreadsheetDocument excel, IEnumerable<string> sheetNames);

		/// <summary>
		/// 设置表头
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="workbookPart"></param>
		/// <param name="sheetName"></param>
		/// <returns></returns>
		Task<bool> SetExcelHeaderAsync<T>(WorkbookPart workbookPart, string sheetName) where T : new();

		/// <summary>
		/// 导出excel数据
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="workbookPart"></param>
		/// <param name="sheetName"></param>
		/// <param name="entityDatas"></param>
		/// <returns></returns>
		Task<bool> ExportExcelAsync<T>(WorkbookPart workbookPart, string sheetName, IEnumerable<T> entityDatas) where T : new();
	}
}
