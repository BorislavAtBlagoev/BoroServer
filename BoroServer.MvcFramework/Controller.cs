namespace BoroServer.MvcFramework
{
    using System.IO;
    using System.Text;
    using System.Runtime.CompilerServices;

    using BoroServer.HTTP;
    using BoroServer.MvcFramework.ViewEngine;

    public abstract class Controller
    {
        private const string UserIdSessionName = "UserId";
        private readonly IViewEngine viewEngine;

        protected Controller()
        {
            this.viewEngine = new BoroViewEngine();
            //this.Request = HttpRequestParser.Parse();
        }

        public HttpRequest Request { get; set; }

        protected HttpResponse View(object viewModel = null, [CallerMemberName] string viewPath = null)
        {
            var controllerName = this.GetType().Name.Replace("Controller", string.Empty);
            var body = File.ReadAllText("Views/" +
                controllerName + "/" +
                viewPath + ".html");
            body = this.viewEngine.GetHtml(body, viewModel, this.GetUserById());
            var html = Encoding.UTF8.GetBytes(this.PutBodyInLayout(body, viewModel));
            return new HttpResponse(html);
        }

        protected HttpResponse Redirect(string path)
        {
            HttpResponse response = new HttpResponse(StatusCode.Redirect);
            response.Headers.Add(new Header($"Location: {path}"));
            return response;
        }

        protected HttpResponse Error(string errorText)
        {
            var viewContent = $"<div class=\"alert alert-danger\" role=\"alert\">{errorText}</div>";
            var html = Encoding.UTF8.GetBytes(this.PutBodyInLayout(viewContent));
            return new HttpResponse(html);
        }

        protected void SignIn(string userId)
        {
            this.Request.Session[UserIdSessionName] = userId;
        }

        protected void SignOut()
        {
            this.Request.Session[UserIdSessionName] = null;
        }

        protected bool IsSignedIn()
            => this.Request.Session.ContainsKey(UserIdSessionName) &&
            this.Request.Session[UserIdSessionName] != null;

        protected string GetUserById()
            => this.Request.Session.ContainsKey(UserIdSessionName) ?
            this.Request.Session[UserIdSessionName] : null;

        private string PutBodyInLayout(string viewContent, object viewModel = null)
        {
            var layout = File.ReadAllText(MvcFrameworkConstants.LayoutPath);
            layout = layout.Replace("@RenderBody()", "___VIEW_GOES_HERE___");
            layout = this.viewEngine.GetHtml(layout, viewModel, this.GetUserById());
            var responseHtml = layout.Replace("___VIEW_GOES_HERE___", viewContent);
            return responseHtml;
        }

    }
}
