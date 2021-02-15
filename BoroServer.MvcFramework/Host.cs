namespace BoroServer.MvcFramework
{
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using BoroServer.HTTP;

    public static class Host
    {
        public static async Task RunAsync(IMvcApplication application, int port = 80)
        {
            ICollection<Route> routeTable = new HashSet<Route>();
            application.Configure(routeTable);

            IHttpServer server = new HttpServer(routeTable);
            await server.StartAsync(port);
        }
    }
}
