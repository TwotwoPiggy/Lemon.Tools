using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using ExcelTools.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ExcelTools.Extensions
{
    /// <summary>
    /// excel操作扩展方法
    /// </summary>
    public static class ExcelOperationExtensions
    {
        /// <summary>
        /// 获取cell的位置
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public static string GetCellPosition(this string colName)
        {
            var pattern = @"^[A-Za-z]*";
            return Regex.Match(colName, pattern, RegexOptions.IgnoreCase)?.Value;
        }

        /// <summary>
        /// 获取cell值
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="stringTable"></param>
        /// <returns></returns>
        public static string GetCellValue(this Cell cell, SharedStringTablePart stringTable)
        {
            var cellValue = cell.CellValue.InnerText;

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
                            cellValue = stringTable.SharedStringTable.ElementAt(int.Parse(cellValue)).InnerText.Trim();
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
            return cellValue;
        }

        /// <summary>
        /// 向共享字符串表中插入值
        /// </summary>
        /// <param name="text"></param>
        /// <param name="sharedStringPart"></param>
        /// <returns></returns>
        public static int InsertSharedStringItem(this SharedStringTablePart sharedStringPart, string text)
        {
            sharedStringPart.SharedStringTable ??= new SharedStringTable();
            int i = 0;
            foreach (var item in sharedStringPart.SharedStringTable.Elements<SharedStringItem>())
            {
                if (item.InnerText == text)
                {
                    return i;
                }
                i++;
            }
            sharedStringPart.SharedStringTable.AppendChild(new SharedStringItem(new Text(text)));
            sharedStringPart.SharedStringTable.Save();
            return i;
        }

        /// <summary>
        /// 创建一个新的单元格
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="rowIndex"></param>
        /// <param name="sheet"></param>
        /// <returns></returns>
        public static Cell CreateCell(this Worksheet sheet, string columnName, uint rowIndex)
        {
            var sheetData = sheet.GetFirstChild<SheetData>();
            var rows = sheetData.Elements<Row>().Where(_ => _.RowIndex == rowIndex);
            Row row;
            var cellReference = columnName + rowIndex;
            if (rows.Count() != 0)
            {
                row = rows.FirstOrDefault();
            }
            else
            {
                row = new Row() { RowIndex = rowIndex };
                sheetData.Append(row);
            }
            var cells = row.Elements<Cell>().Where(_ => _.CellReference.Value == columnName + rowIndex);
            if (cells.Count() > 0)
            {
                return cells.FirstOrDefault();
            }
            else
            {
                Cell refCell = null;
                foreach (var cell in row.Elements<Cell>())
                {
                    if (cell.CellReference.Value.Length == cellReference.Length
                        && string.Compare(cell.CellReference.Value, cellReference, true) > 0)
                    {
                        refCell = cell;
                        break;
                    }
                }
                var newCell = new Cell() { CellReference = cellReference };
                row.InsertBefore(newCell, refCell);
                sheet.Save();
                return newCell;
            }
        }

        /// <summary>
        /// 设置单元格的值
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="value"></param>
        /// <param name="sharedStringPart"></param>
        /// <param name="cellValues"></param>
        /// <returns></returns>
        public static Cell SetValue(this Cell cell, object value, SharedStringTablePart sharedStringPart, CellValues cellValues = CellValues.SharedString)
        {
            cell.DataType = new EnumValue<CellValues>(cellValues);
            switch (cellValues)
            {
                case CellValues.Boolean:
                    var boolResult = bool.TryParse(value.ToString(), out var boolean) && boolean;
                    cell.CellValue = new CellValue(boolResult);
                    break;
                case CellValues.Number:
                    var numberResult = Double.TryParse(value.ToString(), out var number) ? number : 0D;
                    cell.CellValue = new CellValue(numberResult);
                    break;
                case CellValues.Date:
                    var datetimeResult = DateTime.TryParse(value.ToString(), out var datetime) ? datetime : DateTime.Now;
                    cell.CellValue = new CellValue(datetimeResult);
                    break;
                case CellValues.SharedString:
                    cell.CellValue = new CellValue(sharedStringPart.InsertSharedStringItem(value.ToString()));
                    break;
                default:
                    cell.CellValue = new CellValue(sharedStringPart.InsertSharedStringItem(value.ToString()));
                    break;
            }
            return cell;
        }

        /// <summary>
        /// type转CellValues
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static CellValues TypeToCellType(this Type type)
        {

            return type.Name switch
            {
                "String" => CellValues.SharedString,
                "Boolean" => CellValues.Boolean,
                "DateTime" => CellValues.Date,
                "DateTimeOffset" => CellValues.Date,
                "Int16" => CellValues.Number,
                "Int32" => CellValues.Number,
                "Single" => CellValues.Number,
                "Double" => CellValues.Number,
                _ => CellValues.SharedString
            };
        }

        /// <summary>
        /// 列名转序号
        /// </summary>
        /// <param name="colName"></param>
        /// <returns></returns>
        public static int ColNameToIndex(this string colName)
        {
            // ex:XFD=23*26^2+6*26+4
            colName = GetCellPosition(colName);
            var power = 0;
            var result = 0;
            for (int i = colName.Length; i > 0; i--)
            {
                var col = (int)colName[i - 1] - 64;//获取系数
                var term = col * Math.Pow(26, power);//得到其中一项
                result += int.TryParse(term.ToString(), out var parseResult) ? parseResult : 0;
                power++;//幂+1
            }
            return result;
        }

        /// <summary>
        /// 序号转列名
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string IndexToColName(this int index)
        {
            // ex:XFD=23*26^2+6*26+4
            if (index == 0)
            {
                return "";
            }
            string result;
            var remainder = index % 26;//remainder=4,得到D
            result = remainder != 0 ? ((char)(64 + remainder)).ToString() : "Z";//当余数为零时, 末尾一定是Z
            if (index / 26 > 1D)
            {
                var divisor = (index - remainder) / 26;//divisor=23*26+6, 获取被除数
                remainder = divisor % 26;//remainder=6, 得到F
                result = remainder != 0 ? ((char)(64 + remainder)).ToString() + result : "Z" + result;
                if (divisor / 26 >= 1D)
                {
                    divisor = (divisor - remainder) / 26;//divisor=23,得到X
                    result = ((char)(64 + divisor)).ToString() + result;
                }
            }
            return result;
        }

        /// <summary>
        /// 获取工作表部分(WorksheetPart)
        /// </summary>
        /// <param name="workbookPart"></param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public static WorksheetPart GetWorksheetPart(this WorkbookPart workbookPart, string sheetName)
        {
            var sheet = workbookPart.Workbook.Descendants<Sheet>().FirstOrDefault(sheet => sheet.Name.ToString().EqualsString(sheetName));
            if (sheet == null)
            {
                return null;
            }
            return workbookPart.GetPartById(sheet.Id) as WorksheetPart;
        }

        /// <summary>
        /// 获取共享字符串表部分(SharedStringTablePart)
        /// </summary>
        /// <param name="workbookPart"></param>
        /// <returns></returns>
        public static SharedStringTablePart GetSharedStringTablePart(this WorkbookPart workbookPart)
        {
            var stringTables = workbookPart.GetPartsOfType<SharedStringTablePart>();
            return stringTables.Count() > 0
                ? stringTables.FirstOrDefault()
                : workbookPart.AddNewPart<SharedStringTablePart>();
        }
    }
}
