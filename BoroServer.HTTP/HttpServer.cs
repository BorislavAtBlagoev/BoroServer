namespace BoroServer.HTTP
{
    using System;
    using System.Text;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    public class HttpServer : IHttpServer
    {
        private readonly IEnumerable<Route> routeTable;

        public HttpServer(IEnumerable<Route> routeTable)
        {
            this.routeTable = routeTable;
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
                var route = this.routeTable
                    .FirstOrDefault(x => x.Path.ToLower() == request.Path.ToLower() && x.Method == request.Method);

                if (route != null)
                {
                    response = route.Action(request);
                }
                else
                {
                    response = new HttpResponse(new byte[0], "text/html", StatusCode.BadRequest);
                }


                var sessionCookie = request.Cookies.FirstOrDefault(x => x.Name == HttpConstants.SessionCookieName);
                if (sessionCookie != null)
                {
                    if (HttpRequestParser.isGeneratedNow)
                    {
                        var responseSessionCookie = new ResponseCookie(sessionCookie.Name, sessionCookie.Value);
                        responseSessionCookie.Path = "/";
                        response.Cookies.Add(responseSessionCookie);
                    }
                }

                var resposeAsByte = Encoding.UTF8.GetBytes(response.ToString());
                await stream.WriteAsync(resposeAsByte, 0, resposeAsByte.Length);
                await stream.WriteAsync(response.Body, 0, response.Body.Length);
            }
        }
    }
}
