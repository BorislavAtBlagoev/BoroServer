namespace BoroServer.MvcFramework
{
    using System.IO;
    using System.Text;
    using System.Runtime.CompilerServices;

    using BoroServer.HTTP;
    using BoroServer.MvcFramework.ViewEngine;

    public abstract class Controller
    {
        private readonly IViewEngine viewEngine;

        protected Controller()
        {
            this.viewEngine = new BoroViewEngine();
        }

        public HttpResponse View(object viewModel = null, [CallerMemberName]string viewPath = null)
        {
            var controllerName = this.GetType().Name.Replace("Controller", string.Empty);

            var layout = File.ReadAllText(MvcFrameworkConstants.LayoutPath);
            layout = layout.Replace("@RenderBody()", "___VIEW_GOES_HERE___");
            layout = this.viewEngine.GetHtml(layout, viewModel);
          
            var body = File.ReadAllText("Views/" + 
                controllerName + "/" +
                viewPath + ".html");
            body = this.viewEngine.GetHtml(body, viewModel);

            var html = Encoding.UTF8.GetBytes(layout.Replace("___VIEW_GOES_HERE___", body));

            return new HttpResponse(html);
        }

        public HttpResponse Redirect(string path)
        {
            HttpResponse response = new HttpResponse(StatusCode.Redirect);
            response.Headers.Add(new Header($"Location: {path}"));
            return response;
        }
    }
}
