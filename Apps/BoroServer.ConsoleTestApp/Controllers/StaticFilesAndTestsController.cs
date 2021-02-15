namespace BoroServer.ConsoleTestApp.Controllers
{
    using System.IO;
    using System.Text;

    using BoroServer.HTTP;
    using BoroServer.MvcFramework;
    using BoroServer.ConsoleTestApp.Validations;

    public class StaticFilesAndTestsController : Controller
    {
        public HttpResponse ImageSendTest(HttpRequest request)
        {
            var image = @"..\..\..\wwwroot\1.jpg";
            var imageAsByte = File.ReadAllBytes(image);
            var response = new HttpResponse(imageAsByte, "image/jpeg");

            return response;
        }

        public HttpResponse Favicon(HttpRequest request)
        {
            var favicon = File.ReadAllBytes(@"..\..\..\wwwroot\favicon.ico");
            var response = new HttpResponse(favicon, "image/vnd.microsoft.icon");

            return response;
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
    }
}
