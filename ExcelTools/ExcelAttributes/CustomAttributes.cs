using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelTools.ExcelAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class ExcelHeaderAttribute : Attribute
    {
        public string HeaderName { get; }

        public string HeaderPosition { get; }



        public ExcelHeaderAttribute(string headerName = "", string headerPosition = "")
        {
            HeaderName = headerName;
            HeaderPosition = headerPosition;
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class BoolValueConvertAttribute : Attribute
    {
        public Dictionary<string,bool> BoolValues { get; }

        public bool DefaultBool { get;}

        public BoolValueConvertAttribute(string[] trueValues, string[] falseValues,bool defaultBool = false)
        {
            BoolValues = new Dictionary<string, bool>();
            if (trueValues.Length + falseValues.Length < 1)
            {
                throw new ArgumentNullException($"both param {nameof(trueValues)} and {nameof(falseValues)} are empty");
            }
            DefaultBool = defaultBool;
            foreach (var item in trueValues)
            {
                BoolValues.Add(item, true);
            }
            foreach (var item in falseValues)
            {
                BoolValues.Add(item, false);
            }
        }
    }


}
