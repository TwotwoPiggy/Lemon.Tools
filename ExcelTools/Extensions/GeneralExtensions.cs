using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ExcelTools.Extensions
{
    public static class GeneralExtensions
    {
        public static bool ContainsString(this object obj,string strSecond)
        {
            if (obj == null || string.IsNullOrWhiteSpace(obj.ToString()))
            {
                throw new ArgumentNullException(nameof(obj));
            }
            return obj.ToString().Replace(" ", "").Contains(strSecond,StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// 获取cell的位置
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public static string ToCellPosition(this string position)
        {
            var pattern = @"^[A-Za-z]*";
            return Regex.Match(position, pattern, RegexOptions.IgnoreCase)?.Value;
        }
    }
}
