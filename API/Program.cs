using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System;
using System.Reflection;

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
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                    .ConfigureAppConfiguration(configureDelegate => {
                        configureDelegate.AddJsonFile("apisettings.json", true); // contains information about the APIs used in FlashMEMO
                        configureDelegate.AddJsonFile("dbsettings.json"); // contains information on defautl settings for the app's DB context
                        configureDelegate.AddJsonFile("emailsettings.json"); // contains information on defautl settings for the app's email services

                        var configuration = configureDelegate.Build();
                        Log.Logger = new LoggerConfiguration()
                            .Enrich.FromLogContext()
                            .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(configuration["ElasticConfiguration:Uri"]))
                            {
                                AutoRegisterTemplate = true,
                                IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name.ToLower()}-{DateTime.UtcNow:yyyy-MM}"
                            })
                            .Enrich.WithProperty("Environment", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"))
                            .ReadFrom.Configuration(configuration)
                            .CreateLogger();
                    });
                });
    }
}
