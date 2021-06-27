using DocumentFormat.OpenXml.Packaging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ExcelTools.Interfaces
{
	/// <summary>
	/// excel文件读取接口
	/// </summary>
	public interface IExcelReader
	{
		/// <summary>
		/// 打开excel并获取指定sheet
		/// </summary>
		/// <param name="excel"></param>
		/// <param name="sheetName"></param>
		/// <returns></returns>
		WorksheetPart OpenExcelFiles(SpreadsheetDocument excel, string sheetName);

		/// <summary>
		/// 获取excel表头
		/// </summary>
		/// <returns></returns>
		Task<Dictionary<string, string>> GetExcelHeaderAsync(WorksheetPart worksheetPart, SharedStringTablePart stringTable);

		/// <summary>
		/// 将sheet数据转换为指定的T数据集合
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="worksheetPart"></param>
		/// <returns></returns>
		Task<List<T>> ConvertExcelToEntityAsync<T>(WorksheetPart worksheetPart, SharedStringTablePart stringTable, Dictionary<string, string> excelHeaders) where T : new();
	}
}
