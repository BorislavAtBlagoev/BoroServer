namespace BoroServer.ConsoleTestApp.Controllers
{
    using BoroServer.HTTP;
    using BoroServer.MvcFramework;
    using BoroServer.ConsoleTestApp.Services;

    public class UsersController : Controller
    {
        private readonly IUsersService usersService;

        public UsersController()
        {
            this.usersService = new UsersService();
        }

        public HttpResponse Register(HttpRequest request)
        {
            this.Request = request;
            if (this.IsSignedIn())
            {
                return this.Redirect("/Cards/All");
            }

            return this.View();
        }

        public HttpResponse DoRegister(HttpRequest request)
        {
            this.Request = request;
            var username = this.Request.FormData["username"];
            var email = this.Request.FormData["email"];
            var password = this.Request.FormData["password"];
            var confirmPassword = this.Request.FormData["confirmPassword"];

            if (username == string.Empty ||
                email == string.Empty ||
                password == string.Empty ||
                confirmPassword == string.Empty)
            {
                return this.Error("Fields are empty");
            }

            if (!this.usersService.IsPasswordMatch(password, confirmPassword))
            {
                return this.Error("Passwords doesn't match");
            }

            if (!(this.usersService.IsUsernameAvailable(username) &&
                this.usersService.IsEmailAvailable(email)))
            {
                return this.Error("Username or Email are not available.");
            }

            this.usersService.CreateUser(username, email, password);
            return this.Redirect("/Users/Login");
        }

        public HttpResponse Login(HttpRequest request)
        {
            this.Request = request;
            if (this.IsSignedIn())
            {
                return this.Redirect("/Cards/All");
            }

            return this.View();
        }

        public HttpResponse DoLogin(HttpRequest request)
        {
            this.Request = request;
            var username = this.Request.FormData["username"];
            var password = this.Request.FormData["password"];

            if (username == string.Empty || 
                password == string.Empty)
            {
                return this.Error("Fields are empty");
            }

            if (this.usersService.IsUserValid(username, password))
            {
                var userId = this.usersService.GetUserId(username, password);
                this.SignIn(userId);

                return this.Redirect("/Cards/All");
            }

            return this.Error("Incorrect username or password");
        }

        public HttpResponse Logout(HttpRequest request)
        {
            if (this.IsSignedIn())
            {
                this.Request = request;
                this.SignOut();

                return this.Redirect("/");
            }
            else
            {
                return this.Error("You are not logged in!");
            }

        }
    }
}
