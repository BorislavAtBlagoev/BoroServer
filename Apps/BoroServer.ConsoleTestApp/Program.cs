namespace BoroServer.ConsoleTestApp
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using BoroServer.HTTP;

    public class Program
    {
        public static async Task Main()
        {
            IHttpServer server = new HttpServer();
            server.AddRoute("/", HomePage);
            server.AddRoute("/Nikol", Nikol);
            await server.StartAsync(80);
        }

        private static HttpResponse Nikol(HttpRequest arg)
        {
            var body = $"<h1>Nikol</h1>";
            var bodyAsByte = Encoding.UTF8.GetBytes(body);
            var response = new HttpResponse(bodyAsByte);
            response.Cookies.Add(new ResponseCookie("sid=testtest123")
            {
                IsHttpOnly = true,
                MaxAge = 60
            });

            return response;
        }

        private static HttpResponse HomePage(HttpRequest request)
        {
            var body = $"<h1>Hello from Boro's Server111</h1>";
            var bodyAsByte = Encoding.UTF8.GetBytes(body);
            var response = new HttpResponse(bodyAsByte);
            return response;
        }
    }
}
