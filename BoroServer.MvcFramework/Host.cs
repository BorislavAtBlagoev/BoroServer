namespace BoroServer.MvcFramework
{
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using BoroServer.HTTP;

    public static class Host
    {
        public static async Task RunAsync(ICollection<Route> routes, int port = 80)
        {
            IHttpServer server = new HttpServer();

            foreach (var route in routes)
            {
                server.AddRoute(route.Path, route.Action);
            }

            await server.StartAsync(port);
        }
    }
}
