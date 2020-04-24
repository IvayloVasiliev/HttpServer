    namespace SIS.MvcFramework
{
    using System;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;

    using HTTP.Enums;
    using HTTP.Exceptions;
    using HTTP.Requests;
    using HTTP.Responses;
    using Results;
    using Routing;
    using HTTP.Cookies;
    using MvcFramework.Sessions;
    using System.Reflection;
    using System.IO;
    using SIS.HTTP.Sessions;
    using SIS.Common;

    public class ConnectionHandler
    {
        private readonly Socket client;

        private readonly IServerRoutingTable serverRoutingTable;

        private readonly IHttpSessionStorage httpSessionStorage;

        public ConnectionHandler(Socket client, IServerRoutingTable serverRoutingTable, IHttpSessionStorage httpSessionStorage)
        {
            client.ThrowIfNull(nameof(client));
            serverRoutingTable.ThrowIfNull(nameof(serverRoutingTable));
            httpSessionStorage.ThrowIfNull(nameof(httpSessionStorage));

            this.client = client;
            this.serverRoutingTable = serverRoutingTable;
            this.httpSessionStorage = httpSessionStorage;
        }
        
         
        private async Task<IHttpRequest> ReadRequestAsync()
        {
            var result = new StringBuilder();
            var data = new ArraySegment<byte>(new byte[1024]);

            while (true)
            {
                int numberOfBytesRead = await this.client.ReceiveAsync(data, SocketFlags.None);

                if (numberOfBytesRead == 0)
                {
                    break;
                }

                var bytesAsString = Encoding.UTF8.GetString(data.Array, 0, numberOfBytesRead);
                result.Append(bytesAsString);

                if (numberOfBytesRead < 1023)
                {
                    break;
                }
            }

            if (result.Length == 0)
            {
                return null;
            }

            return new HttpRequest(result.ToString());
        }

        private IHttpResponse ReturnIfResource(IHttpRequest httpRequest)
        {
            string folderPrefix = "/../";
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            string resourceFolderPath = "Resources/";
            string requestedResource = httpRequest.Path;

            string fullPathToResource = assemblyLocation + folderPrefix 
                                                + resourceFolderPath + requestedResource;

            if (File.Exists(fullPathToResource)) 
            {
                byte[] content = File.ReadAllBytes(fullPathToResource);
                return new InlineResourceResult(content, HttpResponseStatusCode.Ok);
            }
            else
            {
                return new TextResult($"Route with method {httpRequest.RequestMethod} and path" +
                  $"\"{httpRequest.Path}\" not found", HttpResponseStatusCode.NotFound);
            }          
        }

        private IHttpResponse HandleRequests(IHttpRequest httpRequest)
        {
            if (!this.serverRoutingTable.Contains(httpRequest.RequestMethod, httpRequest.Path))
            {
                return this.ReturnIfResource(httpRequest);
            }

            return this.serverRoutingTable.Get(httpRequest.RequestMethod, httpRequest.Path)
                .Invoke(httpRequest);
        }

        private string SetRequestSession(IHttpRequest httpRequest)
        {
            if (httpRequest.Cookies.ContainCookie(HttpSessionStorage.SessionCookieKey))
            {
                var cookie = httpRequest.Cookies.GetCookie(HttpSessionStorage.SessionCookieKey);
                string sessionId = cookie.Value;

                if (this.httpSessionStorage.ContainsSession(sessionId))
                {
                    httpRequest.Session = this.httpSessionStorage.GetSession(sessionId);
                }

            }

            if (httpRequest.Session == null)
            {
                string sessionId = Guid.NewGuid().ToString();

                httpRequest.Session = this.httpSessionStorage.GetSession(sessionId);
            }

            return httpRequest.Session?.Id;
        }

        private void SetResponceSession(IHttpResponse httpResponce, string sessionId)
        {
            IHttpSession responceSession = this.httpSessionStorage.GetSession(sessionId);

            if (responceSession.IsNew)
            {
                responceSession.IsNew = false;
                httpResponce.AddCookie(new HttpCookie(HttpSessionStorage.SessionCookieKey, responceSession.Id));
            }
        }

        private void PrepareResponce(IHttpResponse httpResponse)
        {
            byte[] byteSegments = httpResponse.GetBytes();

            this.client.Send(byteSegments, SocketFlags.None);
        }

        public async Task ProcessRequestAsync()
        {
            IHttpResponse httpResponce = null;

            try
            {
                IHttpRequest httpRequest = await this.ReadRequestAsync();

                if (httpRequest != null)
                {
                    Console.WriteLine($"Processing: {httpRequest.RequestMethod} {httpRequest.Path}...");
                    string sessionId = this.SetRequestSession(httpRequest);
                    httpResponce = this.HandleRequests(httpRequest);
                    this.SetResponceSession(httpResponce, sessionId);

                }
            }
            catch (BadRequestException e)
            {
                httpResponce = new TextResult(e.ToString(), HttpResponseStatusCode.BadRequest);
            }
            catch (Exception e)
            {
                httpResponce = new TextResult(e.ToString(), HttpResponseStatusCode.InternalServerError);
            }

            this.PrepareResponce(httpResponce);
            this.client.Shutdown(SocketShutdown.Both);
        }

    }
}
