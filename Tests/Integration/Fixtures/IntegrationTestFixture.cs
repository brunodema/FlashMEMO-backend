using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http;

namespace Tests.Integration.Fixtures
{
    public class IntegrationTestFixture : IDisposable
    {
        public IHost Host { get; set; }
        public HttpClient HttpClient { get; set; }
        public IHostBuilder HostBuilder { get; set; }
        public IntegrationTestFixture()
        {
            // Arrange
            HostBuilder = new HostBuilder()
                .ConfigureWebHost(webHost =>
                {
                    webHost.UseTestServer();
                    webHost.UseStartup<TestStartup>().UseConfiguration(new ConfigurationBuilder().AddJsonFile(@"C:\Users\Bruno\OneDrive\Área de Trabalho\FlashMEMO-backend\API\apisettings.json", optional: false).Build());
                });
            ConfigureInternals();
        }

        public async void ConfigureInternals()
        {
            Host = await HostBuilder.StartAsync();
            HttpClient = Host.GetTestClient();
        }

        public void Dispose()
        {
            Host.Dispose();
            HttpClient.Dispose();
        }
    }
}
