using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SandBox
{
    class Program
    {
        static void Main(string[] args)
        {
            const string NewLine = "\r\n";

            TcpListener tcpListener = new TcpListener(IPAddress.Loopback, 80);

            tcpListener.Start();
            

            while (true)
            {
                TcpClient client = tcpListener.AcceptTcpClient();
                using (NetworkStream stream = client.GetStream())
                {
                    var requestBytes = new byte[100000];
                    int readBytes = stream.Read(requestBytes, 0, requestBytes.Length);
                    var stringRequest = Encoding.UTF8.GetString(requestBytes, 0, readBytes);
                    Console.WriteLine(new string('=', 70));
                    Console.WriteLine(stringRequest);

                    string responceBody = "<form method = 'post'><input type ='text' name = 'tweet' placeholder = 'Enter tweet...'/><input type ='submit'/></form>";

                    string responce = "HTTP/1.0 201 CREATED" + NewLine +
                                        "Content-Type: text/html" + NewLine +
                                        "SERVER: MyCustomServer/1.0" + NewLine +
                                        $"Content-Lenght: {responceBody.Length}" + NewLine + NewLine +
                                        responceBody;

                    byte[] responceBytes = Encoding.UTF8.GetBytes(responce);
                    stream.Write(responceBytes, 0, responceBytes.Length);

                    
                }
            }


        }
    }
}
