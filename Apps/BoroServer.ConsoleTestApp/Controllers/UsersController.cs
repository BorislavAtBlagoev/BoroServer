namespace BoroServer.ConsoleTestApp.Controllers
{
    using BoroServer.HTTP;
    using BoroServer.MvcFramework;

    public class UsersController : Controller
    {
        public HttpResponse Register(HttpRequest request)
        {
            return this.View();
        }

        public HttpResponse Login(HttpRequest request)
        {
            return this.View();
        }

        public HttpResponse DoLogin(HttpRequest request)
        {
            return this.Redirect("/");
        }
    }
}
