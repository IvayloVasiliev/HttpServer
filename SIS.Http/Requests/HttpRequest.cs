namespace SIS.HTTP.Requests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Enums;

    using Headers;
    using Headers.Contracts;
    using Requests.Contracts;
    using Exceptions;

    public class HttpRequest : IHttpRequest
    {
        public HttpRequest(string requestString)
        {
            CoreValidator.ThrowIfNullOrEmpty(requestString, nameof(requestString));

            this.FormData = new Dictionary<string, object>();
            this.QueryData = new Dictionary<string, object>();
            this.Headers = new HttpHeaderCollection();

            this.ParseRequest(requestString);

        }

        public string Path { get; private set; }

        public string Url { get; private set; }

        public Dictionary<string, object> FormData { get; }

        public Dictionary<string, object> QueryData { get; }

        public IHttpHeaderCollection Headers { get; }

        public HttpRequestMethod RequestMethod { get; private set; }

        private bool isValidRequestLine(string[] requestLineParams)
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
            HttpRequestMethod method;
            bool parseResult = HttpRequestMethod.TryParse(requestLineParams[0], true, out method);

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
            plainHeaders.Select(plainHeader => plainHeader.Split(new[] { ':', ' ' },
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

        private bool isValidRequestQueryString(string queryString, string[] queryParameters)
        {
            throw new NotImplementedException();
        }

        private void ParseRequestQueryParameters()
        {
            //?name="pesho"&id="11"
            this.Url.Split('?', '#')[1]
                .Split('&')
                .Select(plainQueryParameter => plainQueryParameter.Split('='))
                .ToList()
                .ForEach(queryParameterKVP => this.QueryData.Add(queryParameterKVP[0], queryParameterKVP[1]));
        }

        private void ParseRequestFormDataParameters(string requestBody)
        { 
            requestBody
                .Split('&')
                .Select(plainQueryParameter => plainQueryParameter.Split('='))
                .ToList()
                .ForEach(queryParameterKVP => this.FormData.Add(queryParameterKVP[0], queryParameterKVP[1]));

            //TODO : Parse multiple parameters by name
        }

        private void ParseRequestParameters(string requestBody)
        {
            this.ParseRequestQueryParameters();
            this.ParseRequestFormDataParameters(requestBody); //todo

        }

        private void ParseRequest(string requestString)
        {
            string[] splitRequestString = requestString
                .Split(new[] { GlobalConstants.HttpNewLine }, StringSplitOptions.None);

            string[] requestLineParams = splitRequestString[0]
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (!isValidRequestLine(requestLineParams))
            {
                throw new BadRequestException();
            }

            this.ParseRequestMethod(requestLineParams);
            this.ParseRequestUrl(requestLineParams);
            this.ParseRequestPath();

            this.ParseRequestHeaders(this.ParsePlainRequestHeaders(splitRequestString).ToArray());
            //this.ParseCookies(); TODO

            this.ParseRequestParameters(splitRequestString[splitRequestString.Length - 1]);
        }  
    }
}
