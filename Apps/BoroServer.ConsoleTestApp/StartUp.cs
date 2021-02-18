namespace BoroServer.ConsoleTestApp
{
    using System.Collections.Generic;

    using Microsoft.EntityFrameworkCore;

    using BoroServer.HTTP;
    using BoroServer.MvcFramework;
    using BoroServer.ConsoleTestApp.Data;
    using BoroServer.ConsoleTestApp.Services;

    public class StartUp : IMvcApplication
    {
        public void Configure(ICollection<Route> routeTable)
        {
            new ApplicationDbContext().Database.Migrate();
        }

        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.Add<IUsersService, UsersService>();
        }
    }
}
