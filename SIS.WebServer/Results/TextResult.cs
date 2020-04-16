namespace SIS.MvcFramework.Results
{
    using System.Text;

    using HTTP.Enums;
    using HTTP.Headers;
    using HTTP.Responses;

    public class TextResult : HttpResponse
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
