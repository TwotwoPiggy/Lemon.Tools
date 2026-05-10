using System;

namespace CommonTools.Pdf
{
    /// <summary>
    /// PDF转Word的转换选项配置。
    /// </summary>
    public class ConversionOptions
    {
        /// <summary>
        /// 获取或设置是否使用固定布局模式。
        /// </summary>
        /// <remarks>
        /// true: 固定布局模式,保持PDF的原始布局,适合复杂格式的文档。
        /// false: 流式布局模式,内容可重新流动,适合需要编辑的文档。
        /// </remarks>
        public bool UseFixedLayout { get; set; } = true;

        /// <summary>
        /// 获取或设置输出Word文档的格式。
        /// </summary>
        public WordFormat OutputFormat { get; set; } = WordFormat.Docx;

        /// <summary>
        /// 获取或设置是否提取PDF中的图片。
        /// </summary>
        public bool ExtractImages { get; set; } = true;

        /// <summary>
        /// 获取或设置是否保留PDF的原始字体。
        /// </summary>
        public bool KeepOriginalFonts { get; set; } = true;

        /// <summary>
        /// 获取或设置是否识别表格。
        /// </summary>
        public bool RecognizeTables { get; set; } = true;

        /// <summary>
        /// 获取或设置是否删除空白行。
        /// </summary>
        public bool RemoveEmptyLines { get; set; } = false;

        /// <summary>
        /// 获取或设置转换超时时间(秒)。
        /// </summary>
        public int TimeoutSeconds { get; set; } = 300;

        /// <summary>
        /// 获取或设置是否在转换失败时重试。
        /// </summary>
        public bool RetryOnFailure { get; set; } = false;

        /// <summary>
        /// 获取或设置重试次数。
        /// </summary>
        public int RetryCount { get; set; } = 3;

        /// <summary>
        /// 创建默认转换选项。
        /// </summary>
        /// <returns>默认转换选项实例。</returns>
        public static ConversionOptions Default()
        {
            return new ConversionOptions();
        }

        /// <summary>
        /// 创建高质量转换选项(固定布局,保留所有格式)。
        /// </summary>
        /// <returns>高质量转换选项实例。</returns>
        public static ConversionOptions HighQuality()
        {
            return new ConversionOptions
            {
                UseFixedLayout = true,
                ExtractImages = true,
                KeepOriginalFonts = true,
                RecognizeTables = true,
                RemoveEmptyLines = false
            };
        }

        /// <summary>
        /// 创建可编辑转换选项(流式布局,便于编辑)。
        /// </summary>
        /// <returns>可编辑转换选项实例。</returns>
        public static ConversionOptions Editable()
        {
            return new ConversionOptions
            {
                UseFixedLayout = false,
                ExtractImages = true,
                KeepOriginalFonts = true,
                RecognizeTables = true,
                RemoveEmptyLines = true
            };
        }
    }

    /// <summary>
    /// Word文档格式枚举。
    /// </summary>
    public enum WordFormat
    {
        /// <summary>
        /// DOC格式(Word 97-2003)。
        /// </summary>
        Doc,

        /// <summary>
        /// DOCX格式(Word 2007+)。
        /// </summary>
        Docx
    }
}
