using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using ExcelTools.ExcelAttributes;
using ExcelTools.Extensions;
using ExcelTools.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ExcelTools.Excel
{
    class ExcelWriter : IExcelWriter
    {
        #region 接口实现
        public Task<bool> CreateExcelAsync(string filePath, string sheetName)
        {
            try
            {
                return Task.Run(() =>
                {
                    var excel = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook);
                    //创建一个WorkbookPart
                    var bookPart = excel.AddWorkbookPart();
                    bookPart.Workbook = new Workbook();

                    //创建一个WorksheetPart
                    var sheetPart = bookPart.AddNewPart<WorksheetPart>();
                    sheetPart.Worksheet = new Worksheet(new SheetData());

                    //创建一个sheets
                    var sheets = excel.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());
                    var sheet = new Sheet()
                    {
                        Id = excel.WorkbookPart.GetIdOfPart(sheetPart),
                        SheetId = 1,
                        Name = sheetName
                    };
                    sheets.Append(sheet);
                    bookPart.Workbook.Save();
                    excel.Close();
                    return true;
                });
            }
            catch (Exception)
            {
                throw;
            }

        }

        public Task<bool> CreateExcelAsync(string filePath, IEnumerable<string> sheetNames)
        {
            try
            {
                return Task.Run(() =>
                {
                    var excel = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook);
                    //创建一个WorkbookPart
                    var bookPart = excel.AddWorkbookPart();
                    bookPart.Workbook = new Workbook();
                    //创建一个WorksheetPart
                    var sheetPart = bookPart.AddNewPart<WorksheetPart>();
                    sheetPart.Worksheet = new Worksheet(new SheetData());
                    var relationshipId = excel.WorkbookPart.GetIdOfPart(bookPart);
                    //创建一个sheets
                    var sheets = excel.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());
                    var newSheets = new List<Sheet>();
                    uint sheetId = 1;
                    foreach (var sheetName in sheetNames)
                    {
                        newSheets.Add(new Sheet()
                        {
                            Id = relationshipId,
                            SheetId = sheetId++,
                            Name = sheetName
                        });
                    }
                    sheets.Append(newSheets);
                    bookPart.Workbook.Save();
                    excel.Close();
                    return true;
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<uint> InsertSheetAsync(SpreadsheetDocument excel, string sheetName)
        {
            try
            {
                return Task.Run(() =>
                {
                    //添加一个新的worksheet和sheetData
                    var newSheetPart = excel.WorkbookPart.AddNewPart<WorksheetPart>();
                    newSheetPart.Worksheet = new Worksheet(new SheetData());
                    newSheetPart.Worksheet.Save();

                    //给新sheet设置一个sheetid
                    var sheets = excel.WorkbookPart.Workbook.GetFirstChild<Sheets>();
                    var relationshipId = excel.WorkbookPart.GetIdOfPart(newSheetPart);
                    uint sheetId = sheets.Elements<Sheet>().Count() > 0
                                ? sheets.Elements<Sheet>().Select(_ => _.SheetId.Value).Max()
                                : 0;

                    var sheet = new Sheet()
                    {
                        Id = relationshipId,//sheetPart的id
                        SheetId = ++sheetId,
                        Name = sheetName ?? $"sheet{sheetId}"
                    };
                    sheets.Append(sheet);
                    excel.WorkbookPart.Workbook.Save();
                    excel.Close();
                    return sheetId;
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<Dictionary<string, uint>> InsertSheetsAsync(SpreadsheetDocument excel, IEnumerable<string> sheetNames)
        {
            try
            {
                return Task.Run(() =>
                {
                    var newSheetPart = excel.WorkbookPart.AddNewPart<WorksheetPart>();
                    newSheetPart.Worksheet = new Worksheet(new SheetData());
                    newSheetPart.Worksheet.Save();

                    var sheets = excel.WorkbookPart.Workbook.GetFirstChild<Sheets>();
                    var relationshipId = excel.WorkbookPart.GetIdOfPart(newSheetPart);
                    uint sheetId = sheets.Elements<Sheet>().Count() > 0
                                ? sheets.Elements<Sheet>().Select(_ => _.SheetId.Value).Max()
                                : 0;

                    var newSheets = new List<Sheet>();
                    var sheetIds = new Dictionary<string, uint>();
                    foreach (var sheetName in sheetNames)
                    {
                        newSheets.Add(new Sheet()
                        {
                            Id = relationshipId,
                            SheetId = ++sheetId,
                            Name = sheetName ?? $"sheet{sheetId}"
                        });
                        sheetIds.Add(sheetName, sheetId);
                    }
                    sheets.Append(newSheets);
                    excel.WorkbookPart.Workbook.Save();
                    excel.Close();
                    return sheetIds;
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<bool> SetExcelHeaderAsync<T>(WorkbookPart workbookPart, string sheetName) where T : new()
        {
            return Task.Run(() =>
            {
                var worksheetPart = workbookPart.GetWorksheetPart(sheetName);
                if (worksheetPart == null)
                {
                    throw new ArgumentException($"param {nameof(sheetName)} contains no specific sheet");
                }
                var entityData = new T();
                var headerInfos = entityData.GetType().GetProperties().Select(_ => _.GetCustomAttributes(true).OfType<ExcelHeaderAttribute>().FirstOrDefault());
                foreach (var item in headerInfos.ToList())
                {
                    if (!item.HeaderIndex.HasValue)
                    {
                        continue;
                    }
                    if (string.IsNullOrWhiteSpace(item.HeaderIndex.Value.IndexToColName()))
                    {
                        continue;
                    }
                    worksheetPart
                            .Worksheet
                            .CreateCell(item.HeaderIndex.Value.IndexToColName(), 1)
                            .SetValue(item.HeaderName, workbookPart.GetSharedStringTablePart());
                }
                worksheetPart.Worksheet.Save();
                return true;
            });
        }

        public Task<bool> ExportExcelAsync<T>(WorkbookPart workbookPart, string sheetName, IEnumerable<T> entityDatas) where T : new()
        {
            return Task.Run(() =>
            {
                var worksheetPart = workbookPart.GetWorksheetPart(sheetName);
                if (worksheetPart == null)
                {
                    throw new ArgumentException($"param {nameof(sheetName)} contains no specific sheet");
                }
                var sheetData = worksheetPart.Worksheet.OfType<SheetData>().FirstOrDefault();
                var rowIndex = sheetData.Elements<Row>().Max(row => row.RowIndex);
                foreach (var entityData in entityDatas)
                {
                    rowIndex++;
                    var entityProperties = entityData.GetCustomProperties<ExcelHeaderAttribute>();
                    //一个实例的所有含ExcelHeaderAttribute的属性
                    if (!entityProperties.Any())
                    {
                        continue;
                    }
                    foreach (var entityProperty in entityProperties)
                    {
                        var excelHeaderAttribute = entityProperty.GetCustomAttributes(true).OfType<ExcelHeaderAttribute>().FirstOrDefault();
                        worksheetPart
                                .Worksheet
                                .CreateCell(excelHeaderAttribute.HeaderIndex.Value.IndexToColName(), rowIndex)
                                .SetValue(entityProperty.GetValue(entityData), workbookPart.GetSharedStringTablePart(), entityProperty.PropertyType.TypeToCellType());
                    }

                }
                worksheetPart.Worksheet.Save();
                return true;
            });
        }
        #endregion
    }
}
