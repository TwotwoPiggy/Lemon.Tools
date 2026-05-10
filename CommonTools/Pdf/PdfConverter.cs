using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CommonTools.Pdf
{
    /// <summary>
    /// PDF转Word转换工具类,提供便捷的静态转换方法。
    /// </summary>
    /// <remarks>
    /// 此类封装了常用的PDF转Word转换操作,内部使用Spire.PDF作为默认转换引擎。
    /// 支持单文件转换和批量转换,支持进度回调和取消操作。
    /// </remarks>
    public static class PdfConverter
    {
        private static readonly Lazy<IPdfToWordConverter> _defaultConverter = 
            new Lazy<IPdfToWordConverter>(() => new SpirePdfConverter());

        /// <summary>
        /// 获取默认的PDF转Word转换器实例。
        /// </summary>
        public static IPdfToWordConverter DefaultConverter => _defaultConverter.Value;

        /// <summary>
        /// 异步转换单个PDF文件为Word文档(使用默认转换器)。
        /// </summary>
        /// <param name="pdfPath">PDF文件的完整路径。</param>
        /// <param name="wordPath">输出的Word文档完整路径。</param>
        /// <param name="options">转换选项(可选)。</param>
        /// <param name="cancellationToken">取消令牌(可选)。</param>
        /// <returns>转换结果,包含成功状态、输出路径和错误信息。</returns>
        public static Task<ConversionResult> ConvertAsync(
            string pdfPath,
            string wordPath,
            ConversionOptions? options = null,
            CancellationToken cancellationToken = default)
        {
            return DefaultConverter.ConvertAsync(pdfPath, wordPath, options, cancellationToken);
        }

        /// <summary>
        /// 异步批量转换多个PDF文件为Word文档(使用默认转换器)。
        /// </summary>
        /// <param name="pdfPaths">PDF文件路径集合。</param>
        /// <param name="outputFolder">输出文件夹路径。</param>
        /// <param name="options">转换选项(可选)。</param>
        /// <param name="progress">进度回调(可选),参数为当前处理进度百分比(0-100)。</param>
        /// <param name="cancellationToken">取消令牌(可选)。</param>
        /// <returns>批量转换结果,包含所有文件的转换结果。</returns>
        public static Task<BatchConversionResult> ConvertBatchAsync(
            IEnumerable<string> pdfPaths,
            string outputFolder,
            ConversionOptions? options = null,
            IProgress<int>? progress = null,
            CancellationToken cancellationToken = default)
        {
            return DefaultConverter.ConvertBatchAsync(pdfPaths, outputFolder, options, progress, cancellationToken);
        }

        /// <summary>
        /// 使用指定转换器异步转换单个PDF文件为Word文档。
        /// </summary>
        /// <param name="converter">PDF转Word转换器实例。</param>
        /// <param name="pdfPath">PDF文件的完整路径。</param>
        /// <param name="wordPath">输出的Word文档完整路径。</param>
        /// <param name="options">转换选项(可选)。</param>
        /// <param name="cancellationToken">取消令牌(可选)。</param>
        /// <returns>转换结果,包含成功状态、输出路径和错误信息。</returns>
        public static Task<ConversionResult> ConvertAsync(
            IPdfToWordConverter converter,
            string pdfPath,
            string wordPath,
            ConversionOptions? options = null,
            CancellationToken cancellationToken = default)
        {
            if (converter == null)
                throw new ArgumentNullException(nameof(converter));

            return converter.ConvertAsync(pdfPath, wordPath, options, cancellationToken);
        }

        /// <summary>
        /// 使用指定转换器异步批量转换多个PDF文件为Word文档。
        /// </summary>
        /// <param name="converter">PDF转Word转换器实例。</param>
        /// <param name="pdfPaths">PDF文件路径集合。</param>
        /// <param name="outputFolder">输出文件夹路径。</param>
        /// <param name="options">转换选项(可选)。</param>
        /// <param name="progress">进度回调(可选),参数为当前处理进度百分比(0-100)。</param>
        /// <param name="cancellationToken">取消令牌(可选)。</param>
        /// <returns>批量转换结果,包含所有文件的转换结果。</returns>
        public static Task<BatchConversionResult> ConvertBatchAsync(
            IPdfToWordConverter converter,
            IEnumerable<string> pdfPaths,
            string outputFolder,
            ConversionOptions? options = null,
            IProgress<int>? progress = null,
            CancellationToken cancellationToken = default)
        {
            if (converter == null)
                throw new ArgumentNullException(nameof(converter));

            return converter.ConvertBatchAsync(pdfPaths, outputFolder, options, progress, cancellationToken);
        }
    }
}
