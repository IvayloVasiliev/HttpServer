using SIS.HTTP.Enums;
using System.Text;

namespace SIS.MvcFramework.Results
{
    public class NotFoundResult : ActionResult
    {
        public NotFoundResult( string message, HttpResponseStatusCode httpResponseStatusCode = HttpResponseStatusCode.NotFound) 
            : base(httpResponseStatusCode)
        {
            this.Content = Encoding.UTF8.GetBytes(message);
        }
    }
}
