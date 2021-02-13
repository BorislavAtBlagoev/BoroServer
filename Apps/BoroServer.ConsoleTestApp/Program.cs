namespace BoroServer.ConsoleTestApp
{
    using System.Threading.Tasks;

    using BoroServer.HTTP;

    public class Program
    {
        public static async Task Main()
        {
            IHttpServer server = new HttpServer();
            await server.StartAsync(80);
        }
    }
}
