namespace SIS.MvcFramework
{
    using System;
    using System.Net;
    using System.Net.Sockets;

    using Routing;
    using HTTP.Common;
    using System.Threading.Tasks;

    public class Server
    {
        private const string LocalHostIpAddres = "127.0.0.1";

        private readonly int port;

        private readonly TcpListener tcpListener;

        private IServerRoutingTable serverRoutingTable;

        private bool isRunning;

        public Server(int port, IServerRoutingTable serverRoutingTable)
        {
            CoreValidator.ThrowIfNull(serverRoutingTable, nameof(serverRoutingTable));

            this.port = port;
            this.serverRoutingTable = serverRoutingTable;

            this.tcpListener = new TcpListener(IPAddress.Parse(LocalHostIpAddres), port);
        }

        private async Task ListenAsync(Socket client)
        {
            var connectionHandler = new ConnectionHandler(client, this.serverRoutingTable);
            await connectionHandler.ProcessRequestAsync();
        }

        public void Run()
        {
            this.tcpListener.Start();
            this.isRunning = true;

            Console.WriteLine($"Server started at http://{LocalHostIpAddres}:{this.port}");

            while (this.isRunning)
            {
              
               
                var client = this.tcpListener.AcceptSocketAsync().GetAwaiter().GetResult();

                Task.Run(() => this.ListenAsync(client));
            }
        }
    }
}
