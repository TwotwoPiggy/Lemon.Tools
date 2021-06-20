using ExcelTools;
using ExcelTools.ExcelAttributes;
using System;
using TestExcelTool;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var excelHelper = new ExcelHelper() { FilePath = @"D:\CustouchProject\test1.xlsx" };
            var datas = excelHelper.ImportExcelSAXAsync<TestModel>("sheet5");

            var datas2 = excelHelper.ImportExcelDOMAsync<TestModel>("sheet5");
            Console.WriteLine("success");
        }
    }

}
