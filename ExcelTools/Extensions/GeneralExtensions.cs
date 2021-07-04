using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using ExcelTools.ExcelAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace ExcelTools.Extensions
{
    /// <summary>
    /// 通用扩展方法
    /// </summary>
    public static class GeneralExtensions
    {
        public static bool ContainsString(this object obj, string strSecond)
        {
            if (obj == null || string.IsNullOrWhiteSpace(obj.ToString()))
            {
                throw new ArgumentNullException(nameof(obj));
            }
            return obj.ToString().Replace(" ", "").Contains(strSecond, StringComparison.CurrentCultureIgnoreCase);
        }

        public static bool EqualsString(this string strEqualed, string strToEqual)
        {
            return strEqualed.Trim().Equals(strToEqual, StringComparison.CurrentCultureIgnoreCase);
        }


        /// <summary>
        /// 根据表头名称来获取对象属性
        /// </summary>
        /// <typeparam name="TAttribute">自定义特性类型</typeparam>
        /// <param name="object"></param>
        /// <param name="headerName"></param>
        /// <returns></returns>
        public static PropertyInfo GetCustomProperty<TAttribute>(this object @object, string headerName) where TAttribute : ExcelHeaderAttribute
        {

            return @object
                        .GetType()
                        .GetProperties()
                        .FirstOrDefault(_ => _.GetCustomAttributes<TAttribute>(true)
                                                .FirstOrDefault()
                                                .HeaderName.Trim() == headerName
                                        //_.GetCustomAttributes(true)
                                        //.OfType<TProperty>()
                                        //.FirstOrDefault()
                                        //.HeaderName.Trim() == headerName
                                        );
        }

        /// <summary>
        /// 获取包含TAttribute特性的属性
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="object"></param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetCustomProperties<TAttribute>(this object @object) where TAttribute : ExcelHeaderAttribute
        {
            return @object
                        .GetType()
                        .GetProperties()
                        .Where(_ => _.GetCustomAttributes<TAttribute>(true).FirstOrDefault() != null);
        }

    }
}
