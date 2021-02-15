namespace BoroServer.ConsoleTestApp.Controllers
{
    using System.IO;
    using System.Text;

    using BoroServer.HTTP;
    using BoroServer.MvcFramework;
    using BoroServer.ConsoleTestApp.Validations;

    public class StaticFilesAndTestsController : Controller
    {
        public HttpResponse Image(HttpRequest request)
        {
            var image = @"..\..\..\wwwroot\1.jpg";
            var imageAsByte = File.ReadAllBytes(image);
            var response = new HttpResponse(imageAsByte, "image/jpeg");

            return response;
        }

        public HttpResponse Favicon(HttpRequest request)
        { 
            return FileRead(@"wwwroot\favicon.ico", "image/vnd.microsoft.icon");
        }

        public HttpResponse Nikol(HttpRequest request)
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

            return CookieValidation.CookieSender(request, response, responseCookie, responseCookie2);
        }

        public HttpResponse CustomCss(HttpRequest request)
        {
            return FileRead(@"wwwroot\css\custom.css", "text/css");
        }

        public HttpResponse BootstrapCss(HttpRequest request)
        {
            return FileRead(@"wwwroot\css\bootstrap.min.css", "text/css");
        }

        public HttpResponse CustomJs(HttpRequest request)
        {
            return FileRead(@"wwwroot\js\custom.js", "text/javascript");
        }

        public HttpResponse BootstrapJs(HttpRequest request)
        {
            return FileRead(@"wwwroot\js\bootstrap.bundle.min.js", "text/javascript");
        }

        private static HttpResponse FileRead(string path, string contentType)
        {
            var body = File.ReadAllBytes(path);
            return new HttpResponse(body, contentType);
        }
    }
}
