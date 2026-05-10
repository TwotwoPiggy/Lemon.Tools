using System;

namespace CommonTools.Pdf
{
    /// <summary>
    /// 表示单个PDF文件转Word的转换结果。
    /// </summary>
    public class ConversionResult
    {
        /// <summary>
        /// 获取或设置转换是否成功。
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 获取或设置源PDF文件路径。
        /// </summary>
        public string SourcePath { get; set; } = string.Empty;

        /// <summary>
        /// 获取或设置输出的Word文档路径。
        /// </summary>
        public string OutputPath { get; set; } = string.Empty;

        /// <summary>
        /// 获取或设置错误信息(如果转换失败)。
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// 获取或设置异常对象(如果转换失败)。
        /// </summary>
        public Exception? Exception { get; set; }

        /// <summary>
        /// 获取或设置转换耗时(毫秒)。
        /// </summary>
        public long DurationMilliseconds { get; set; }

        /// <summary>
        /// 获取或设置PDF文件的页数。
        /// </summary>
        public int PageCount { get; set; }

        /// <summary>
        /// 创建成功的转换结果。
        /// </summary>
        /// <param name="sourcePath">源PDF路径。</param>
        /// <param name="outputPath">输出Word路径。</param>
        /// <param name="durationMilliseconds">转换耗时(毫秒)。</param>
        /// <param name="pageCount">PDF页数。</param>
        /// <returns>成功的转换结果实例。</returns>
        public static ConversionResult SuccessResult(
            string sourcePath, 
            string outputPath, 
            long durationMilliseconds,
            int pageCount)
        {
            return new ConversionResult
            {
                Success = true,
                SourcePath = sourcePath,
                OutputPath = outputPath,
                DurationMilliseconds = durationMilliseconds,
                PageCount = pageCount
            };
        }

        /// <summary>
        /// 创建失败的转换结果。
        /// </summary>
        /// <param name="sourcePath">源PDF路径。</param>
        /// <param name="errorMessage">错误信息。</param>
        /// <param name="exception">异常对象。</param>
        /// <returns>失败的转换结果实例。</returns>
        public static ConversionResult FailureResult(
            string sourcePath, 
            string errorMessage, 
            Exception? exception = null)
        {
            return new ConversionResult
            {
                Success = false,
                SourcePath = sourcePath,
                ErrorMessage = errorMessage,
                Exception = exception
            };
        }
    }
}
