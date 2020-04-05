namespace SIS.WebServer
{
    using System;
    using System.Net.Sockets;
    using System.Text;

    using HTTP.Common;
    using HTTP.Enums;
    using HTTP.Exceptions;
    using HTTP.Requests;
    using HTTP.Requests.Contracts;
    using HTTP.Responses.Contracts;
    using Results;
    using Routing.Contracts;


    public class ConnectionHandler
    {
        private readonly Socket client;

        private IServerRoutingTable serverRoutingTable;

        public ConnectionHandler(Socket client, IServerRoutingTable serverRoutingTable)
        {
            CoreValidator.ThrowIfNull(client, nameof(client));
            CoreValidator.ThrowIfNull(serverRoutingTable, nameof(serverRoutingTable));

            this.client = client;
            this.serverRoutingTable = serverRoutingTable;
        }

        public void ProcessRequest()
        {
            IHttpResponse httpResponce = null;

            try
            {
                IHttpRequest httpRequest = this.ReadRequest();

                if (httpRequest != null)
                {
                    Console.WriteLine($"Processing: {httpRequest.RequestMethod} {httpRequest.Path}...");

                    httpResponce = this.HandleRequests(httpRequest);
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

        private IHttpRequest ReadRequest()
        {
            var result = new StringBuilder();
            var data = new ArraySegment<byte>(new byte[1024]);

            while (true)
            {
                int numberOfBytesRead = this.client.Receive(data.Array, SocketFlags.None);

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

        private IHttpResponse HandleRequests(IHttpRequest httpRequest)
        {
            if (!this.serverRoutingTable.Contains(httpRequest.RequestMethod, httpRequest.Path))
            {
                return new TextResult($"Route with method {httpRequest.RequestMethod} and path" +
                    $"{httpRequest.Path} not found", HttpResponseStatusCode.NotFound);
            }

            return this.serverRoutingTable.Get(httpRequest.RequestMethod, httpRequest.Path)
                .Invoke(httpRequest);
        } 

        private void PrepareResponce(IHttpResponse httpResponse)
        {
            byte[] byteSegments = httpResponse.GetBytes();

            this.client.Send(byteSegments, SocketFlags.None);
        }
    }
}
