namespace BoroServer.ConsoleTestApp
{
    using System.Threading.Tasks;

    using BoroServer.MvcFramework;

    public class Program
    {
        public static async Task Main()
        {
            IMvcApplication application = new StartUp();
            await Host.RunAsync(application);
        }
    }
}
