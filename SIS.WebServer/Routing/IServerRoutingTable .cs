namespace SIS.MvcFramework.Routing
{
    using System;

    using HTTP.Enums;
    using HTTP.Requests;
    using HTTP.Responses;

    public interface IServerRoutingTable
    {
        void Add(HttpRequestMethod method, string path, Func<IHttpRequest, IHttpResponse> func);

        bool Contains(HttpRequestMethod requestMethod, string path);

        Func<IHttpRequest, IHttpResponse> Get(HttpRequestMethod requestMethod, string path);
    }
}
