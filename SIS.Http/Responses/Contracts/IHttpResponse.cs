 namespace SIS.HTTP.Responses.Contracts
{
    using Enums;
    using Headers;
    using Headers.Contracts;
    using Cookies;
    using Cookies.Contracts;

    public interface IHttpResponse
    {
        HttpResponseStatusCode StatusCode { get; set; }

        IHttpHeaderCollection Headers { get; }

        IHttpCookieCollections Cookies { get; }

        byte[] Content { get; set; }

        void AddHeader(HttpHeader header);

        void AddCookie(HttpCookie cookie);

        byte[] GetBytes();

    }
}
