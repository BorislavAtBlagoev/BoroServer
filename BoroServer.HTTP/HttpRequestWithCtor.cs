namespace BoroServer.HTTP
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class HttpRequestWithCtor
    {
        private bool isHeader = true;
        private readonly string[] lines;
        private readonly string headerLine;
        private readonly string[] headerParts;
        private readonly StringBuilder sb;

        public HttpRequestWithCtor(string requestString)
        {
            this.sb = new StringBuilder();
            this.Headers = new HashSet<Header>();
            this.Cookies = new HashSet<Cookie>();

            this.lines = requestString.Split(new string[] { HttpConstants.NewLine },
                StringSplitOptions.None);

            this.headerLine = lines[0];
            this.headerParts = this.headerLine.Split(' ');
            this.Method = this.headerParts[0];
            this.Path = this.headerParts[1];
            this.HttpVersion = this.headerParts[2];

            for (int i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(this.lines[i]))
                {
                    this.isHeader = false;
                    continue;
                }

                if (this.isHeader)
                {
                    this.Headers.Add(new Header(this.lines[i]));
                }
                else
                {
                    sb.Append(lines[i]).ToString();
                }
            }

            if (this.Headers.Any(x => x.Name == HttpConstants.RequestCookieHeader))
            {
                var cookiesAsString = this.Headers.FirstOrDefault(x => x.Name == HttpConstants.RequestCookieHeader).Value;
                var cookies = cookiesAsString.Split(new string[] { "; " },
                    StringSplitOptions.RemoveEmptyEntries);

                foreach (var cookie in cookies)
                {
                    this.Cookies.Add(new Cookie(cookie));
                }
            }

            this.Body = this.sb.ToString();
        }

        public string Path { get; set; }

        public string Method { get; set; }

        public string HttpVersion { get; set; }

        public ICollection<Header> Headers { get; set; }

        public ICollection<Cookie> Cookies { get; set; }

        public string Body { get; set; }
    }
}
