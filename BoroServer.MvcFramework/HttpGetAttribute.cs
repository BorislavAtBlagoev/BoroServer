namespace BoroServer.MvcFramework
{
    using BoroServer.HTTP;

    public class HttpGetAttribute : BaseHttpAttribute
    {
        public HttpGetAttribute()
        {
        }

        public HttpGetAttribute(string url)
        {
            this.Url = url;
        }

        public override HttpMethod Method => HttpMethod.GET;
    }
}
