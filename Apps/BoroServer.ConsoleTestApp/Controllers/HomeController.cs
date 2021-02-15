namespace BoroServer.ConsoleTestApp.Controllers
{
    using BoroServer.HTTP;
    using BoroServer.MvcFramework;

    public class HomeController : Controller
    {
        public HttpResponse Index(HttpRequest request)
        {
            return this.View();
        }
    }
}
