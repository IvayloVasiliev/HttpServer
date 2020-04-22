﻿namespace SIS.HTTP.Cookies
{
    using Common;
    using SIS.Common;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;

    public class HttpCookieCollection : IHttpCookieCollections
    {
        private Dictionary<string, HttpCookie> httpCookies;

        public HttpCookieCollection()
        {
            this.httpCookies = new Dictionary<string, HttpCookie>();
        }

        public void AddCookie(HttpCookie httpCookie)
        {
            httpCookie.ThrowIfNull(nameof(httpCookie));

            if (!this.ContainCookie(httpCookie.Key))
            {
                this.httpCookies.Add(httpCookie.Key, httpCookie);
            } 
        }

        public bool ContainCookie(string key)
        {
            key.ThrowIfNullOrEmpty(nameof(key));

            return  this.httpCookies.ContainsKey(key);
        }

        public HttpCookie GetCookie(string key)
        {
            key.ThrowIfNullOrEmpty(nameof(key));

            return this.httpCookies[key];
        }  

        public bool HasCookies()
        {
            return this.httpCookies.Count != 0;
        }

        public IEnumerator<HttpCookie> GetEnumerator()
        {
            return this.httpCookies.Values.GetEnumerator();

        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var cookie in this.httpCookies.Values)
            {
                sb.AppendLine($"Set-Cookie:{cookie}").Append(GlobalConstants.HttpNewLine);
            }

            return sb.ToString();
        }
    }
}
