namespace BoroServer.ConsoleTestApp.Controllers
{
    using BoroServer.HTTP;
    using BoroServer.MvcFramework;

    public class UserController : Controller
    {
        public HttpResponse Register(HttpRequest request)
        {
            return this.View(@"Views\Users\Register.html");
        }

        public HttpResponse Login(HttpRequest request)
        {
            return this.View(@"Views\Users\Login.html");
        }
    }
}
