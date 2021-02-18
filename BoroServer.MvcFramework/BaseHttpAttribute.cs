namespace BoroServer.MvcFramework
{
    using System;

    using BoroServer.HTTP;

    public abstract class BaseHttpAttribute : Attribute
    {
        public string Url { get; set; }

        public abstract HttpMethod Method { get; }
    }
}
