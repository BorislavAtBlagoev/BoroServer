namespace BoroServer.ConsoleTestApp.Controllers
{
    using BoroServer.HTTP;
    using BoroServer.MvcFramework;

    public class CardsController : Controller
    {
        public HttpResponse Add(HttpRequest request)
        {
            this.Request = request;
            return this.View();
        }

        public HttpResponse All(HttpRequest request)
        {
            this.Request = request;
            return this.View();
        }

        public HttpResponse Collection(HttpRequest request)
        {
            this.Request = request;
            return this.View();
        }
    }
}
