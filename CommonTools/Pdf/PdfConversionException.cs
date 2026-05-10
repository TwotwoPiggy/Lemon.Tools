using System;

namespace CommonTools.Pdf
{
    /// <summary>
    /// PDF转换过程中发生的异常。
    /// </summary>
    public class PdfConversionException : Exception
    {
        /// <summary>
        /// 获取或设置源PDF文件路径。
        /// </summary>
        public string? SourcePdfPath { get; set; }

        /// <summary>
        /// 获取或设置目标Word文件路径。
        /// </summary>
        public string? TargetWordPath { get; set; }

        /// <summary>
        /// 初始化 <see cref="PdfConversionException"/> 类的新实例。
        /// </summary>
        public PdfConversionException()
            : base()
        {
        }

        /// <summary>
        /// 初始化 <see cref="PdfConversionException"/> 类的新实例。
        /// </summary>
        /// <param name="message">异常消息。</param>
        public PdfConversionException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// 初始化 <see cref="PdfConversionException"/> 类的新实例。
        /// </summary>
        /// <param name="message">异常消息。</param>
        /// <param name="innerException">内部异常。</param>
        public PdfConversionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// 初始化 <see cref="PdfConversionException"/> 类的新实例。
        /// </summary>
        /// <param name="message">异常消息。</param>
        /// <param name="sourcePdfPath">源PDF文件路径。</param>
        /// <param name="targetWordPath">目标Word文件路径。</param>
        /// <param name="innerException">内部异常。</param>
        public PdfConversionException(
            string message, 
            string sourcePdfPath, 
            string targetWordPath, 
            Exception? innerException = null)
            : base(message, innerException)
        {
            SourcePdfPath = sourcePdfPath;
            TargetWordPath = targetWordPath;
        }

        /// <summary>
        /// 创建文件不存在异常。
        /// </summary>
        /// <param name="filePath">文件路径。</param>
        /// <returns>文件不存在异常实例。</returns>
        public static PdfConversionException FileNotFound(string filePath)
        {
            return new PdfConversionException(
                $"PDF文件不存在: {filePath}",
                filePath,
                null);
        }

        /// <summary>
        /// 创建转换失败异常。
        /// </summary>
        /// <param name="sourcePath">源PDF路径。</param>
        /// <param name="targetPath">目标Word路径。</param>
        /// <param name="innerException">内部异常。</param>
        /// <returns>转换失败异常实例。</returns>
        public static PdfConversionException ConversionFailed(
            string sourcePath, 
            string targetPath, 
            Exception? innerException = null)
        {
            return new PdfConversionException(
                $"PDF转换失败: {sourcePath}",
                sourcePath,
                targetPath,
                innerException);
        }

        /// <summary>
        /// 创建输出路径无效异常。
        /// </summary>
        /// <param name="outputPath">输出路径。</param>
        /// <returns>输出路径无效异常实例。</returns>
        public static PdfConversionException InvalidOutputPath(string outputPath)
        {
            return new PdfConversionException(
                $"输出路径无效: {outputPath}",
                null,
                outputPath);
        }
    }
}
