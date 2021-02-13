namespace BoroServer.HTTP
{
    using System.Collections.Generic;

    public class HttpRequest
    {
        public HttpRequest()
        {
            this.Headers = new HashSet<Header>();
            this.Cookies = new HashSet<Cookie>();
        }

        public string Path { get; set; }

        public string Method { get; set; }

        public string HttpVersion { get; set; }

        public string Body { get; set; }

        public ICollection<Header> Headers { get; }

        public ICollection<Cookie> Cookies { get; }
    }
}
