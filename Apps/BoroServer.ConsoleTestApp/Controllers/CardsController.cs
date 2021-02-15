namespace BoroServer.ConsoleTestApp.Controllers
{
    using BoroServer.HTTP;
    using BoroServer.MvcFramework;

    public class CardsController : Controller
    {
        public HttpResponse Add(HttpRequest request)
        {
            return this.View();
        }

        public HttpResponse All(HttpRequest request)
        {
            return this.View();
        }

        public HttpResponse Collection(HttpRequest request)
        {
            return this.View();
        }
    }
}
