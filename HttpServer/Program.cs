using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using SIS.HTTP.Enums;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using SIS.HTTP.Headers;
using System.Globalization;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace SandBox
{
    class Program
    {
        const string NewLine = "\r\n";

        static void Main(string[] args)
        {
            #region
            //HTTP REQUEST
            //string request = "POST /url/asd?name=john&id=1#fragment HTTP/1.1\r\n"
            //    + "Authorization: Basic 123456879\r\n"
            //    + "Date: " + DateTime.Now + "\r\n"
            //    + "Host: localhost:12345\r\n"
            //    + "\r\n"
            //    + "username=John&password=123456";
            //HttpRequest httpRequest = new HttpRequest(request);

            //HTTP RESPONCE
            //HttpResponse responce = new HttpResponse(HttpResponseStatusCode.InternalServerError);
            //responce.AddHeader(new HttpHeader("Host", "localhost:12345"));
            //responce.AddHeader(new HttpHeader("Date", DateTime.Now.ToString(CultureInfo.InvariantCulture)));

            //responce.Content = Encoding.UTF8.GetBytes("<h1> Hello World!</h1>");
            //Console.WriteLine(Encoding.UTF8.GetString(responce.GetBytes()));
            #endregion



            //TcpListener tcpListener = new TcpListener(IPAddress.Loopback, 80);
            //tcpListener.Start();

            //while (true)
            //{
            //    TcpClient client = tcpListener.AcceptTcpClient();

            //    Task.Run(() => ProcessClient(client));
            //}
        }

        public static async Task ProcessClient( TcpClient client)
        {
            using (NetworkStream stream = client.GetStream())
            {
                var requestBytes = new byte[100000];

                int readBytes = await stream.ReadAsync(requestBytes, 0, requestBytes.Length);
                var stringRequest = Encoding.UTF8.GetString(requestBytes, 0, readBytes);
                Console.WriteLine(new string('=', 70));
                Console.WriteLine(stringRequest);

                //string responceBody = "<form method = 'post'><input type ='text' name = 'tweet' placeholder = 'Enter tweet...'/><input type ='submit'/></form>";
                string responceBody = DateTime.Now.ToString();
                    
                string responce = "HTTP/1.0 200 OK" + NewLine +
                                    "Content-Type: text/html" + NewLine +
                                    "Set-Cookie: cookie1=test" + NewLine +
                                    "SERVER: MyCustomServer/1.0" + NewLine +
                                    $"Content-Lenght: {responceBody.Length}" + NewLine + NewLine +
                                    responceBody;

                byte[] responceBytes = Encoding.UTF8.GetBytes(responce);
                await stream.WriteAsync(responceBytes, 0, responceBytes.Length);
            }
        }
    }
}
