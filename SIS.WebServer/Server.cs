namespace SIS.WebServer
{
    using System;
    using System.Net;
    using System.Net.Sockets;

    using Routing.Contracts;
    using HTTP.Common;

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

        private void Listen(Socket client)
        {
            var connectionHandler = new ConnectionHandler(client, this.serverRoutingTable);
            connectionHandler.ProcessRequest();
        }

        public void Run()
        {
            this.tcpListener.Start();
            this.isRunning = true;

            Console.WriteLine($"Server started at http://{LocalHostIpAddres}:{this.port}");

            while (this.isRunning)
            {
                Console.WriteLine($"Waiting for client...");

                var client = this.tcpListener.AcceptSocket();

                this.Listen(client);
            }
        }
    }
}
