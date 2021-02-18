namespace BoroServer.ConsoleTestApp.Controllers
{
    using BoroServer.HTTP;
    using BoroServer.MvcFramework;

    public class CardsController : Controller
    {
        public HttpResponse Add()
        {
            return this.View();
        }

        public HttpResponse All()
        {
            return this.View();
        }

        public HttpResponse Collection()
        {
            return this.View();
        }
    }
}
