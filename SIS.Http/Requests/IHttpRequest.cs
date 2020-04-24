namespace SIS.HTTP.Requests
{
    using System.Collections.Generic;

    using Enums;
    using Headers;
    using Cookies;
    using Sessions;

    public interface IHttpRequest
    {
        string Path { get; }

        string Url { get; }

        Dictionary<string, ISet<string>> FormData { get; }

        Dictionary<string, ISet<string>> QueryData { get; }

        IHttpCookieCollections Cookies { get; }

        IHttpHeaderCollection Headers { get; }

        HttpRequestMethod RequestMethod { get; }

        IHttpSession Session { get; set; }
    }
}
