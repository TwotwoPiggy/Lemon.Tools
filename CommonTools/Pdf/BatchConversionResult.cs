using System;
using System.Collections.Generic;

namespace CommonTools.Pdf
{
    /// <summary>
    /// 表示批量PDF转Word的转换结果。
    /// </summary>
    public class BatchConversionResult
    {
        /// <summary>
        /// 获取或设置所有文件的转换结果列表。
        /// </summary>
        public List<ConversionResult> Results { get; set; } = new();

        /// <summary>
        /// 获取成功转换的文件数量。
        /// </summary>
        public int SuccessCount => Results.Count(r => r.Success);

        /// <summary>
        /// 获取失败转换的文件数量。
        /// </summary>
        public int FailureCount => Results.Count(r => !r.Success);

        /// <summary>
        /// 获取总文件数量。
        /// </summary>
        public int TotalCount => Results.Count;

        /// <summary>
        /// 获取总转换耗时(毫秒)。
        /// </summary>
        public long TotalDurationMilliseconds { get; set; }

        /// <summary>
        /// 获取是否所有文件都转换成功。
        /// </summary>
        public bool AllSuccess => FailureCount == 0 && TotalCount > 0;

        /// <summary>
        /// 获取失败的转换结果列表。
        /// </summary>
        public List<ConversionResult> FailedResults => Results.FindAll(r => !r.Success);

        /// <summary>
        /// 添加单个转换结果。
        /// </summary>
        /// <param name="result">转换结果。</param>
        public void AddResult(ConversionResult result)
        {
            Results.Add(result);
        }

        /// <summary>
        /// 获取批量转换的摘要信息。
        /// </summary>
        /// <returns>摘要信息字符串。</returns>
        public string GetSummary()
        {
            return $"批量转换完成: 总计 {TotalCount} 个文件, 成功 {SuccessCount} 个, 失败 {FailureCount} 个, 耗时 {TotalDurationMilliseconds} 毫秒";
        }
    }
}
