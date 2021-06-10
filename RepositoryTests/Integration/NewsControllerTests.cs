using API.ViewModels;
using Data.Messages;
using Data.Models;
using Data.Models.Interfaces;
using Data.Repository;
using Data.Repository.Interfaces;
using Data.Tools;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Tests.Integration.Fixtures;
using Tests.Integration.Interfaces;
using Xunit;

namespace Tests.Integration
{
    public class NewsControllerTests : IClassFixture<IntegrationTestFixture>, IRepositoryControllerTests<News, Guid>
    {
        private readonly IntegrationTestFixture _integrationTestFixture;
        public string BaseEndpoint { get; set; } = "api/v1/news";
        public string CreateEndpoint { get; set; }
        public string UpdateEndpoint { get; set; }
        public string GetEndpoint { get; set; }
        public string ListEndpoint { get; set; }
        public string DeleteEndpoint { get; set; }
        public IBaseRepository<News, Guid> BaseRepository { get; set; }

        public NewsControllerTests(IntegrationTestFixture integrationTestFixture)
        {
            _integrationTestFixture = integrationTestFixture;
            CreateEndpoint = $"{BaseEndpoint}/create";
            UpdateEndpoint = $"{BaseEndpoint}/update";
            GetEndpoint = $"{BaseEndpoint}/get";
            ListEndpoint = $"{BaseEndpoint}/list";
            DeleteEndpoint = $"{BaseEndpoint}/delete";

            BaseRepository = (NewsRepository)this._integrationTestFixture.Host.Services.GetService(typeof(NewsRepository));

        }
        public class CreatesSuccessfullyTestData
        {
            public static IEnumerable<object[]> TestCases
            {
                get
                {
                    yield return new object[] {
                        new News {
                            NewsID = Guid.NewGuid(),
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
        public async void CreatesSuccessfully(News entity)
        {
            // Arrange
            var body = JsonContent.Create(entity);

            // Act
            var response = await _integrationTestFixture.HttpClient.PostAsync(CreateEndpoint, body);
            var parsedResponse = await response.Content.ReadFromJsonAsync<BaseResponseModel>();

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.OK);
            Assert.Null(parsedResponse.Errors);

            var a = await BaseRepository.GetByIdAsync(entity.NewsID);

            // Undo
            response = await _integrationTestFixture.HttpClient.PostAsync(DeleteEndpoint, JsonContent.Create(entity.NewsID));
            parsedResponse = await response.Content.ReadFromJsonAsync<BaseResponseModel>();
            Assert.True(response.StatusCode == HttpStatusCode.OK);
            Assert.Null(parsedResponse.Errors);
        }
        public class DeletesByIdSuccessfullyTestData
        {
            public static IEnumerable<object[]> TestCases
            {
                get
                {
                    yield return new object[] {
                        new Guid("5CDA2C98-98D7-0341-0D7F-5F634136DBE3")
                    };
                }
            }
        }
        [Theory]
        [MemberData(nameof(DeletesByIdSuccessfullyTestData.TestCases), MemberType = typeof(DeletesByIdSuccessfullyTestData))]
        public async void DeletesByIdSuccessfully(Guid id)
        {
            // Arrange
            var entity = await BaseRepository.GetByIdAsync(id);
            var body = JsonContent.Create(id);

            // Act
            var response = await _integrationTestFixture.HttpClient.PostAsync(DeleteEndpoint, body);
            var parsedResponse = await response.Content.ReadFromJsonAsync<BaseResponseModel>();

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.OK);
            Assert.Null(parsedResponse.Errors);

            // Undo
            response = await _integrationTestFixture.HttpClient.PostAsync(CreateEndpoint, JsonContent.Create(entity));
            parsedResponse = await response.Content.ReadFromJsonAsync<BaseResponseModel>();
            Assert.True(response.StatusCode == HttpStatusCode.OK);
            Assert.Null(parsedResponse.Errors);
        }
        public class FailsDeletionIfIdDoesNotExistTestData
        {
            public static IEnumerable<object[]> TestCases
            {
                get
                {
                    yield return new object[] {
                        new Guid("00000000-0000-0000-0000-000000000000")
                    };
                }
            }
        }
        [Theory]
        [MemberData(nameof(FailsDeletionIfIdDoesNotExistTestData.TestCases), MemberType = typeof(FailsDeletionIfIdDoesNotExistTestData))]
        public async void FailsDeletionIfIdDoesNotExist(Guid id)
        {
            // Arrange
            var body = JsonContent.Create(id);

            // Act
            var response = await _integrationTestFixture.HttpClient.PostAsync(DeleteEndpoint, body);
            var parsedResponse = await response.Content.ReadFromJsonAsync<BaseResponseModel>();

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.InternalServerError);
            Assert.True(parsedResponse.Message == RepositoryExceptionMessages.NullObjectInvalidID);
        }
        [Theory]
        [InlineData(100)]
        public async void ListsAllRecordsSuccessfully(int expectedNumberOfRecords)
        {
            // Arrange
            var queryParams = $"?pageSize={expectedNumberOfRecords}";

            // Act
            var response = await _integrationTestFixture.HttpClient.GetAsync($"{ListEndpoint}{queryParams}");
            var parsedResponse = await response.Content.ReadFromJsonAsync<PaginatedListResponse<News>>();

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.OK);
            Assert.True(parsedResponse.Data.Count == expectedNumberOfRecords);
        }
        [Theory]
        [InlineData(10, 1, 10)]
        [InlineData(10, 2, 10)]
        [InlineData(100, 1, 100)]
        [InlineData(99, 2, 1)]
        public async void GetsSpecifiedNumberOfRecordsPerPage(int pageSize, int? pageNumber, int expectedNumberOfRecords)
        {
            // Arrange
            var count = (await BaseRepository.GetAllAsync()).Count;
            var targetPageNumber = pageNumber == null ? 1 : pageNumber;
            var queryParams = $"?pageSize={pageSize}&pageNumber={targetPageNumber}";

            // Act
            var response = await _integrationTestFixture.HttpClient.GetAsync($"{ListEndpoint}{queryParams}");
            var parsedResponse = await response.Content.ReadFromJsonAsync<PaginatedListResponse<News>>();

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.OK);
            Assert.True(parsedResponse.Data.Count == expectedNumberOfRecords);
            Assert.True(parsedResponse.Data.PageIndex == pageNumber);
            Assert.True(parsedResponse.Data.Total == count);
        }
        public class ReportsValidationErrorsWhenCreatingTestData
        {
            public static IEnumerable<object[]> TestCases
            {
                get
                {
                    yield return new object[] {
                        new News {
                            NewsID = Guid.NewGuid(),
                            Title = "Test News",
                            Subtitle = "This is a test news",
                            Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Vel fringilla est ullamcorper eget nulla facilisi etiam dignissim. Orci sagittis eu volutpat odio facilisis mauris sit amet massa. Tincidunt vitae semper quis lectus nulla. Accumsan tortor posuere ac ut consequat semper viverra. Dictum non consectetur a erat. Tellus molestie nunc non blandit massa enim. Mauris a diam maecenas sed. Viverra aliquet eget sit amet tellus cras. A pellentesque sit amet porttitor eget.",
                            CreationDate = DateTime.Now,
                            LastUpdated = DateTime.Now
                        },
                        new string[]{} // once char limits for title/subtitle/content and datetime checks are implemented, come back to this method
                    };
                }
            }
        }
        [Theory]
        [MemberData(nameof(ReportsValidationErrorsWhenCreatingTestData.TestCases), MemberType = typeof(ReportsValidationErrorsWhenCreatingTestData))]
        public async void ReportsValidationErrorsWhenCreating(News entity, string[] expectedErrors)
        {
            Assert.True(true); // skip this for now
        }

        public void ReportsValidationErrorsWhenUpdating(News entity, string[] expectedErrors)
        {
            throw new NotImplementedException();
        }

        public void UpdatesSuccessfully(News entity)
        {
            throw new NotImplementedException();
        }
    }
}
