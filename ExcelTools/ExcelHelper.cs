using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using ExcelTools.Excel;
using ExcelTools.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ExcelTools
{

    public class ExcelHelper
    {
        #region 构造函数
        /// <summary>
        /// 初始化Excel操作工具
        /// </summary>
        public ExcelHelper() { }

        /// <summary>
        /// 初始化Excel操作工具
        /// </summary>
        /// <param name="isEditable">操作的excel是否可编辑, 当操作类型为修改时, 请传入true值</param>
        public ExcelHelper(bool isEditable)
        {
            IsEditable = isEditable;
        }

        /// <summary>
        /// 初始化Excel操作工具
        /// </summary>
        /// <param name="filePath">操作的excel文件完整路径</param>
        /// <param name="isEditable">操作的excel是否可编辑, 当操作类型为修改时, 请传入true值</param>
        public ExcelHelper(string filePath, bool isEditable = true) : this(isEditable)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentNullException(nameof(filePath));
            }
            FilePath = filePath;
        }

        /// <summary>
        /// 初始化Excel操作工具
        /// </summary>
        /// <param name="filePath">操作的excel文件字节流</param>
        /// <param name="isEditable">操作的excel是否可编辑, 当操作类型为修改时, 请传入true值</param>
        public ExcelHelper(Stream fileStream, bool isEditable = true) : this(isEditable)
        {
            FileStream = fileStream ?? throw new ArgumentNullException(nameof(fileStream));
        }
        #endregion

        #region 公共属性
        /// <summary>
        /// excel文件完整路径
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// excel文件是否可修改
        /// </summary>
        public bool IsEditable { get; set; } = true;
        /// <summary>
        /// excel文件字节流
        /// </summary>
        public Stream FileStream { get; set; }
        #endregion

        #region 公共方法
        /// <summary>
        /// 使用DOM方式导入excel文件
        /// 建议：当excel文件较小时使用
        /// </summary>
        /// <typeparam name="T">指定存放数据的自定义类型</typeparam>
        /// <param name="sheetName">导入excel的sheet名称</param>
        /// <returns></returns>    
        public async Task<IEnumerable<T>> ImportExcelDOMAsync<T>(string sheetName) where T : new()
        {
            IExcelReader excelReader = new ExcelReaderDOM();
            IEnumerable<T> result = null;
            if (!string.IsNullOrWhiteSpace(FilePath))
            {
                result = await ImportExcelByFilePathAsync<T>(excelReader, sheetName);
            }
            if ((result == null || !result.Any()) && FileStream.Length > 0)//上一个操作错误导致result为空, 同时文件字节流不为空时
            {
                result = await ImportExcelByStreamAsync<T>(excelReader, sheetName);
            }
            return result;
        }

        /// <summary>
        /// 使用SAX方式导入excel文件
        /// 建议：当excel文件较大时使用
        /// </summary>
        /// <typeparam name="T">指定存放数据的自定义类型</typeparam>
        /// <param name="sheetName">导入excel的sheet名称</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> ImportExcelSAXAsync<T>(string sheetName) where T : new()
        {
            IExcelReader excelReader = new ExcelReaderSAX();
            IEnumerable<T> result = null;
            if (!string.IsNullOrWhiteSpace(FilePath))
            {
                result = await ImportExcelByFilePathAsync<T>(excelReader, sheetName);
            }
            if ((result == null || !result.Any()) && FileStream.Length > 0)//上一个操作错误导致result为空, 同时文件字节流不为空时
            {
                result = await ImportExcelByStreamAsync<T>(excelReader, sheetName);
            }
            return result;
        }

        /// <summary>
        /// 导出excel数据
        /// </summary>
        /// <typeparam name="T">待导出的自定义数据类型</typeparam>
        /// <param name="sheetName">导入excel的sheet名称</param>
        /// <param name="entities">待导出的数据集合</param>
        /// <returns></returns>
        public async Task<bool> ExportAsync<T>(string sheetName, IEnumerable<T> entities, bool? isEditable = null) where T : new()
        {
            IExcelWriter excelWriter = new ExcelWriter();
            var result = await excelWriter.CreateExcelAsync(FilePath, sheetName);
            if (!result)
            {
                return result;
            }
            if (!string.IsNullOrWhiteSpace(FilePath))
            {
                result = await ExportByFilePathAsync<T>(excelWriter, sheetName, entities, isEditable.HasValue ? isEditable.Value : IsEditable);
            }
            if (!result && FileStream.Length > 0)//上一个操作错误result=false, 同时文件字节流不为空时
            {
                result = await ExportByFileStreamAsync<T>(excelWriter, sheetName, entities, isEditable.HasValue ? isEditable.Value : IsEditable);
            }
            return result;
        }
        #endregion

        #region 私有方法
        private async Task<bool> ExportByFilePathAsync<T>(IExcelWriter excelWriter, string sheetName, IEnumerable<T> entities, bool isEditable) where T : new()
        {
            using var document = SpreadsheetDocument.Open(FilePath, isEditable);
            var setHeaderResult = await excelWriter.SetExcelHeaderAsync<T>(document.WorkbookPart, sheetName);
            if (!setHeaderResult)
            {
                return false;
            }
            return await excelWriter.ExportExcelAsync<T>(document.WorkbookPart, sheetName, entities);
        }

        private async Task<bool> ExportByFileStreamAsync<T>(IExcelWriter excelWriter, string sheetName, IEnumerable<T> entities, bool isEditable) where T : new()
        {
            using var document = SpreadsheetDocument.Open(FileStream, isEditable);
            var setHeaderResult = await excelWriter.SetExcelHeaderAsync<T>(document.WorkbookPart, sheetName);
            if (!setHeaderResult)
            {
                return false;
            }
            return await excelWriter.ExportExcelAsync<T>(document.WorkbookPart, sheetName, entities);
        }

        private async Task<IEnumerable<T>> ImportExcelByFilePathAsync<T>(IExcelReader excelReader, string sheetName) where T : new()
        {
            using var document = SpreadsheetDocument.Open(FilePath, IsEditable);
            var worksheetPart = excelReader.OpenExcelFiles(document, sheetName);
            if (worksheetPart == null)
            {
                return null;
            }
            var stringTable = document.WorkbookPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();
            var excelHeaderTask = await excelReader.GetExcelHeaderAsync(worksheetPart, stringTable);
            if (!excelHeaderTask.Any())
            {
                return null;
            }
            return await excelReader.ConvertExcelToEntityAsync<T>(worksheetPart, stringTable, excelHeaderTask);
        }

        private async Task<IEnumerable<T>> ImportExcelByStreamAsync<T>(IExcelReader excelReader, string sheetName) where T : new()
        {
            using var document = SpreadsheetDocument.Open(FilePath, IsEditable);
            var worksheetPart = excelReader.OpenExcelFiles(document, sheetName);
            if (worksheetPart == null)
            {
                return null;
            }
            var stringTable = document.WorkbookPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();
            var excelHeaderTask = await excelReader.GetExcelHeaderAsync(worksheetPart, stringTable);
            if (!excelHeaderTask.Any())
            {
                return null;
            }
            return await excelReader.ConvertExcelToEntityAsync<T>(worksheetPart, stringTable, excelHeaderTask);
        }
        #endregion
    }
}
