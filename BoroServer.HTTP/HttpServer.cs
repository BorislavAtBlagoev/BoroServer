namespace BoroServer.HTTP
{
    using System;
    using System.Text;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Linq;

    public class HttpServer : IHttpServer
    {
        IDictionary<string, Func<HttpRequestWithCtor, HttpResponse>> routeTable =
            new Dictionary<string, Func<HttpRequestWithCtor, HttpResponse>>();

        public void AddRoute(string path, Func<HttpRequestWithCtor, HttpResponse> action)
        {
            if (!routeTable.ContainsKey(path))
            {
                routeTable.Add(path, action);
            }
            else
            {
                routeTable[path] = action;
            }
        }

        public async Task StartAsync(int port)
        {
            TcpListener listener = new TcpListener(IPAddress.Loopback, port);
            listener.Start();

            while (true)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();
                ProcessClientAsync(client);
            }
        }

        private async Task ProcessClientAsync(TcpClient client)
        {
            using (NetworkStream stream = client.GetStream())
            {
                int position = 0;
                byte[] buffer = new byte[HttpConstants.BufferSize];
                List<byte> data = new List<byte>();

                while (true)
                {
                    int count = await stream.ReadAsync(buffer, position, buffer.Length);
                    position += count;

                    if (count < buffer.Length)
                    {
                        byte[] partialBuffer = new byte[count];
                        Array.Copy(buffer, partialBuffer, count);
                        data.AddRange(partialBuffer);
                        break;
                    }
                    else
                    {
                        data.AddRange(buffer);
                    }
                }

                var html = $"<h1>Hello from Boro's Server111</h1>";
                /*var response = "HTTP/1.1 200 OK" + HttpConstants.NewLine +
                   "Server: BoroServer 2020" + HttpConstants.NewLine +
                   "Content-Type: text/html; charset=utf-8" + HttpConstants.NewLine +
                   "Content-Lenght: " + html.Length + HttpConstants.NewLine +
                   HttpConstants.NewLine +
                   html + HttpConstants.NewLine;*/

                var body = Encoding.UTF8.GetBytes(html);
                var response = new HttpResponse(body);
                response.Cookies.Add(new ResponseCookie("sid=asdasdas"));
                response.Cookies.FirstOrDefault().IsHttpOnly = true;
                response.Cookies.FirstOrDefault().MaxAge = 60;
                var responseAsBytes = Encoding.UTF8.GetBytes(response.ToString());
                await stream.WriteAsync(responseAsBytes, 0, responseAsBytes.Length);
                await stream.WriteAsync(body, 0, body.Length);


                var requestAsString = Encoding.UTF8.GetString(data.ToArray());
                var requestObj = new HttpRequestWithCtor(requestAsString);
                var requestObjFromParser = HttpRequestParser.Parse(requestAsString);
                
                foreach (var cookie in requestObjFromParser.Cookies)
                {
                    Console.WriteLine(cookie.ToString());
                }

                foreach (var cookie in requestObjFromParser.Headers)
                {
                    Console.WriteLine(cookie.ToString());
                }
                Console.WriteLine($"{requestObjFromParser.Headers.Count} {requestObjFromParser.HttpVersion} {requestObjFromParser.Method} {requestObjFromParser.Path} {requestObjFromParser.Cookies.Count}");
            }
        }
    }
}
