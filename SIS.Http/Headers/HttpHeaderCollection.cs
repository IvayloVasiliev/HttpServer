using SIS.HTTP.Headers.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.HTTP.Headers
{
    public class HttpHeaderCollection : IHttpHeaderCollection
    {
        private readonly Dictionary<string, HttpHeader> headers;

        public void AddHeader(HttpHeader header)
        {
            throw new NotImplementedException();
        }

        public bool ContainsHeader(string key)
        {
            throw new NotImplementedException();
        }

        public HttpHeader GetHeader(string key)
        {
            throw new NotImplementedException();
        }
    }
}
