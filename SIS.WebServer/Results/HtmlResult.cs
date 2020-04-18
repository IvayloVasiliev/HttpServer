namespace SIS.MvcFramework.Results
{
    using HTTP.Enums;
    using HTTP.Headers;
    using System.Text;

    public class HtmlResult : ActionResult
    {
        public HtmlResult(string content, HttpResponseStatusCode responseStatusCode = HttpResponseStatusCode.Ok)
            : base(responseStatusCode)
        {
            this.Headers.AddHeader(new HttpHeader("Content-Type", "text/html; charset=utf-8"));
            this.Content = Encoding.UTF8.GetBytes(content);
        }

    }
}
