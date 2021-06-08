using Data.Models;
using Data.Models.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Tests.Integration.Fixtures;
using Tests.Integration.Interfaces;
using Xunit;

namespace Tests.Integration
{
    public class NewsControllerTests : IClassFixture<IntegrationTestFixture>, IRepositoryControllerTests<INews>
    {
        private readonly IntegrationTestFixture _integrationTestFixture;
        public string BaseEndpoint { get; set; } = "api/v1/news";
        public string CreateEndpoint { get; set; }
        public string UpdateEndpoint { get; set; }
        public string GetEndpoint { get; set; }
        public string DeleteEndpoint { get; set; }

        public NewsControllerTests(IntegrationTestFixture integrationTestFixture)
        {
            _integrationTestFixture = integrationTestFixture;
            CreateEndpoint = $"{BaseEndpoint}/create";
            UpdateEndpoint = $"{BaseEndpoint}/update";
            GetEndpoint = $"{BaseEndpoint}/get";
            DeleteEndpoint = $"{BaseEndpoint}/delete";
        }
        public class CreatesSuccessfullyTestData
        {
            public static IEnumerable<object[]> TestCases
            {
                get
                {
                    yield return new object[] {
                        new News {
                            Title = "Test News",
                            Subtitle = "This is a test news",
                            Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Vel fringilla est ullamcorper eget nulla facilisi etiam dignissim. Orci sagittis eu volutpat odio facilisis mauris sit amet massa. Tincidunt vitae semper quis lectus nulla. Accumsan tortor posuere ac ut consequat semper viverra. Dictum non consectetur a erat. Tellus molestie nunc non blandit massa enim. Mauris a diam maecenas sed. Viverra aliquet eget sit amet tellus cras. A pellentesque sit amet porttitor eget.",
                            CreationDate = DateTime.Now,
                            LastUpdated = DateTime.Now
                        }
                    };
                }
            }
        }
        [Theory]
        [MemberData(nameof(CreatesSuccessfullyTestData.TestCases), MemberType = typeof(CreatesSuccessfullyTestData))]
        public async void CreatesSuccessfully(INews entity)
        {
            // Arrange
            var body = JsonContent.Create(entity);

            // Act
            var response = await this._integrationTestFixture.HttpClient.PostAsync("/api/v1/News/create", body);

            // Assert
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.True(responseString == "This is a test");
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
