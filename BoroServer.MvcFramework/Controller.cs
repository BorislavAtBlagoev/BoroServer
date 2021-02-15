namespace BoroServer.MvcFramework
{
    using System.IO;
    using System.Text;
    using System.Runtime.CompilerServices;

    using BoroServer.HTTP;

    public abstract class Controller
    {
        public HttpResponse View([CallerMemberName]string viewPath = null)
        {
            var controllerName = this.GetType().Name.Replace("Controller", string.Empty);
            var layout = File.ReadAllText(MvcFrameworkConstants.LayoutPath);
          
            var body = File.ReadAllText("Views/" + 
                controllerName + "/" +
                viewPath + ".html");

            var html =Encoding.UTF8.GetBytes(layout.Replace("@RenderBody()", body));

            return new HttpResponse(html);
        }
    }
}
