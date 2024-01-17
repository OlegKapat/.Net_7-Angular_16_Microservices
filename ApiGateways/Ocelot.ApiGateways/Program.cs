using ApiGateways;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Ocelot.ApiGateways
{
    using Microsoft.Extensions.Configuration.Json;
    using Microsoft.Extensions.Hosting;

    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "Ocelot API Gateway";
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(
                    (hostingContext, config) =>
                    {
                        config.AddJsonFile(
                            $"ocelot.{hostingContext.HostingEnvironment.EnvironmentName}.json",
                            true,
                            true
                        );
                    }
                )
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
