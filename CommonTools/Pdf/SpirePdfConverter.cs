using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Spire.Pdf;

namespace CommonTools.Pdf
{
    /// <summary>
    /// 基于Spire.PDF的PDF转Word转换器实现。
    /// </summary>
    /// <remarks>
    /// 使用Spire.PDF库实现PDF到Word文档的转换。
    /// 免费版限制:每文档最多10页。
    /// 商业版:无页数限制,需要购买许可证。
    /// </remarks>
    public class SpirePdfConverter : IPdfToWordConverter
    {
        /// <summary>
        /// 获取转换器的名称和版本信息。
        /// </summary>
        public string ConverterInfo => "Spire.PDF Converter v12.2.14";

        /// <summary>
        /// 异步转换单个PDF文件为Word文档。
        /// </summary>
        /// <param name="pdfPath">PDF文件的完整路径。</param>
        /// <param name="wordPath">输出的Word文档完整路径。</param>
        /// <param name="options">转换选项(可选)。</param>
        /// <param name="cancellationToken">取消令牌(可选)。</param>
        /// <returns>转换结果,包含成功状态、输出路径和错误信息。</returns>
        public async Task<ConversionResult> ConvertAsync(
            string pdfPath, 
            string wordPath, 
            ConversionOptions? options = null, 
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(pdfPath))
                throw new ArgumentNullException(nameof(pdfPath));

            if (string.IsNullOrWhiteSpace(wordPath))
                throw new ArgumentNullException(nameof(wordPath));

            options ??= ConversionOptions.Default();
            var stopwatch = Stopwatch.StartNew();

            try
            {
                ValidateFileExists(pdfPath);
                ValidateOutputDirectory(wordPath);
                cancellationToken.ThrowIfCancellationRequested();

                return await Task.Run(() =>
                {
                    try
                    {
                        using var pdfDoc = new PdfDocument();
                        pdfDoc.LoadFromFile(pdfPath);

                        var pageCount = pdfDoc.Pages.Count;

                        var fileFormat = options.OutputFormat == WordFormat.Doc 
                            ? FileFormat.DOC 
                            : FileFormat.DOCX;

                        pdfDoc.SaveToFile(wordPath, fileFormat);
                        pdfDoc.Close();

                        stopwatch.Stop();

                        return ConversionResult.SuccessResult(
                            pdfPath,
                            wordPath,
                            stopwatch.ElapsedMilliseconds,
                            pageCount);
                    }
                    catch (Exception ex)
                    {
                        stopwatch.Stop();
                        return ConversionResult.FailureResult(
                            pdfPath,
                            $"转换失败: {ex.Message}",
                            ex);
                    }
                }, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                stopwatch.Stop();
                return ConversionResult.FailureResult(
                    pdfPath,
                    "转换操作已取消");
            }
            catch (FileNotFoundException ex)
            {
                stopwatch.Stop();
                return ConversionResult.FailureResult(
                    pdfPath,
                    ex.Message,
                    ex);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                return ConversionResult.FailureResult(
                    pdfPath,
                    $"转换失败: {ex.Message}",
                    ex);
            }
        }

        /// <summary>
        /// 异步批量转换多个PDF文件为Word文档。
        /// </summary>
        /// <param name="pdfPaths">PDF文件路径集合。</param>
        /// <param name="outputFolder">输出文件夹路径。</param>
        /// <param name="options">转换选项(可选)。</param>
        /// <param name="progress">进度回调(可选),参数为当前处理进度百分比(0-100)。</param>
        /// <param name="cancellationToken">取消令牌(可选)。</param>
        /// <returns>批量转换结果,包含所有文件的转换结果。</returns>
        public async Task<BatchConversionResult> ConvertBatchAsync(
            IEnumerable<string> pdfPaths,
            string outputFolder,
            ConversionOptions? options = null,
            IProgress<int>? progress = null,
            CancellationToken cancellationToken = default)
        {
            if (pdfPaths == null)
                throw new ArgumentNullException(nameof(pdfPaths));

            if (string.IsNullOrWhiteSpace(outputFolder))
                throw new ArgumentNullException(nameof(outputFolder));

            if (!Directory.Exists(outputFolder))
                throw new DirectoryNotFoundException($"输出文件夹不存在: {outputFolder}");

            options ??= ConversionOptions.Default();
            var pdfPathList = pdfPaths.ToList();
            var result = new BatchConversionResult();
            var stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < pdfPathList.Count; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var pdfPath = pdfPathList[i];
                var fileName = Path.GetFileNameWithoutExtension(pdfPath);
                var extension = options.OutputFormat == WordFormat.Doc ? ".doc" : ".docx";
                var wordPath = Path.Combine(outputFolder, fileName + extension);

                var conversionResult = await ConvertAsync(pdfPath, wordPath, options, cancellationToken);
                result.AddResult(conversionResult);

                progress?.Report((int)((i + 1) * 100.0 / pdfPathList.Count));
            }

            stopwatch.Stop();
            result.TotalDurationMilliseconds = stopwatch.ElapsedMilliseconds;

            return result;
        }

        /// <summary>
        /// 验证PDF文件是否存在。
        /// </summary>
        /// <param name="pdfPath">PDF文件路径。</param>
        /// <exception cref="FileNotFoundException">PDF文件不存在。</exception>
        private static void ValidateFileExists(string pdfPath)
        {
            if (!File.Exists(pdfPath))
                throw new FileNotFoundException($"PDF文件不存在: {pdfPath}", pdfPath);
        }

        /// <summary>
        /// 验证并创建输出目录。
        /// </summary>
        /// <param name="wordPath">Word输出路径。</param>
        private static void ValidateOutputDirectory(string wordPath)
        {
            var directory = Path.GetDirectoryName(wordPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }
    }
}
