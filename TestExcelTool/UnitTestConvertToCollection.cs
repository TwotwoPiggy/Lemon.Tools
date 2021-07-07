using ExcelTools;
using ExcelTools.ExcelAttributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace TestExcelTool
{
    [TestClass]
    public class UnitTestConvertToCollection
    {
        private int data = 10;
        private string filePath = @"D:\Computers\develop\.NetCore\0.Lemon\ToolsTestFiles\alphabet.xlsx";
        private string fileExportPath = @"D:\Computers\develop\.NetCore\0.Lemon\ToolsTestFiles\export.xlsx";
        string sheet = "sheet1";
        [TestMethod]
        public void TestImportPartSAX()
        {
            var excelHelper = new ExcelHelper() { FilePath = filePath };
            var datas2 = excelHelper.ImportExcelSAXAsync<TestModel>(sheet);
            Assert.AreEqual(data, datas2.Result.ToList().Count);
        }

        [TestMethod]
        public void TestImportPartDOM()
        {
            var excelHelper = new ExcelHelper() { FilePath = filePath };
            var datas2 = excelHelper.ImportExcelDOMAsync<TestModel>(sheet);
            Assert.AreEqual(data, datas2.Result.ToList().Count);
        }

        [TestMethod]
        public void TestExport()
        {
            var excelHelper = new ExcelHelper(fileExportPath, true);
            var entities = new List<TestModel>();
            var entity1 = new TestModel();
            entity1.A = 0;
            entity1.B = 1;
            entity1.C = 2;
            entity1.D = 3;
            entity1.E = 4;
            entity1.F = 5;
            entity1.G = 6;
            entity1.H = 7;
            entity1.I = 8;
            entity1.J = 9;
            entity1.K = 10;
            entity1.L = 11;
            entity1.M = 12;
            entity1.N = 13;
            entity1.O = 14;
            entity1.P = 15;
            entity1.Q = 16;
            entity1.R = 17;
            entity1.S = 18;
            entity1.T = 19;
            entity1.U = 20;
            entity1.V = 21;
            entity1.W = 22;
            entity1.X = 23;
            entity1.Y = 24;
            entity1.Z = 25;
            var entity2 = new TestModel();
            entity2.A = 28;
            entity2.B = 29;
            entity2.C = 30;
            entity2.D = 31;
            entity2.E = 32;
            entity2.F = 33;
            entity2.G = 34;
            entity2.H = 35;
            entity2.I = 36;
            entity2.J = 37;
            entity2.K = 38;
            entity2.L = 39;
            entity2.M = 40;
            entity2.N = 41;
            entity2.O = 42;
            entity2.P = 43;
            entity2.Q = 44;
            entity2.R = 45;
            entity2.S = 46;
            entity2.T = 47;
            entity2.U = 48;
            entity2.V = 49;
            entity2.W = 50;
            entity2.X = 51;
            entity2.Y = 52;
            entity2.Z = 53;
            entities.Add(entity1);
            entities.Add(entity2);
            var result = excelHelper.ExportAsync<TestModel>(sheet, entities);
            Assert.AreEqual(true, result.Result);
        }
    }

    public class TestModel
    {
        [ExcelHeader("a", 1)]
        //[BoolValueConvert(new string[] { "1" }, new string[] { "0" })]
        public int A { get; set; }
        [ExcelHeader("b", 2)]
        public int B { get; set; }
        [ExcelHeader("c", 3)]
        public int C { get; set; }
        [ExcelHeader("d", 4)]
        public int D { get; set; }
        [ExcelHeader("e", 5)]
        public int E { get; set; }
        [ExcelHeader("f", 6)]
        public int F { get; set; }
        [ExcelHeader("g", 7)]
        public int G { get; set; }
        [ExcelHeader("h", 8)]
        public int H { get; set; }
        [ExcelHeader("i", 9)]
        public int I { get; set; }
        [ExcelHeader("j", 10)]
        public int J { get; set; }
        [ExcelHeader("k", 11)]
        public int K { get; set; }
        [ExcelHeader("l", 12)]
        public int L { get; set; }
        [ExcelHeader("m", 13)]
        public int M { get; set; }
        [ExcelHeader("n", 14)]
        public int N { get; set; }
        [ExcelHeader("o", 15)]
        public int O { get; set; }
        [ExcelHeader("p", 16)]
        public int P { get; set; }
        [ExcelHeader("q", 17)]
        public int Q { get; set; }
        [ExcelHeader("r", 18)]
        public int R { get; set; }
        [ExcelHeader("s", 19)]
        public int S { get; set; }
        [ExcelHeader("t", 20)]
        public int T { get; set; }
        [ExcelHeader("u", 21)]
        public int U { get; set; }
        [ExcelHeader("v", 22)]
        public int V { get; set; }
        [ExcelHeader("w", 23)]
        public int W { get; set; }
        [ExcelHeader("x", 24)]
        public int X { get; set; }
        [ExcelHeader("y", 25)]
        public int Y { get; set; }
        [ExcelHeader("z", 26)]
        public int Z { get; set; }
    }
}
