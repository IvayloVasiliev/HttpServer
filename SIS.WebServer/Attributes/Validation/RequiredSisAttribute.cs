namespace SIS.MvcFramework.Attributes.Validation
{
    public class RequiredSisAttribute : ValidationSisAttribute
    {
        public RequiredSisAttribute()
        {

        }
        public RequiredSisAttribute(string errorMessage) : base(errorMessage)
        {

        }

        public override bool IsValid(object value)
        {
            return value != null;
        }
    }
}
