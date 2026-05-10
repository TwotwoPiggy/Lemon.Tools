using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CommonTools.Pdf;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommonTools.Tests
{
    [TestClass]
    public class PdfConverterTests
    {
        private const string TestPdfFileName = "test.pdf";
        private const string TestPdfContent = "%PDF-1.4\n1 0 obj\n<<\n/Type /Catalog\n>>\nendobj\ntrailer\n<<\n/Root 1 0 R\n>>\n%%EOF";
        private string _testFolder = string.Empty;

        [TestInitialize]
        public void TestInitialize()
        {
            _testFolder = Path.Combine(Path.GetTempPath(), "PdfConverterTests", Guid.NewGuid().ToString());
            Directory.CreateDirectory(_testFolder);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (Directory.Exists(_testFolder))
            {
                try
                {
                    Directory.Delete(_testFolder, true);
                }
                catch
                {
                }
            }
        }

        [TestMethod]
        [Ignore("需要真实的PDF文件进行测试")]
        public async Task ConvertAsync_ValidPdf_ReturnsSuccessResult()
        {
            var pdfPath = CreateTestPdf();
            var wordPath = Path.Combine(_testFolder, "output.docx");

            var result = await PdfConverter.ConvertAsync(pdfPath, wordPath);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(pdfPath, result.SourcePath);
            Assert.AreEqual(wordPath, result.OutputPath);
            Assert.IsTrue(result.DurationMilliseconds > 0);
            Assert.IsTrue(File.Exists(wordPath));
        }

        [TestMethod]
        public async Task ConvertAsync_InvalidPdfPath_ReturnsFailureResult()
        {
            var pdfPath = Path.Combine(_testFolder, "nonexistent.pdf");
            var wordPath = Path.Combine(_testFolder, "output.docx");

            var result = await PdfConverter.ConvertAsync(pdfPath, wordPath);

            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.ErrorMessage);
            Assert.IsTrue(result.ErrorMessage.Contains("不存在") || result.ErrorMessage.Contains("not found"));
        }

        [TestMethod]
        public async Task ConvertAsync_NullPdfPath_ThrowsArgumentNullException()
        {
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(
                () => PdfConverter.ConvertAsync(null!, "output.docx"));
        }

        [TestMethod]
        public async Task ConvertAsync_NullWordPath_ThrowsArgumentNullException()
        {
            var pdfPath = CreateTestPdf();

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(
                () => PdfConverter.ConvertAsync(pdfPath, null!));
        }

        [TestMethod]
        [Ignore("需要真实的PDF文件进行测试")]
        public async Task ConvertAsync_WithCustomOptions_ReturnsSuccessResult()
        {
            var pdfPath = CreateTestPdf();
            var wordPath = Path.Combine(_testFolder, "output.docx");
            var options = ConversionOptions.HighQuality();

            var result = await PdfConverter.ConvertAsync(pdfPath, wordPath, options);

            Assert.IsTrue(result.Success);
            Assert.IsTrue(File.Exists(wordPath));
        }

        [TestMethod]
        [Ignore("需要真实的PDF文件进行测试")]
        public async Task ConvertBatchAsync_MultiplePdfs_ReturnsBatchResult()
        {
            var pdfPaths = new[]
            {
                CreateTestPdf("test1.pdf"),
                CreateTestPdf("test2.pdf"),
                CreateTestPdf("test3.pdf")
            };

            var result = await PdfConverter.ConvertBatchAsync(pdfPaths, _testFolder);

            Assert.AreEqual(3, result.TotalCount);
            Assert.AreEqual(3, result.SuccessCount);
            Assert.AreEqual(0, result.FailureCount);
            Assert.IsTrue(result.AllSuccess);
        }

        [TestMethod]
        public async Task ConvertBatchAsync_WithProgressCallback_ReportsProgress()
        {
            var pdfPaths = new[]
            {
                CreateTestPdf("test1.pdf"),
                CreateTestPdf("test2.pdf")
            };

            var progressValues = new System.Collections.Generic.List<int>();
            var progress = new Progress<int>(value => progressValues.Add(value));

            await PdfConverter.ConvertBatchAsync(pdfPaths, _testFolder, progress: progress);

            Assert.AreEqual(2, progressValues.Count);
            Assert.AreEqual(50, progressValues[0]);
            Assert.AreEqual(100, progressValues[1]);
        }

        [TestMethod]
        public async Task ConvertBatchAsync_EmptyList_ReturnsEmptyResult()
        {
            var result = await PdfConverter.ConvertBatchAsync(Array.Empty<string>(), _testFolder);

            Assert.AreEqual(0, result.TotalCount);
            Assert.AreEqual(0, result.SuccessCount);
            Assert.AreEqual(0, result.FailureCount);
        }

        [TestMethod]
        public async Task ConvertBatchAsync_InvalidOutputFolder_ThrowsDirectoryNotFoundException()
        {
            var pdfPaths = new[] { CreateTestPdf() };
            var invalidFolder = Path.Combine(_testFolder, "nonexistent");

            await Assert.ThrowsExceptionAsync<DirectoryNotFoundException>(
                () => PdfConverter.ConvertBatchAsync(pdfPaths, invalidFolder));
        }

        [TestMethod]
        public async Task ConvertAsync_CancellationRequested_ReturnsFailureResult()
        {
            var pdfPath = CreateTestPdf();
            var wordPath = Path.Combine(_testFolder, "output.docx");
            var cts = new CancellationTokenSource();
            cts.Cancel();

            var result = await PdfConverter.ConvertAsync(pdfPath, wordPath, cancellationToken: cts.Token);

            Assert.IsFalse(result.Success);
            Assert.IsTrue(result.ErrorMessage?.Contains("取消") == true);
        }

        [TestMethod]
        public void SpirePdfConverter_ConverterInfo_ReturnsValidInfo()
        {
            var converter = new SpirePdfConverter();

            Assert.IsFalse(string.IsNullOrEmpty(converter.ConverterInfo));
            Assert.IsTrue(converter.ConverterInfo.Contains("Spire.PDF"));
        }

        [TestMethod]
        public void ConversionOptions_Default_ReturnsDefaultOptions()
        {
            var options = ConversionOptions.Default();

            Assert.IsTrue(options.UseFixedLayout);
            Assert.AreEqual(WordFormat.Docx, options.OutputFormat);
            Assert.IsTrue(options.ExtractImages);
            Assert.IsTrue(options.KeepOriginalFonts);
        }

        [TestMethod]
        public void ConversionOptions_HighQuality_ReturnsHighQualityOptions()
        {
            var options = ConversionOptions.HighQuality();

            Assert.IsTrue(options.UseFixedLayout);
            Assert.IsTrue(options.ExtractImages);
            Assert.IsTrue(options.KeepOriginalFonts);
            Assert.IsTrue(options.RecognizeTables);
            Assert.IsFalse(options.RemoveEmptyLines);
        }

        [TestMethod]
        public void ConversionOptions_Editable_ReturnsEditableOptions()
        {
            var options = ConversionOptions.Editable();

            Assert.IsFalse(options.UseFixedLayout);
            Assert.IsTrue(options.ExtractImages);
            Assert.IsTrue(options.KeepOriginalFonts);
            Assert.IsTrue(options.RecognizeTables);
            Assert.IsTrue(options.RemoveEmptyLines);
        }

        [TestMethod]
        public void ConversionResult_SuccessResult_ReturnsCorrectResult()
        {
            var result = ConversionResult.SuccessResult(
                "source.pdf",
                "output.docx",
                1000,
                5);

            Assert.IsTrue(result.Success);
            Assert.AreEqual("source.pdf", result.SourcePath);
            Assert.AreEqual("output.docx", result.OutputPath);
            Assert.AreEqual(1000, result.DurationMilliseconds);
            Assert.AreEqual(5, result.PageCount);
            Assert.IsNull(result.ErrorMessage);
            Assert.IsNull(result.Exception);
        }

        [TestMethod]
        public void ConversionResult_FailureResult_ReturnsCorrectResult()
        {
            var exception = new Exception("Test exception");
            var result = ConversionResult.FailureResult(
                "source.pdf",
                "Error message",
                exception);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("source.pdf", result.SourcePath);
            Assert.AreEqual("Error message", result.ErrorMessage);
            Assert.AreEqual(exception, result.Exception);
        }

        [TestMethod]
        public void BatchConversionResult_GetSummary_ReturnsCorrectSummary()
        {
            var result = new BatchConversionResult();
            result.AddResult(ConversionResult.SuccessResult("test1.pdf", "output1.docx", 100, 1));
            result.AddResult(ConversionResult.SuccessResult("test2.pdf", "output2.docx", 200, 2));
            result.TotalDurationMilliseconds = 300;

            var summary = result.GetSummary();

            Assert.IsTrue(summary.Contains("总计 2 个文件"));
            Assert.IsTrue(summary.Contains("成功 2 个"));
            Assert.IsTrue(summary.Contains("失败 0 个"));
            Assert.IsTrue(summary.Contains("300 毫秒"));
        }

        private string CreateTestPdf(string fileName = TestPdfFileName)
        {
            var pdfPath = Path.Combine(_testFolder, fileName);
            File.WriteAllText(pdfPath, TestPdfContent);
            return pdfPath;
        }
    }
}
