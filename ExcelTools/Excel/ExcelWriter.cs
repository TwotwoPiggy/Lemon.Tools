using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using ExcelTools.ExcelAttributes;
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
		public Task<bool> CreateExcel(string filePath, string sheetName, bool needCloseExcel = false)
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
						Id = excel.WorkbookPart.GetIdOfPart(bookPart),
						SheetId = 1,
						Name = sheetName
					};
					sheets.Append(sheet);
					bookPart.Workbook.Save();
					if (needCloseExcel)
					{
						excel.Close();
					}
					return true;
				});
			}
			catch (Exception)
			{
				throw;
			}

		}

		public Task<bool> CreateExcel(string filePath, IEnumerable<string> sheetNames, bool needCloseExcel = false)
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
					if (needCloseExcel)
					{
						excel.Close();
					}
					return true;
				});
			}
			catch (Exception)
			{
				throw;
			}
		}

		public Task<bool> InsertSheet(SpreadsheetDocument excel, string sheetName, bool needCloseExcel = false)
		{
			try
			{
				return Task.Run(() =>
				{
					var newSheetPart = excel.WorkbookPart.AddNewPart<WorksheetPart>();
					newSheetPart.Worksheet = new Worksheet(new SheetData());
					var sheets = excel.WorkbookPart.Workbook.GetFirstChild<Sheets>();
					var relationshipId = excel.WorkbookPart.GetIdOfPart(newSheetPart);
					uint sheetId = sheets.Elements<Sheet>().Count() > 0
								? sheets.Elements<Sheet>().Select(_ => _.SheetId.Value).Max()
								: 0;

					var sheet = new Sheet()
					{
						Id = relationshipId,
						SheetId = ++sheetId,
						Name = sheetName
					};
					sheets.Append(sheet);
					excel.WorkbookPart.Workbook.Save();
					if (needCloseExcel)
					{
						excel.Close();
					}
					return true;
				});
			}
			catch (Exception)
			{
				throw;
			}
		}

		public Task<bool> InsertSheets(SpreadsheetDocument excel, IEnumerable<string> sheetNames, bool needCloseExcel = false)
		{
			try
			{
				return Task.Run(() =>
				{
					var newSheetPart = excel.WorkbookPart.AddNewPart<WorksheetPart>();
					newSheetPart.Worksheet = new Worksheet(new SheetData());
					var sheets = excel.WorkbookPart.Workbook.GetFirstChild<Sheets>();
					var relationshipId = excel.WorkbookPart.GetIdOfPart(newSheetPart);

					var newSheets = new List<Sheet>();
					uint sheetId = sheets.Elements<Sheet>().Count() > 0
								? sheets.Elements<Sheet>().Select(_ => _.SheetId.Value).Max() + 1
								: 1;
					foreach (var sheetName in sheetNames)
					{
						newSheets.Add(new Sheet()
						{
							Id = relationshipId,
							SheetId = ++sheetId,
							Name = sheetName
						});
					}
					sheets.Append(newSheets);
					excel.WorkbookPart.Workbook.Save();
					if (needCloseExcel)
					{
						excel.Close();
					}
					return true;
				});
			}
			catch (Exception)
			{
				throw;
			}
		}

		public Task<bool> SetExcelHeader<T>(WorkbookPart workbookPart,string sheetName) where T : new()
		{
			var sheet = workbookPart.Workbook.Descendants<Sheet>().FirstOrDefault(sheet => sheet.Name.ToString().EqualsString(sheetName));
			if (sheet == null)
			{
				throw new ArgumentException($"param {nameof(sheetName)} contains no specific sheet");
			}
			var worksheetPart = workbookPart.GetPartById(sheet.Id) as WorksheetPart;
			var stringTables = workbookPart.GetPartsOfType<SharedStringTablePart>();
			var shareStringPart = stringTables.Count() > 0
								? stringTables.FirstOrDefault()
								: workbookPart.AddNewPart<SharedStringTablePart>();
			if (shareStringPart.SharedStringTable == null)
			{
				shareStringPart.SharedStringTable = new SharedStringTable();
			}
			var entityData = new T();
			var headerInfos = entityData.GetType().GetCustomAttributes(true).OfType<ExcelHeaderAttribute>();
			foreach (var item in headerInfos)
			{
				int i = 0;
				i = InsertSharedStringItem(item.HeaderName, shareStringPart);
				var cell = 
			}
			//foreach (var item in shareStringPart.SharedStringTable.Elements<SharedStringItem>())
			//{
			//	if (item.InnerText == Text)
			//	{

			//	}
			//}
		}
		#endregion

		#region 私有方法
		private int InsertSharedStringItem(string text,SharedStringTablePart sharedStringPart)
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

		private Cell InsertCellWorksheet(string columnName, uint rowIndex, Worksheet sheet)
		{
			var sheetData = sheet.GetFirstChild<SheetData>();
			string cellReference = columnName + rowIndex;
			var rows = sheetData.Elements<Row>().Where(_ => _.RowIndex == rowIndex);
			Row row;
			if (rows.Count() != 0)
			{
				row = rows.FirstOrDefault();
			}
			else
			{
				row = new Row() { RowIndex = rowIndex };
				sheetData.Append(row);
			}
			if (row.Elements<Cell>().Where(_ => _.CellReference.Value == columnName + rowIndex).Count() > 0)
			{
				return row.Elements<Cell>().Where(_ => _.CellReference.Value == cellReference).FirstOrDefault();
			}
			else
			{
				Cell refCell = null;
				foreach (var cell in row.Elements<Cell>())
				{
					if (string.Compare(cell.CellReference.Value,cellReference,true)>0)
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
		#endregion

	}
}
