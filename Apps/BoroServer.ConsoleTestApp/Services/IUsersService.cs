namespace BoroServer.ConsoleTestApp.Services
{
    public interface IUsersService
    {
        void CreateUser(string username, string email, string password);

        string GetUserId(string username, string password);

        bool IsEmailAvailable(string email);

        bool IsUsernameAvailable(string username);

        bool IsUserValid(string username, string password);

        bool IsPasswordMatch(string password, string confirmPassword);
    }
}
