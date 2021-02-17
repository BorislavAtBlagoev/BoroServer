namespace BoroServer.HTTP
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;

    public static class HttpRequestParser
    {
        public static bool isGeneratedNow ;
        public static IDictionary<string, IDictionary<string, string>> Sessions 
            = new Dictionary<string, IDictionary<string, string>>();

        public static HttpRequest Parse(string requestAsString)
        {
            bool isHeader = true;
            HttpRequest request = new HttpRequest();
            StringBuilder sb = new StringBuilder();

            string[] lines = requestAsString.Split(new string[] { HttpConstants.NewLine },
                StringSplitOptions.None);

            string headerLine = lines[0];
            string[] headerParts = headerLine.Split(' ');

            request.Method = (HttpMethod)Enum.Parse(typeof(HttpMethod), headerParts[0], true);
            request.Path = headerParts[1];
            request.HttpVersion = headerParts[2];

            for (int i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                {
                    isHeader = false;
                    continue;
                }

                if (isHeader)
                {
                    request.Headers.Add(new Header(lines[i]));
                }
                else
                {
                    sb.Append(lines[i]).ToString();
                }

                if (lines[i].Contains("&"))
                {
                    var formDataParts = lines[i].Split('&');
                    foreach (var formDataPart in formDataParts)
                    {
                        var formData = formDataPart.Split(new char[] { '=' }, 2);
                        request.FormData[formData[0]] = formData[1];
                    }
                }
            }

            if (request.Headers.Any(x => x.Name == HttpConstants.RequestCookieHeader))
            {
                string cookiesAsString = request.Headers
                    .FirstOrDefault(x => x.Name == HttpConstants.RequestCookieHeader).Value;
                string[] cookies = cookiesAsString.Split(new string[] { "; " }, 2, StringSplitOptions.RemoveEmptyEntries);

                foreach (var cookie in cookies)
                {
                    request.Cookies.Add(new Cookie(cookie));
                }
            }

            var sessionCookie = request.Cookies.FirstOrDefault(x => x.Name == HttpConstants.SessionCookieName);
            if (sessionCookie == null)
            {
                isGeneratedNow = true;
                var sessionId = Guid.NewGuid().ToString();
                request.Session = new Dictionary<string, string>();
                Sessions.Add(sessionId, request.Session);
                request.Cookies.Add(new Cookie(HttpConstants.SessionCookieName, sessionId));
            }
            else if (!Sessions.ContainsKey(sessionCookie.Value))
            {
                isGeneratedNow = false;
                request.Session = new Dictionary<string, string>();
                Sessions.Add(sessionCookie.Value, request.Session);
            }
            else
            {
                isGeneratedNow = false;
                request.Session = Sessions[sessionCookie.Value];
            }

            request.Body = sb.ToString();

            return request;
        }
    }
}
