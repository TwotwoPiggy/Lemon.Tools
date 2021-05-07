using DocumentFormat.OpenXml.Spreadsheet;
using ExcelTools.ExcelAttributes;
using ExcelTools.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ExcelTools.Utils
{
    internal class ExcelParameters<T> : IParameter where T:new()
    {
        public T Data { get; set; }
        public Row Row { get; set; }

        public string Position { get; set; }

        public string Value { get; set; }

        public PropertyInfo Property { get; set; }

        public Dictionary<string, bool> BoolValueDic { get; set; }

        public BoolValueConvertAttribute boolValueConvertAttribute { get; set; }

        public List<T> result { get; set; }


    }
}
