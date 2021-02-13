namespace BoroServer.HTTP
{
    using System;

    public class Cookie
    {
        private readonly string[] cookieParts;

        public Cookie(string cookieLine)
        {
            this.cookieParts = cookieLine.Split(new char[] { '=' }, 
                2, StringSplitOptions.None);
            this.Name = cookieParts[0];
            this.Value = cookieParts[1];
        }

        public string Name { get; }

        public string Value { get; }

        public override string ToString()
        {
            return $"{this.Name} {this.Value}";
        }
    }
}
