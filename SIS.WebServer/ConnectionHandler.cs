    namespace SIS.MvcFramework
{
    using System;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;

    using HTTP.Common;
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

    public class ConnectionHandler
    {
        private readonly Socket client;

        private readonly IServerRoutingTable serverRoutingTable;

        public ConnectionHandler(Socket client, IServerRoutingTable serverRoutingTable)
        {
            CoreValidator.ThrowIfNull(client, nameof(client));
            CoreValidator.ThrowIfNull(serverRoutingTable, nameof(serverRoutingTable));

            this.client = client;
            this.serverRoutingTable = serverRoutingTable;
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
                httpResponce = new TextResult(e.Message, HttpResponseStatusCode.BadRequest);
            }
            catch (Exception e)
            {
                httpResponce = new TextResult(e.Message, HttpResponseStatusCode.InternalServerError);
            }

            this.PrepareResponce(httpResponce);
            this.client.Shutdown(SocketShutdown.Both);
        }

        private void PrepareResponce(IHttpResponse httpResponse)
        {
            byte[] byteSegments = httpResponse.GetBytes();

            this.client.Send(byteSegments, SocketFlags.None);
        }

        private string SetRequestSession(IHttpRequest httpRequest)
        {
            string sessionId = null;

            if (httpRequest.Cookies.ContainCookie(HttpSessionStorage.SessionCookieKey))
            {
                var cookie = httpRequest.Cookies.GetCookie(HttpSessionStorage.SessionCookieKey);
                sessionId = cookie.Value;
            }
            else
            {
                sessionId = Guid.NewGuid().ToString();
            }

            httpRequest.Session = HttpSessionStorage.GetSession(sessionId);
            return httpRequest.Session.Id;
        }

        private void SetResponceSession(IHttpResponse responce, string sessionId)
        {
            if (sessionId != null)
            {
                responce.Cookies.AddCookie(new HttpCookie(HttpSessionStorage.SessionCookieKey, sessionId));
            }        
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
            string folderPrefix = "/../../../../";
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            string resourceFolderPath = "Resources/";
            string requestedResource = httpRequest.Path;

            string fullPathToResource = assemblyLocation + folderPrefix 
                                                + resourceFolderPath + requestedResource;

            if (File.Exists(fullPathToResource)) 
            {
                byte[] content = File.ReadAllBytes(fullPathToResource);
                return new InlineResourceResult(content, HttpResponseStatusCode.Found);
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
    }
}
