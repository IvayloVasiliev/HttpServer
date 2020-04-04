using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using SIS.HTTP.Enums;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using SIS.HTTP.Headers;
using System.Globalization;

namespace SandBox
{
    class Program
    {
        static void Main(string[] args)
        {

            //string request = "POST /url/asd?name=john&id=1#fragment HTTP/1.1\r\n"
            //    + "Authorization: Basic 123456879\r\n"
            //    + "Date: " + DateTime.Now + "\r\n"
            //    + "Host: localhost:12345\r\n"
            //    + "\r\n"
            //    + "username=John&password=123456";
            //HttpRequest httpRequest = new HttpRequest(request);

                    //HttpResponse responce = new HttpResponse(HttpResponseStatusCode.InternalServerError);
                    //responce.AddHeader(new HttpHeader("Host", "localhost:12345"));
                    //responce.AddHeader(new HttpHeader("Date", DateTime.Now.ToString(CultureInfo.InvariantCulture)));

                    //responce.Content = Encoding.UTF8.GetBytes("<h1> Hello World!</h1>");
                    //Console.WriteLine(Encoding.UTF8.GetString(responce.GetBytes()));

            //const string NewLine = "\r\n";
            //TcpListener tcpListener = new TcpListener(IPAddress.Loopback, 80);
            //tcpListener.Start();         

            //while (true)
            //{
            //    TcpClient client = tcpListener.AcceptTcpClient();
            //    using (NetworkStream stream = client.GetStream())
            //    {
            //        var requestBytes = new byte[100000];
            //        int readBytes = stream.Read(requestBytes, 0, requestBytes.Length);
            //        var stringRequest = Encoding.UTF8.GetString(requestBytes, 0, readBytes);
            //        Console.WriteLine(new string('=', 70));
            //        Console.WriteLine(stringRequest);

            //        string responceBody = "<form method = 'post'><input type ='text' name = 'tweet' placeholder = 'Enter tweet...'/><input type ='submit'/></form>";

            //        string responce = "HTTP/1.0 201 CREATED" + NewLine +
            //                            "Content-Type: text/html" + NewLine +
            //                            "SERVER: MyCustomServer/1.0" + NewLine +
            //                            $"Content-Lenght: {responceBody.Length}" + NewLine + NewLine +
            //                            responceBody;

            //        byte[] responceBytes = Encoding.UTF8.GetBytes(responce);
            //        stream.Write(responceBytes, 0, responceBytes.Length);
            //    }
            //}
        }
    }
}
