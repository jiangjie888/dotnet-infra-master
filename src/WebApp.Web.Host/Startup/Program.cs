using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace WebApp.Web.Host.Startup
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                //.UseContentRoot(Directory.GetCurrentDirectory())
                //.UseUrls("http://localhost:8888/", "http://localhost:9999/")；
                .Build();
        }
    }
}
