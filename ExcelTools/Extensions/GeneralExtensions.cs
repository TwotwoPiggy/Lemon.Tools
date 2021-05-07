using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ExcelTools.Extensions
{
    public static class GeneralExtensions
    {
        public static bool ContainString(this object obj,string strSecond)
        {
            if (obj == null || string.IsNullOrWhiteSpace(obj.ToString()))
            {
                throw new ArgumentNullException(nameof(obj));
            }
            return obj.ToString().ToLower().Replace(" ", "").Contains(strSecond);
        }

        public static string ToPosition(this string position)
        {
            var pattern = @"^[A-Za-z]*";
            return Regex.Match(position, pattern, RegexOptions.IgnoreCase).Value;
        }
    }
}
