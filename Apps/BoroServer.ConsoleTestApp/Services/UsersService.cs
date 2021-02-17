namespace BoroServer.ConsoleTestApp.Services
{
    using System.Linq;
    using System.Text;
    using System.Security.Cryptography;

    using BoroServer.MvcFramework;
    using BoroServer.ConsoleTestApp.Data;

    public class UsersService : IUsersService
    {
        private readonly ApplicationDbContext dbContext;

        public UsersService()
        {
            this.dbContext = new ApplicationDbContext();
        }

        public void CreateUser(string username, string email, string password)
        {
                var user = new User
                {
                    Username = username,
                    Email = email,
                    Password = ComputeHash(password),
                    Role = IdentityRole.User
                };

                this.dbContext.Users.Add(user);
                this.dbContext.SaveChanges();
        }

        public bool IsEmailAvailable(string email)
        {
            return !this.dbContext.Users
                .Any(x => x.Email.ToLower() == email.ToLower());
        }

        public bool IsUsernameAvailable(string username)
        {
            return !this.dbContext.Users
                .Any(x => x.Username.ToLower() == username.ToLower());
        }

        public bool IsUserValid(string username, string password)
        {
            return this.dbContext.Users
                .Any(x => x.Username.ToLower() == username.ToLower() && x.Password == ComputeHash(password));
        }

        public bool IsPasswordMatch(string password, string confirmPassword) 
            => password == confirmPassword;

        private static string ComputeHash(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            using var hash = SHA512.Create();
            var hashedInputBytes = hash.ComputeHash(bytes);

            var hashedInputStringBuilder = new StringBuilder(128);
            foreach (var b in hashedInputBytes)
            {
                hashedInputStringBuilder.Append(b.ToString("X2"));
            }

            return hashedInputStringBuilder.ToString();
        }
    }
}
