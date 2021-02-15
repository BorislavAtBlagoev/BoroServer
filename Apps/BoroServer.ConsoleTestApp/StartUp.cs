using BoroServer.ConsoleTestApp.Controllers;
using BoroServer.HTTP;
using BoroServer.MvcFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoroServer.ConsoleTestApp
{
    public class StartUp : IMvcApplication
    {
        public void Configure(ICollection<Route> routeTable)
        {
            HomeController home = new HomeController();
            UsersController user = new UsersController();
            CardsController card = new CardsController();
            StaticFilesAndTestsController staticFiles = new StaticFilesAndTestsController();

            routeTable.Add(new Route("/", home.Index));

            routeTable.Add(new Route("/Nikol", staticFiles.Nikol));
            routeTable.Add(new Route("/favicon.ico", staticFiles.Favicon));
            routeTable.Add(new Route("/Image", staticFiles.Image));
            routeTable.Add(new Route("/js/bootstrap.bundle.min.js", staticFiles.BootstrapJs));
            routeTable.Add(new Route("/js/custom.js", staticFiles.CustomJs));
            routeTable.Add(new Route("/css/bootstrap.min.css", staticFiles.BootstrapCss));
            routeTable.Add(new Route("/css/custom.css", staticFiles.CustomCss));

            routeTable.Add(new Route("/Users/Register", user.Register));
            routeTable.Add(new Route("/Users/Login", user.Login));

            routeTable.Add(new Route("/Cards/Add", card.Add));
            routeTable.Add(new Route("/Cards/All", card.All));
            routeTable.Add(new Route("/Cards/Collection", card.Collection));
        }

        public void ConfigureServices()
        {
            throw new NotImplementedException();
        }
    }
}
