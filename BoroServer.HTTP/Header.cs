namespace BoroServer.HTTP
{
    using System;

    public class Header
    {
        private readonly string[] headerParts;

        public Header(string headerLine)
        {
            this.headerParts = headerLine.Split(new string[] { ": " }, 
                2, StringSplitOptions.None);
            this.Name = this.headerParts[0];
            this.Value = this.headerParts[1];
        }

        public string Name { get; }

        public string Value { get; }

        public override string ToString()
        {
            return $"{this.Name}: {this.Value}";
        }
    }
}
