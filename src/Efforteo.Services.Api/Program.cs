using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Efforteo.Services.Api
{
    public class Program
    {
        public static void Main(string[] args)
            => CreateWebHostBuilder(args).Build().Run();

        private static IWebHostBuilder CreateWebHostBuilder(string[] args)
            => WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((host, config) => { config.AddJsonFile("ocelot.json"); })
                .UseStartup<Startup>();
    }
}
