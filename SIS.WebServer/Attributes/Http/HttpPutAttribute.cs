namespace SIS.MvcFramework.Attributes
{
    using SIS.HTTP.Enums;
    using System;

    class HttpPutAttribute : BaseHttpAttribute
    {
        public override HttpRequestMethod Method => HttpRequestMethod.Put;
    }
}
