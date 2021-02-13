namespace BoroServer.HTTP
{
    using System;
    using System.Linq;
    using System.Text;

    public static class HttpRequestParser
    {
        private static bool isHeader = true;

        public static HttpRequest Parse(string requestAsString)
        {
            HttpRequest request = new HttpRequest();
            StringBuilder sb = new StringBuilder();

            string[] lines = requestAsString.Split(new string[] { HttpConstants.NewLine },
                StringSplitOptions.None);

            string headerLine = lines[0];
            string[] headerParts = headerLine.Split(' ');

            request.Method = headerParts[0];
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

            request.Body = sb.ToString();

            return request;
        }
    }
}
