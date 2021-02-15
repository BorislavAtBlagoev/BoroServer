namespace BoroServer.ConsoleTestApp.Validations
{
    using BoroServer.HTTP;
    using System.Linq;

    public static class CookieValidation
    {
        public static HttpResponse CookieSender(HttpRequest request, HttpResponse response, params ResponseCookie[] responseCookies)
        {
            if (request.Cookies.Count == 0)
            {
                foreach (var responseCookie in responseCookies)
                {
                    response.Cookies.Add(responseCookie);
                }
            }
            else
            {
                foreach (var responseCookie in responseCookies)
                {
                    var isSended = request.Cookies.Any(x => x.Name == responseCookie.Name && x.Value == responseCookie.Value);
                    if (!isSended)
                    {
                        response.Cookies.Add(responseCookie);
                    }
                }
            }

            return response;
        }
    }
}
