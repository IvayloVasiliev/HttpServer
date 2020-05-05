namespace SIS.MvcFramework.Attributes.Validation
{
    public class RerquiredSisAttribute : ValidationSisAttribute
    {
        public RerquiredSisAttribute()
        {

        }
        public RerquiredSisAttribute(string errorMessage) : base(errorMessage)
        {

        }

        public override bool IsValid(object value)
        {
            return value != null;
        }
    }
}
