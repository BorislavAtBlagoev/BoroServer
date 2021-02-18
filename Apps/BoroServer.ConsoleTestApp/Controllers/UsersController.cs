namespace BoroServer.ConsoleTestApp.Controllers
{
    using BoroServer.HTTP;
    using BoroServer.MvcFramework;
    using BoroServer.ConsoleTestApp.Services;
    using BoroServer.ConsoleTestApp.DTO;

    public class UsersController : Controller
    {
        private readonly IUsersService usersService;

        public UsersController()
        {
            this.usersService = new UsersService();
        }

        public HttpResponse Register()
        {
            if (this.IsSignedIn())
            {
                return this.Redirect("/Cards/All");
            }

            return this.View();
        }

        [HttpPost]
        //string username, string email, string password, string confirmPassword
        public HttpResponse Register(UserDTO userDTO)
        {
            if (userDTO.Username == string.Empty ||
                userDTO.Email == string.Empty ||
                userDTO.Password == string.Empty ||
                userDTO.ConfirmPassword == string.Empty)
            {
                return this.Error("Fields are empty");
            }

            if (!this.usersService.IsPasswordMatch(userDTO.Password, userDTO.ConfirmPassword))
            {
                return this.Error("Passwords doesn't match");
            }

            if (!(this.usersService.IsUsernameAvailable(userDTO.Username) &&
                this.usersService.IsEmailAvailable(userDTO.Email)))
            {
                return this.Error("Username or Email are not available.");
            }

            this.usersService.CreateUser(userDTO.Username, userDTO.Email, userDTO.Password);
            return this.Redirect("/Users/Login");
        }

        public HttpResponse Login()
        {
            if (this.IsSignedIn())
            {
                return this.Redirect("/Cards/All");
            }

            return this.View();
        }

        [HttpPost]
        public HttpResponse Login(string username, string password)
        {
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

        public HttpResponse Logout()
        {
            if (this.IsSignedIn())
            {
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
