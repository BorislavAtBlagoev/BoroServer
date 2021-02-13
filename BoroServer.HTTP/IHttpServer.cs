namespace BoroServer.HTTP
{
    using System;
    using System.Threading.Tasks;

    public interface IHttpServer
    {
        Task StartAsync(int port);
        void AddRoute(string path, Func<HttpRequestWithCtor, HttpResponse> action);
    }
}
