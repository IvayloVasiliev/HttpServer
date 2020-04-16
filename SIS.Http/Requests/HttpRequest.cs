namespace SIS.HTTP.Requests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Enums;

    using Headers;
    using Requests;
    using Exceptions;
    using Cookies;
    using SIS.HTTP.Sessions.Contracts;

    public class HttpRequest : IHttpRequest
    {
        public HttpRequest(string requestString)
        {
            CoreValidator.ThrowIfNullOrEmpty(requestString, nameof(requestString));

            this.FormData = new Dictionary<string, object>();
            this.QueryData = new Dictionary<string, object>();
            this.Headers = new HttpHeaderCollection();
            this.Cookies = new HttpCookieCollection();

            this.ParseRequest(requestString);

        }

        public string Path { get; private set; }

        public string Url { get; private set; }

        public Dictionary<string, object> FormData { get; }

        public Dictionary<string, object> QueryData { get; }

        public IHttpHeaderCollection Headers { get; }

        public HttpRequestMethod RequestMethod { get; private set; }

        public IHttpCookieCollections Cookies { get; }

        public IHttpSession Session { get; set; }

        private bool IsValidRequestLine(string[] requestLineParams)
        {
            if (requestLineParams.Length != 3 
                || requestLineParams[2] != GlobalConstants.HttpOneProtocolFragment)
            {
                return false;
            }
            return true;
        }

        private void ParseRequestMethod(string[] requestLineParams)
        {
           
            bool parseResult = HttpRequestMethod.TryParse(requestLineParams[0], true, out HttpRequestMethod method);

            if (!parseResult)
            {
                throw new BadRequestException(string.Format(GlobalConstants.UnsupportedHttpMethodExceptionMessage,
                    requestLineParams[0]));
            }

            this.RequestMethod = method;
        }

        private void ParseRequestUrl(string[] requestLineParams)
        {
            this.Url = requestLineParams[1];
        }

        private void ParseRequestPath()
        {
            this.Path = this.Url.Split('?')[0];
        }

        private void ParseRequestHeaders(string[] plainHeaders)
        {
            plainHeaders.Select(plainHeader => plainHeader.Split(new[] { ": " },
                StringSplitOptions.RemoveEmptyEntries))
                .ToList()
                .ForEach(headerKVP => this.Headers.AddHeader(new HttpHeader(headerKVP[0], headerKVP[1])));
        }

        private IEnumerable<string> ParsePlainRequestHeaders(string[] requestLines)
        {
            for (int i = 1; i < requestLines.Length - 1; i++)
            {
                if (!string.IsNullOrEmpty(requestLines[i]))
                {
                    yield return requestLines[i];
                }
            }
            
        }

        private bool IsValidRequestQueryString(string queryString, string[] queryParameters)
        {
            CoreValidator.ThrowIfNullOrEmpty(queryString, nameof(queryString));

            return true; //todo
        }

        private bool HasQueryString()
        {
            return this.Url.Split('?').Length > 1;
        }

        private void ParseRequestQueryParameters()
        {
            //?name="pesho"&id="11"
            if (this.HasQueryString())
            {
                this.Url
                .Split('?', '#')[1]
                .Split('&')
                .Select(plainQueryParameter => plainQueryParameter.Split('='))
                .ToList()
                .ForEach(queryParameterKVP => this.QueryData.Add(queryParameterKVP[0], queryParameterKVP[1]));
            }
        }

        private void ParseRequestFormDataParameters(string requestBody)
        {
            //if (!string.IsNullOrEmpty(requestBody))
            //{
            //    requestBody
            //    .Split('&')
            //    .Select(plainQueryParameter => plainQueryParameter.Split('='))
            //    .ToList()
            //    .ForEach(queryParameterKVP => this.FormData.Add(queryParameterKVP[0], queryParameterKVP[1]));
            //}

            //TODO : Parse multiple parameters by name

            if (string.IsNullOrEmpty(requestBody) == false)
            {
                var paramsPairs = requestBody
                   .Split('&')
                   .Select(plainQueryParameter => plainQueryParameter.Split('='))
                   .ToList();

                foreach (var paramPair in paramsPairs)
                {
                    string key = paramPair[0];
                    string value = paramPair[1];

                    if (this.FormData.ContainsKey(key) == false)
                    {
                        this.FormData.Add(key, new HashSet<string>());
                    }

                    ((ISet<string>)this.FormData[key]).Add(value);
                }
            }
        }

        private void ParseRequestParameters(string requestBody)
        {
            this.ParseRequestQueryParameters();
            this.ParseRequestFormDataParameters(requestBody); //todo
        }

        private void ParseCookies()
        {
            if (this.Headers.ContainsHeader(HttpHeader.Cookie))
            {
                string value = this.Headers.GetHeader(HttpHeader.Cookie).Value;
                string[] unparsedCookies = value.Split(new[] { "; "}, StringSplitOptions.RemoveEmptyEntries);

                foreach (string unpasedCookie in unparsedCookies)
                {
                    string[] cookieKVP = unpasedCookie.Split(new[] {'='}, 2);

                    HttpCookie httpCookie = new HttpCookie(cookieKVP[0], cookieKVP[1], false);

                    this.Cookies.AddCookie(httpCookie);
                }
            }
        }

        private void ParseRequest(string requestString)
        {
            string[] splitRequestString = requestString
                .Split(new[] { GlobalConstants.HttpNewLine }, StringSplitOptions.None);

            string[] requestLineParams = splitRequestString[0]
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (!this.IsValidRequestLine(requestLineParams))
            {
                throw new BadRequestException();
            }

            this.ParseRequestMethod(requestLineParams);
            this.ParseRequestUrl(requestLineParams);
            this.ParseRequestPath();

            this.ParseRequestHeaders(this.ParsePlainRequestHeaders(splitRequestString).ToArray());
            this.ParseCookies(); 

            this.ParseRequestParameters(splitRequestString[splitRequestString.Length - 1]);
        }
    }
}
