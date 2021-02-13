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
        IDictionary<string, Func<HttpRequest, HttpResponse>> routeTable =
            new Dictionary<string, Func<HttpRequest, HttpResponse>>();

        public void AddRoute(string path, Func<HttpRequest, HttpResponse> action)
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

                var requestAsString = Encoding.UTF8.GetString(data.ToArray());
                var request = HttpRequestParser.Parse(requestAsString);

                HttpResponse response;

                if (this.routeTable.ContainsKey(request.Path))
                {
                    var action = this.routeTable[request.Path];
                    response = action(request);
                }
                else
                {
                    var response404Body = $"<h1>Page not found!</h1>";
                    var response404BodyAsByte = Encoding.UTF8.GetBytes(response404Body);
                    response = new HttpResponse(response404BodyAsByte, "text/html", StatusCode.BadRequest);
                }

                var resposeAsByte = Encoding.UTF8.GetBytes(response.ToString());
                await stream.WriteAsync(resposeAsByte, 0, resposeAsByte.Length);
                await stream.WriteAsync(response.Body, 0, response.Body.Length);
            }
        }
    }
}
