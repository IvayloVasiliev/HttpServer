using System;
using System.Text.RegularExpressions;

namespace SIS.MvcFramework.Attributes.Validation
{
    public class RegexSisAttribute : ValidationSisAttribute
    {
        private readonly string pattern;

        public RegexSisAttribute(string pattern, string errorMessage)
            : base(errorMessage)
        {
            this.pattern = pattern;
        }

        public override bool IsValid(object value)
        {
            string valueAsString = (string)Convert.ChangeType(value, typeof(string));

            return Regex.IsMatch(valueAsString, pattern);
        }
    }
}
