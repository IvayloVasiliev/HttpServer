namespace SIS.HTTP.Responses
{
    using System.Text;

    using Common;
    using Enums;
    using Extensions;
    using Headers;
    using Headers.Contracts;
    using Responses.Contracts;
   

    public class HttpResponse : IHttpResponse
    {
        public HttpResponse()
        {
            this.Headers = new HttpHeaderCollection();
            this.Content = new byte[0];
        }

        public HttpResponse(HttpResponseStatusCode statusCode)
            : this()
        {
            CoreValidator.ThrowIfNull(statusCode, nameof(statusCode));
            this.StatusCode = statusCode;
        }

        public HttpResponseStatusCode StatusCode { get; set; }

        public IHttpHeaderCollection Headers { get; }

        public byte[] Content { get; set; }

        public void AddHeader(HttpHeader header)
        {
            this.Headers.AddHeader(header);
        }

        public byte[] GetBytes()
        {
            byte[] httpResponceBytesWithoutBody = Encoding.UTF8.GetBytes(this.ToString());

            byte[] httpResponceBytesWithBody = new byte[httpResponceBytesWithoutBody.Length + this.Content.Length];

            for (int i = 0; i < httpResponceBytesWithoutBody.Length; i++)
            {
                httpResponceBytesWithBody[i] = httpResponceBytesWithoutBody[i];
            }

            for (int i = 0; i <httpResponceBytesWithBody.Length - httpResponceBytesWithoutBody.Length; i++)
            {
                httpResponceBytesWithBody[i + httpResponceBytesWithoutBody.Length] = Content[i];
            }

            return httpResponceBytesWithBody;
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result
                .Append($"{GlobalConstants.HttpOneProtocolFragment} {this.StatusCode.GetStatusLine()}")
                .Append(GlobalConstants.HttpNewLine)
                .Append($"{this.Headers}")
                .Append(GlobalConstants.HttpNewLine);

            result.Append(GlobalConstants.HttpNewLine);

            return result.ToString();
        }

    }
}
