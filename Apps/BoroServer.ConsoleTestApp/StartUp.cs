namespace BoroServer.ConsoleTestApp
{
    using System;
    using System.Collections.Generic;

    using BoroServer.HTTP;
    using BoroServer.MvcFramework;
    using BoroServer.ConsoleTestApp.Controllers;

    public class StartUp : IMvcApplication
    {
        public void Configure(ICollection<Route> routeTable)
        {
            HomeController home = new HomeController();
            UsersController user = new UsersController();
            CardsController card = new CardsController();
            StaticFilesAndTestsController staticFiles = new StaticFilesAndTestsController();

            routeTable.Add(new Route("/", HttpMethod.GET, home.Index));

            routeTable.Add(new Route("/Nikol", HttpMethod.GET, staticFiles.Nikol));
            routeTable.Add(new Route("/favicon.ico", HttpMethod.GET, staticFiles.Favicon));
            routeTable.Add(new Route("/Image", HttpMethod.GET, staticFiles.Image));
            routeTable.Add(new Route("/js/bootstrap.bundle.min.js", HttpMethod.GET, staticFiles.BootstrapJs));
            routeTable.Add(new Route("/js/custom.js", HttpMethod.GET, staticFiles.CustomJs));
            routeTable.Add(new Route("/css/bootstrap.min.css", HttpMethod.GET, staticFiles.BootstrapCss));
            routeTable.Add(new Route("/css/custom.css", HttpMethod.GET, staticFiles.CustomCss));

            routeTable.Add(new Route("/Users/Register", HttpMethod.GET, user.Register));
            routeTable.Add(new Route("/Users/Login", HttpMethod.GET, user.Login));
            routeTable.Add(new Route("/Users/Login", HttpMethod.POST, user.DoLogin));

            routeTable.Add(new Route("/Cards/Add", HttpMethod.GET, card.Add));
            routeTable.Add(new Route("/Cards/All", HttpMethod.GET, card.All));
            routeTable.Add(new Route("/Cards/Collection", HttpMethod.GET, card.Collection));
        }

        public void ConfigureServices()
        {
             //TODO: Implement this configure services.
            throw new NotImplementedException();
        }
    }
}
