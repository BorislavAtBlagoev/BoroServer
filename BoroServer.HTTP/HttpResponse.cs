namespace BoroServer.HTTP
{
    using System.Collections.Generic;
    using System.Text;

    public class HttpResponse
    {
        public HttpResponse(byte[] body, string contentType = "text/html", StatusCode statusCode = StatusCode.Ok)
        {
            this.Body = body;
            this.StatusCode = statusCode;
            this.Headers = new HashSet<Header>();
            this.Cookies = new HashSet<ResponseCookie>();
            this.Headers.Add(new Header($"Content-Type: {contentType}"));
            this.Headers.Add(new Header($"Content-Lenght: {this.Body?.Length ?? 0}"));
        }

        public StatusCode StatusCode { get; set; }

        public ICollection<Header> Headers { get; set; }

        public ICollection<ResponseCookie> Cookies { get; set; }

        public byte[] Body { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"HTTP/1.1 {(int)this.StatusCode} {this.StatusCode}" + HttpConstants.NewLine);
            sb.Append("Server: BoroServer 2021" + HttpConstants.NewLine);
            foreach (var header in this.Headers)
            {
                sb.Append(header.ToString() + HttpConstants.NewLine);
            }

            foreach (var cookie in this.Cookies)
            {
                sb.Append("Set-Cookie: " + cookie.ToString() + HttpConstants.NewLine);
            }

            sb.Append(HttpConstants.NewLine);

            return sb.ToString();
        }
    }
}
