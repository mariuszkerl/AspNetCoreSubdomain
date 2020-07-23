using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace AspNetCoreSubdomain.SubdomainsAreaWebSite
{
    public class Program
    {
        public static void Main(string[] args)
        {
             var host = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .UseKestrel()
                .UseIISIntegration()
                .Build();

            host.Run();
        }
    }
}
