using ExcelTools;
using ExcelTools.ExcelAttributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace TestExcelTool
{
    [TestClass]
    public class UnitTestConvertToCollection
    {
        private int data = 10;
        private string filePath = @"D:\Computers\develop\.NetCore\0.Lemon\ToolsTestFiles\alphabet.xlsx";
        private string fileExportPath = @"D:\develop\export.xlsx";
        string sheet = "sheet1";
        [TestMethod]
        public void TestImportPartSAX()
        {
            var excelHelper = new ExcelHelper() { FilePath = filePath };
            var datas2 = excelHelper.ImportExcelSAXAsync<TestModel>(sheet);
            Assert.AreEqual(data, datas2.Result.Count);
        }

        [TestMethod]
        public void TestImportPartDOM()
        {
            var excelHelper = new ExcelHelper() { FilePath = filePath };
            var datas2 = excelHelper.ImportExcelDOMAsync<TestModel>(sheet);
            Assert.AreEqual(data, datas2.Result.Count);
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
        [ExcelHeader("a")]
        [BoolValueConvert(new string[] { "1" }, new string[] { "0" })]
        public int A { get; set; }
        [ExcelHeader("b")]
        public int B { get; set; }
        [ExcelHeader("c")]
        public int C { get; set; }
        [ExcelHeader("d")]
        public int D { get; set; }
        [ExcelHeader("e")]
        public int E { get; set; }
        [ExcelHeader("f")]
        public int F { get; set; }
        [ExcelHeader("g")]
        public int G { get; set; }
        [ExcelHeader("h")]
        public int H { get; set; }
        [ExcelHeader("i")]
        public int I { get; set; }
        [ExcelHeader("j")]
        public int J { get; set; }
        [ExcelHeader("k")]
        public int K { get; set; }
        [ExcelHeader("l")]
        public int L { get; set; }
        [ExcelHeader("m")]
        public int M { get; set; }
        [ExcelHeader("n")]
        public int N { get; set; }
        [ExcelHeader("o")]
        public int O { get; set; }
        [ExcelHeader("p")]
        public int P { get; set; }
        [ExcelHeader("q")]
        public int Q { get; set; }
        [ExcelHeader("r")]
        public int R { get; set; }
        [ExcelHeader("s")]
        public int S { get; set; }
        [ExcelHeader("t")]
        public int T { get; set; }
        [ExcelHeader("u")]
        public int U { get; set; }
        [ExcelHeader("v")]
        public int V { get; set; }
        [ExcelHeader("w")]
        public int W { get; set; }
        [ExcelHeader("x")]
        public int X { get; set; }
        [ExcelHeader("y")]
        public int Y { get; set; }
        [ExcelHeader("z")]
        public int Z { get; set; }
    }
}
