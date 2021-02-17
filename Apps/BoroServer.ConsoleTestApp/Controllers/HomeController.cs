namespace BoroServer.ConsoleTestApp.Controllers
{
    using BoroServer.HTTP;
    using BoroServer.MvcFramework;

    public class HomeController : Controller
    {
        public HttpResponse Index(HttpRequest request)
        {
            this.Request = request;
            if (this.IsSignedIn())
            {
                return this.Redirect("/Cards/All");
            }            
            else
            {
                return this.View();
            }

        }
    }
}
