namespace BoroServer.ConsoleTestApp
{
    using System.IO;
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
            server.AddRoute("/favicon.ico", Favicon);
            await server.StartAsync(80);
        }

        private static HttpResponse Favicon(HttpRequest request)
        {
            var favicon = File.ReadAllBytes(@"..\..\..\wwwroot\favicon.ico");
            var response = new HttpResponse(favicon, "image/vnd.microsoft.icon");

            return response;
        }

        private static HttpResponse Nikol(HttpRequest request)
        {
            var body = $"<h1>Nikol</h1>";
            var bodyAsByte = Encoding.UTF8.GetBytes(body);

            var response = new HttpResponse(bodyAsByte);
            var responseCookie = new ResponseCookie("sid=testtest123")
            {
                IsHttpOnly = true,
                MaxAge = 60
            };
            var responseCookie2 = new ResponseCookie("sid2=aaaaaaaa")
            {
                MaxAge = 60 * 60
            };

            return CookieSender(request, response, responseCookie, responseCookie2);
        }

        private static HttpResponse HomePage(HttpRequest request)
        {
            var body = $"<h1>Hello from Boro's Server</h1>";
            var bodyAsByte = Encoding.UTF8.GetBytes(body);
            var response = new HttpResponse(bodyAsByte);

            return response;
        }

        private static HttpResponse CookieSender(HttpRequest request, HttpResponse response, params ResponseCookie[] responseCookies)
        {
            if (request.Cookies.Count == 0)
            {
                foreach (var responseCookie in responseCookies)
                {
                    response.Cookies.Add(responseCookie);
                }
            }
            else
            {
                foreach (var responseCookie in responseCookies)
                {
                    var isSended = request.Cookies.Any(x => x.Name == responseCookie.Name && x.Value == responseCookie.Value);
                    if (!isSended)
                    {
                        response.Cookies.Add(responseCookie);
                    }
                }
            }

            return response;
        }
    }
}
