namespace BoroServer.HTTP
{
    using System.Collections.Generic;

    public class HttpRequest
    {
        public HttpRequest()
        {
            this.Headers = new HashSet<Header>();
            this.Cookies = new HashSet<Cookie>();
            this.Session = new Dictionary<string, string>();
            this.FormData = new Dictionary<string, string>();
            this.QueryData = new Dictionary<string, string>();
        }

        public string Path { get; set; }

        public HttpMethod Method { get; set; }

        public string HttpVersion { get; set; }

        public string Body { get; set; }

        public string QueryString { get; set; }

        public ICollection<Header> Headers { get; }

        public ICollection<Cookie> Cookies { get; }

        public IDictionary<string, string> FormData { get; }

        public IDictionary<string, string> QueryData { get; set; }

        public IDictionary<string, string> Session { get; set; }
    }
}
