using System;

namespace SIS.MvcFramework.Attributes.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class ValidationSisAttribute : Attribute
    {
        protected ValidationSisAttribute(string errorMessage = "Error Message")
        {
            this.ErrorMessage = errorMessage;
        }

        public string ErrorMessage { get; set; }

        public abstract bool IsValid(object value);
    }
}
