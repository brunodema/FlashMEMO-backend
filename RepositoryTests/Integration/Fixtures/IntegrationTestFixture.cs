using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http;

namespace Tests.Integration.Fixtures
{
    public abstract class IntegrationTestFixture : IDisposable
    {
        protected IHost Host { get; set; }
        protected HttpClient HttpClient { get; set; }
        protected IntegrationTestFixture()
        {
            // Arrange
            var hostBuilder = new HostBuilder()
                .ConfigureWebHost(webHost =>
                {
                    webHost.UseTestServer();
                    webHost.UseStartup<TestStartup>();
                });
            ConfigureInternals(hostBuilder);
        }

        protected async void ConfigureInternals(IHostBuilder hostBuilder)
        {
            Host = await hostBuilder.StartAsync();
            HttpClient = Host.GetTestClient();
        }

        public void Dispose()
        {
            Host.Dispose();
            HttpClient.Dispose();
        }
    }
}
