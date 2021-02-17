namespace BoroServer.HTTP
{
    using System.Text;

    public class ResponseCookie : Cookie
    {
        public ResponseCookie(string cookieLine) 
            : base(cookieLine)
        {
            this.Path = "/";
        }

        public ResponseCookie(string name, string value)
            : base(name, value)
        {
            this.Path = "/";
        }

        public bool IsHttpOnly { get; set; }

        public int MaxAge { get; set; }

        public string Path { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{this.Name}={this.Value}; Path={this.Path}; {(this.IsHttpOnly ? "HttpOnly;" : string.Empty)}");
            if (this.MaxAge != 0)
            {
                sb.Append($" Max-Age={this.MaxAge};");
            }

            return sb.ToString();
        }
    }
}
