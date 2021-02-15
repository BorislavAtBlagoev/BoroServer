namespace BoroServer.MvcFramework
{
    using System.Collections.Generic;

    using BoroServer.HTTP;

    public interface IMvcApplication
    {
        void ConfigureServices();

        void Configure(ICollection<Route> routeTable);
    }
}
