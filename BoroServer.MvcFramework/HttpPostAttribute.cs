namespace BoroServer.MvcFramework
{
    using BoroServer.HTTP;

    public class HttpPostAttribute : BaseHttpAttribute
    {
        public HttpPostAttribute()
        {
        }

        public HttpPostAttribute(string url)
        {
            this.Url = url;
        }

        public override HttpMethod Method => HttpMethod.POST;
    }
}
