namespace SIS.MvcFramework.Routing
{
    using HTTP.Enums;
    using HTTP.Requests;
    using HTTP.Responses;
    using SIS.Common;
    using System;
    using System.Collections.Generic;

    public class ServerRoutingTable : IServerRoutingTable
    {
        private Dictionary<HttpRequestMethod, Dictionary<string, Func<IHttpRequest, IHttpResponse>>> routingTable;

        public ServerRoutingTable()
        {
            this.routingTable = new Dictionary<HttpRequestMethod, Dictionary<string, Func<IHttpRequest, IHttpResponse>>>
            {
                [HttpRequestMethod.Get] = new Dictionary<string, Func<IHttpRequest, IHttpResponse>>(),
                [HttpRequestMethod.Post] = new Dictionary<string, Func<IHttpRequest, IHttpResponse>>(),
                [HttpRequestMethod.Put] = new Dictionary<string, Func<IHttpRequest, IHttpResponse>>(),
                [HttpRequestMethod.Delete] = new Dictionary<string, Func<IHttpRequest, IHttpResponse>>(),
            };
        }

        public void Add(HttpRequestMethod method, string path, Func<IHttpRequest, IHttpResponse> func)
        {
            method.ThrowIfNull(nameof(method));
            func.ThrowIfNull(nameof(func));
            path.ThrowIfNullOrEmpty(nameof(path));

            this.routingTable[method].Add(path, func);
        }

        public bool Contains(HttpRequestMethod method, string path)
        {
            method.ThrowIfNull(nameof(method));
            path.ThrowIfNullOrEmpty(nameof(path));

            return this.routingTable.ContainsKey(method) && this.routingTable[method].ContainsKey(path);
        }

        public Func<IHttpRequest, IHttpResponse> Get(HttpRequestMethod method, string path)
        {
            method.ThrowIfNull(nameof(method));
            path.ThrowIfNullOrEmpty(nameof(path));

            return this.routingTable[method][path];
        }
    }
}
