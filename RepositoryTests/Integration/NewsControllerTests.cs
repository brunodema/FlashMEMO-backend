using Data.Models.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using Tests.Integration.Fixtures;
using Tests.Integration.Interfaces;
using Xunit;

namespace Tests.Integration
{
    public class NewsControllerTests : IClassFixture<IntegrationTestFixture>, IRepositoryControllerTests<INews>
    {
        [Fact]
        public async Task BasicEndPointTest()
        {
            // Arrange
            var hostBuilder = new HostBuilder()
                .ConfigureWebHost(webHost =>
                {
                        // Add TestServer
                        webHost.UseTestServer();
                    webHost.UseStartup<TestStartup>();
                });

            // Create and start up the host
            var host = await hostBuilder.StartAsync();

            // Create an HttpClient which is setup for the test host
            var client = host.GetTestClient();

            // Act
            var response = await client.GetAsync("/api/v1/News/list");

            // Assert
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.True(responseString == "This is a test");
        }

        public void CreatesSuccessfully(INews entity)
        {
            throw new NotImplementedException();
        }

        public void DeletesByIdSuccessfully(Guid guid)
        {
            throw new NotImplementedException();
        }

        public void DeletesSuccessfully(INews entity)
        {
            throw new NotImplementedException();
        }

        public void FailsDeletionIfIdDoesNotExist(Guid guid)
        {
            throw new NotImplementedException();
        }

        public void GetsAllRecordsSuccessfully(int expectedNumberOfRecords)
        {
            throw new NotImplementedException();
        }

        public void GetsSpecifiedNumberOfPagesAndRecords(int pageSize, int numberOfPages)
        {
            throw new NotImplementedException();
        }

        public void GetsSpecifiedNumberOfRecordsAtMax(int numberOfRecords)
        {
            throw new NotImplementedException();
        }

        public void ReportsValidationErrorsWhenCreating(INews entity, string[] expectedErrors)
        {
            throw new NotImplementedException();
        }

        public void ReportsValidationErrorsWhenUpdating(INews entity, string[] expectedErrors)
        {
            throw new NotImplementedException();
        }

        public void UpdatesSuccessfully(INews entity)
        {
            throw new NotImplementedException();
        }
    }
}
