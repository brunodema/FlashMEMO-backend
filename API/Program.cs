using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                    .ConfigureAppConfiguration(configureDelegate => {
                        configureDelegate.AddJsonFile("apisettings.json"); // contains information about the APIs used in FlashMEMO
                        configureDelegate.AddJsonFile("dbsettings.json"); // contains information on defautl settings for the app's DB context
                    });
                });
    }
}
