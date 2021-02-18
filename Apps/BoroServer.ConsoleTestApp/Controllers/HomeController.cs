namespace BoroServer.ConsoleTestApp.Controllers
{
    using BoroServer.HTTP;
    using BoroServer.MvcFramework;

    public class HomeController : Controller
    {
        [HttpGet("/")]
        public HttpResponse Index()
        {
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
