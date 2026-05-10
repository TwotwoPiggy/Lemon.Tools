using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CommonTools.Pdf
{
    /// <summary>
    /// PDF转Word转换器接口,定义了PDF到Word文档转换的标准操作。
    /// </summary>
    /// <remarks>
    /// 此接口支持多种转换方案的可插拔实现,可以根据项目需求灵活切换转换引擎。
    /// 实现类包括SpirePdfConverter、AsposePdfConverter等。
    /// </remarks>
    public interface IPdfToWordConverter
    {
        /// <summary>
        /// 异步转换单个PDF文件为Word文档。
        /// </summary>
        /// <param name="pdfPath">PDF文件的完整路径。</param>
        /// <param name="wordPath">输出的Word文档完整路径。</param>
        /// <param name="options">转换选项(可选)。</param>
        /// <param name="cancellationToken">取消令牌(可选)。</param>
        /// <returns>转换结果,包含成功状态、输出路径和错误信息。</returns>
        /// <exception cref="ArgumentNullException">路径参数为null或空。</exception>
        /// <exception cref="FileNotFoundException">PDF文件不存在。</exception>
        /// <exception cref="PdfConversionException">转换过程中发生错误。</exception>
        Task<ConversionResult> ConvertAsync(
            string pdfPath, 
            string wordPath, 
            ConversionOptions? options = null, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步批量转换多个PDF文件为Word文档。
        /// </summary>
        /// <param name="pdfPaths">PDF文件路径集合。</param>
        /// <param name="outputFolder">输出文件夹路径。</param>
        /// <param name="options">转换选项(可选)。</param>
        /// <param name="progress">进度回调(可选),参数为当前处理进度百分比(0-100)。</param>
        /// <param name="cancellationToken">取消令牌(可选)。</param>
        /// <returns>批量转换结果,包含所有文件的转换结果。</returns>
        /// <exception cref="ArgumentNullException">路径参数为null或空。</exception>
        /// <exception cref="DirectoryNotFoundException">输出文件夹不存在。</exception>
        Task<BatchConversionResult> ConvertBatchAsync(
            IEnumerable<string> pdfPaths,
            string outputFolder,
            ConversionOptions? options = null,
            IProgress<int>? progress = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取转换器的名称和版本信息。
        /// </summary>
        string ConverterInfo { get; }
    }
}
