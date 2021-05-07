using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using ExcelTools.ExcelAttributes;
using ExcelTools.Extensions;
using ExcelTools.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ExcelTools
{

    public class ExcelHelper
    {
        #region 构造函数
        public ExcelHelper()
        {

        }

        public ExcelHelper(bool isEditable)
        {
            _isEditable = isEditable;
        }
        public ExcelHelper(string filePath, bool isEditable = false):this(isEditable)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentNullException(nameof(filePath));
            }
            _filePath = filePath;
        }

        public ExcelHelper(Stream fileStream,bool isEditable = false) : this(isEditable)
        {
            _fileStream = fileStream ?? throw new ArgumentNullException(nameof(fileStream));
        }
        #endregion

        #region 私有变量

        private string _filePath;

        private bool _isEditable;

        private Stream _fileStream;

        private Dictionary<string, string> ExcelHeaders = new Dictionary<string, string>();
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
        public List<T> ImportExcelToListDOM<T>(string sheetName) where T:new()
        {
            using var document = SpreadsheetDocument.Open(_filePath, _isEditable);
            var worksheetPart = Open(document,sheetName);
            GetExcelHeaderDOM(worksheetPart, document);
            return ConvertToEntityDOM<T>(worksheetPart);
        }

        public List<T> ImportExcelToListSAX<T>(string sheetName) where T : new()
        {
            using var document = SpreadsheetDocument.Open(_filePath, _isEditable);
            var worksheetPart = Open(document, sheetName);
            GetExcelHeaderSAX(worksheetPart, document);
            return ConvertToEntitySAX<T>(worksheetPart);
        }

        #endregion

        #region 私有方法
        private WorksheetPart Open(SpreadsheetDocument document, string sheetName)
        {
            var workbookPart = document.WorkbookPart;
            var sheet = workbookPart.Workbook.Descendants<Sheet>().FirstOrDefault(sheet=>sheet.Name == sheetName);
            if (sheet == null)
            {
                throw new ArgumentException($"param {nameof(sheetName)} contains no specific sheet");
            }
            
            return workbookPart.GetPartById(sheet.Id) as WorksheetPart;
        }

        

        private List<T> ConvertToEntityDOM<T>(WorksheetPart worksheetPart) where T: new()
        {
            if (worksheetPart == null)
            {
                throw new ArgumentNullException(nameof(worksheetPart));
            }
            var sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
            T data;
            string value;
            string position;
            PropertyInfo property;
            Dictionary<string, bool> boolValueDic;
            BoolValueConvertAttribute boolValueConvertAttribute;
            var result = new List<T>();

            foreach (var r in sheetData.Elements<Row>().Where(row=>row.RowIndex !=1))
            {
                data = new T();
                foreach (var c in r.Elements<Cell>())
                {
                    position = c.CellReference.Value.ToPosition();
                    value = c.CellValue.Text;
                    property = data.GetType()
                                    .GetProperties()
                                    .FirstOrDefault(property =>
                                        property
                                            .GetCustomAttributes(true)
                                            .OfType<ExcelHeaderAttribute>()
                                            .FirstOrDefault()
                                            .HeaderName.Trim() == ExcelHeaders[position]
                                    );
                    if (property == null)
                    {
                        continue;
                    }
                    if (property.PropertyType != typeof(bool))
                    {
                        property.SetValue(data, Convert.ChangeType(value, property.PropertyType));
                    }
                    else
                    {
                        boolValueConvertAttribute = property.GetCustomAttributes(true).OfType<BoolValueConvertAttribute>().FirstOrDefault();
                        boolValueDic = boolValueConvertAttribute.BoolValues;
                        property.SetValue(data, boolValueDic.ContainsKey(value) ? boolValueDic[value] : boolValueConvertAttribute.DefaultBool);
                    }
                }
                result.Add(data);
            }
            return result;
        }
        
        private void GetExcelHeaderDOM(WorksheetPart worksheetPart, SpreadsheetDocument document)
        {
            if (worksheetPart == null)
            {
                throw new ArgumentNullException(nameof(worksheetPart));
            }
            var sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
            ExcelHeaders.Clear();
            string position;
            string headerName;
            var row = sheetData.Elements<Row>().First(row => row.RowIndex == 1);
            var stringTable = document.WorkbookPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();
            foreach (var c in row.Elements<Cell>())
            {
                position = c.CellReference.Value.ToPosition();
                headerName = c.InnerText;
                if (c.DataType == null)
                {
                    continue;
                }
                switch (c.DataType.Value)
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
                if (string.IsNullOrWhiteSpace(headerName))
                {
                    continue;
                }
                ExcelHeaders.Add(position, headerName);
            }
        }


        private List<T> ConvertToEntitySAX<T>(WorksheetPart worksheetPart) where T : new()
        {
            if (worksheetPart == null)
            {
                throw new ArgumentNullException(nameof(worksheetPart));
            }
            var excelReader = OpenXmlReader.Create(worksheetPart);

            T data;
            Row r;
            string position;
            string value;
            PropertyInfo property;
            Dictionary<string, bool> boolValueDic;
            BoolValueConvertAttribute boolValueConvertAttribute;
            var result = new List<T>();
            var excelParameters = new ExcelParameters<T>();

            while (excelReader.Read())
            {
                data = new T();
                if (excelReader.ElementType != typeof(Row))
                {
                    continue;
                }
                r = excelReader.LoadCurrentElement() as Row;
                if (r.RowIndex == 1)
                {
                    continue;
                }
                foreach (var c in r.Elements<Cell>())
                {
                    position = c.CellReference.Value.ToPosition();
                    value = c.CellValue.Text;
                    property = data.GetType()
                                    .GetProperties()
                                    .FirstOrDefault(property =>
                                        property
                                            .GetCustomAttributes(true)
                                            .OfType<ExcelHeaderAttribute>()
                                            .FirstOrDefault()
                                            .HeaderName.Trim() == ExcelHeaders[position]
                                    );
                    if (property == null)
                    {
                        continue;
                    }
                    if (property.PropertyType != typeof(bool))
                    {
                        property.SetValue(data, Convert.ChangeType(value, property.PropertyType));
                    }
                    else
                    {
                        boolValueConvertAttribute = property.GetCustomAttributes(true).OfType<BoolValueConvertAttribute>().FirstOrDefault();
                        boolValueDic = boolValueConvertAttribute.BoolValues;
                        property.SetValue(data, boolValueDic.ContainsKey(value) ? boolValueDic[value] : boolValueConvertAttribute.DefaultBool);
                    }
                }
                result.Add(data);
            }
            return result;
        }

        private void GetExcelHeaderSAX(WorksheetPart worksheetPart, SpreadsheetDocument document)
        {
            if (worksheetPart == null)
            {
                throw new ArgumentNullException(nameof(worksheetPart));
            }
            var excelReader = OpenXmlReader.Create(worksheetPart);
            ExcelHeaders.Clear();
            string position;
            string headerName;
            Row row;
            while (excelReader.Read())
            {
                if (excelReader.ElementType != typeof(Row))
                {
                    continue;
                }
                row = excelReader.LoadCurrentElement() as Row;
                if (row.RowIndex != 1)
                {
                    continue;
                }
                var stringTable = document.WorkbookPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();
                foreach (var c in row.Elements<Cell>())
                {
                    position = c.CellReference.Value.ToPosition();
                    headerName = c.InnerText;
                    if (c.DataType != null)
                    {
                        switch (c.DataType.Value)
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
                    ExcelHeaders.Add(position, headerName);
                }
                break;
            }
        }

        private T GetRowData<T>(Row r) where T : new()
        {
            T data;
            string value;
            string position;
            PropertyInfo property;
            Dictionary<string, bool> boolValueDic;
            BoolValueConvertAttribute boolValueConvertAttribute;
            var result = new List<T>();
            data = new T();
            foreach (var c in r.Elements<Cell>())
            {
                position = c.CellReference.Value.ToPosition();
                value = c.CellValue.Text;
                property = data.GetType()
                                .GetProperties()
                                .FirstOrDefault(property =>
                                    property
                                        .GetCustomAttributes(true)
                                        .OfType<ExcelHeaderAttribute>()
                                        .FirstOrDefault()
                                        .HeaderName.Trim() == ExcelHeaders[position]
                                );
                if (property == null)
                {
                    continue;
                }
                if (property.PropertyType != typeof(bool))
                {
                    property.SetValue(data, Convert.ChangeType(value, property.PropertyType));
                }
                else
                {
                    boolValueConvertAttribute = property.GetCustomAttributes(true).OfType<BoolValueConvertAttribute>().FirstOrDefault();
                    boolValueDic = boolValueConvertAttribute.BoolValues;
                    property.SetValue(data, boolValueDic.ContainsKey(value) ? boolValueDic[value] : boolValueConvertAttribute.DefaultBool);
                }
            }
            return data;
            #endregion
        }
    }
}
