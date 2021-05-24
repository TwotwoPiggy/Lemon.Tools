using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using ExcelTools.Excel;
using ExcelTools.ExcelAttributes;
using ExcelTools.Extensions;
using ExcelTools.Interfaces;
using ExcelTools.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ExcelTools
{

	public class ExcelHelper
	{
		#region 构造函数
		public ExcelHelper() { }

		public ExcelHelper(bool isEditable)
		{
			_isEditable = isEditable;
		}
		public ExcelHelper(string filePath, bool isEditable = false) : this(isEditable)
		{
			if (string.IsNullOrWhiteSpace(filePath))
			{
				throw new ArgumentNullException(nameof(filePath));
			}
			_filePath = filePath;
		}

		public ExcelHelper(Stream fileStream, bool isEditable = false) : this(isEditable)
		{
			_fileStream = fileStream ?? throw new ArgumentNullException(nameof(fileStream));
		}
		#endregion

		#region 私有变量

		private string _filePath;

		private bool _isEditable;

		private Stream _fileStream;

		#endregion

		#region 公共属性

		/// <summary>
		/// excel文件完整路径
		/// </summary>
		public string FilePath
		{
			get => _filePath;
			set { _filePath = value; }
		}

		/// <summary>
		/// excel文件是否可修改
		/// </summary>
		public bool IsEditable
		{
			get => _isEditable;
			set { _isEditable = value; }
		}

		public Stream FileStream
		{
			set { _fileStream = value; }
		}
		#endregion

		#region 公共方法
		/// <summary>
		/// 使用DOM方式导入excel文件
		/// 建议：当excel文件较小时使用
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="sheetName"></param>
		/// <returns></returns>
		public async Task<List<T>> ImportExcelToListDOM<T>(string sheetName) where T : new()
		{
			IExcelReader excelBySAX = new ExcelReaderDOM();
			return await ImportExcelToListAsync<T>(excelBySAX, sheetName);
		}

		/// <summary>
		/// 使用SAX方式导入excel文件
		/// 建议：当excel文件较大时使用
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="sheetName"></param>
		/// <returns></returns>
		public async Task<List<T>> ImportExcelToListSAX<T>(string sheetName) where T : new()
		{
			IExcelReader excelBySAX = new ExcelReaderSAX();
			return await ImportExcelToListAsync<T>(excelBySAX, sheetName);
		}

		#endregion

		#region 私有方法
		private async Task<List<T>> ImportExcelToListAsync<T>(IExcelReader excelReader, string sheetName) where T : new()
		{
			using var document = SpreadsheetDocument.Open(_filePath, _isEditable);
			var worksheetPart = excelReader.OpenExcelFiles(document, sheetName);
			if (worksheetPart == null)
			{
				return null;
			}
			var excelHeaderTask = excelReader.GetExcelHeaderAsync(worksheetPart, document);
			if (!excelHeaderTask.Result.Any())
			{
				return null;
			}
			return await excelReader.ConvertExcelToEntityAsync<T>(worksheetPart, excelHeaderTask.Result);
		}


		#endregion
	}
}
