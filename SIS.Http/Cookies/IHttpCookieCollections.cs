namespace SIS.HTTP.Cookies
{
    using System.Collections.Generic;

    public interface IHttpCookieCollections : IEnumerable<HttpCookie>
    {
        void AddCookie(HttpCookie httpCookie);

        bool ContainCookie(string key);

        HttpCookie GetCookie(string key);

        bool HasCookies();
    }
}
