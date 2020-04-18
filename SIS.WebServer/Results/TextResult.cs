namespace SIS.MvcFramework.Results
{
    using HTTP.Enums;
    using HTTP.Headers;
    using System.Text;

    public class TextResult : ActionResult
    {
        public TextResult(string content, HttpResponseStatusCode responceStatusCode, string contentType 
            = "text/plain; charset=utf-8")
            : base(responceStatusCode)
        {
            this.Headers.AddHeader(new HttpHeader("Content-Type", contentType));
            this.Content = Encoding.UTF8.GetBytes(content);
        }

        public TextResult(byte[] content, HttpResponseStatusCode responceStatusCode, string contentType
            = "text/plain; charset=utf-8")
            : base(responceStatusCode)
        {
            this.Headers.AddHeader(new HttpHeader("Content-Type", contentType));
            this.Content = content;
        }
    }
}
