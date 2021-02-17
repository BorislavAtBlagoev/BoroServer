namespace BoroServer.ConsoleTestApp.Controllers
{
    using BoroServer.HTTP;
    using BoroServer.MvcFramework;
    using BoroServer.ConsoleTestApp.Data;

    public class CardsController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public CardsController()
        {
            this.dbContext = new ApplicationDbContext();
        }

        public HttpResponse Add(HttpRequest request)
        {
            return this.View();
        }

        public HttpResponse All(HttpRequest request)
        {
            this.Request = request;
            return this.View();
        }

        public HttpResponse Collection(HttpRequest request)
        {
            return this.View();
        }
    }
}
