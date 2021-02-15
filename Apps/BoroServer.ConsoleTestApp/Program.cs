namespace BoroServer.ConsoleTestApp
{
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using BoroServer.MvcFramework;
    using BoroServer.ConsoleTestApp.Controllers;

    public class Program
    {
        public static async Task Main()
        {
            HomeController home = new HomeController();
            UserController user = new UserController();
            CardController card = new CardController();
            StaticFilesAndTestsController staticFiles = new StaticFilesAndTestsController();

            HashSet<Route> routeTable = new HashSet<Route>();

            routeTable.Add(new Route("/", home.Index));

            routeTable.Add(new Route("/Nikol", staticFiles.Nikol));
            routeTable.Add(new Route("/favicon.ico", staticFiles.Favicon));
            routeTable.Add(new Route("/Image", staticFiles.ImageSendTest));

            routeTable.Add(new Route("/Users/Register", user.Register));
            routeTable.Add(new Route("/Users/Login", user.Login));

            routeTable.Add(new Route("/Cards/Add", card.Add));
            routeTable.Add(new Route("/Cards/All", card.All));
            routeTable.Add(new Route("/Cards/Collection", card.Collection));

            await Host.RunAsync(routeTable);
        }
    }
}
