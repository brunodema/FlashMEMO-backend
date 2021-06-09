using API.ViewModels;
using Data.Models;
using Data.Models.Interfaces;
using Data.Repository;
using Data.Repository.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
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
        public string DeleteEndpoint { get; set; }
        public IBaseRepository<News, Guid> BaseRepository { get; set; }

        public NewsControllerTests(IntegrationTestFixture integrationTestFixture)
        {
            _integrationTestFixture = integrationTestFixture;
            CreateEndpoint = $"{BaseEndpoint}/create";
            UpdateEndpoint = $"{BaseEndpoint}/update";
            GetEndpoint = $"{BaseEndpoint}/get";
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

        public void DeletesByIdSuccessfully(Guid guid)
        {
            throw new NotImplementedException();
        }
        public class DeletesSuccessfullyTestData
        {
            public static IEnumerable<object[]> TestCases
            {
                get
                {
                    yield return new object[] {
                        new News {
                            NewsID = Guid.Parse("5CDA2C98-98D7-0341-0D7F-5F634136DBE3"),
                            Title = "ut",
                            Subtitle =  "ut lacus. Nulla tincidunt, neque vitae",
                            ThumbnailPath = "assets/features/flashmemo_dummy3.jpg",
                            Content = "nec luctus felis purus ac tellus. Suspendisse sed dolor. Fusce mi lorem, vehicula et, rutrum eu, ultrices sit amet, risus. Donec nibh enim, gravida sit amet, dapibus id, blandit at, nisi. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Proin vel nisl. Quisque fringilla euismod enim. Etiam gravida molestie arcu. Sed eu nibh vulputate mauris sagittis placerat. Cras dictum ultricies ligula. Nullam enim. Sed nulla ante, iaculis nec, eleifend non, dapibus rutrum, justo. Praesent luctus.",
                            CreationDate = DateTime.Parse("2021-09-16 13:40:24"),
                            LastUpdated = DateTime.Parse("2020-10-21 04:46:45")
                        }
                    };
                }
            }
        }
        [Theory]
        [MemberData(nameof(DeletesSuccessfullyTestData.TestCases), MemberType = typeof(DeletesSuccessfullyTestData))]
        public async void DeletesSuccessfully(News entity)
        {
            // Arrange
            var body = JsonContent.Create(entity.NewsID);

            // Act
            var response = await _integrationTestFixture.HttpClient.PostAsync(DeleteEndpoint, body);
            var parsedResponse = await response.Content.ReadFromJsonAsync<BaseResponseModel>();
            Assert.True(response.StatusCode == HttpStatusCode.OK);
            Assert.Null(parsedResponse.Errors);

            // Undo
            response = await _integrationTestFixture.HttpClient.PostAsync(CreateEndpoint, JsonContent.Create(entity));
            parsedResponse = await response.Content.ReadFromJsonAsync<BaseResponseModel>();
            Assert.True(response.StatusCode == HttpStatusCode.OK);
            Assert.Null(parsedResponse.Errors);
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

        public void ReportsValidationErrorsWhenCreating(News entity, string[] expectedErrors)
        {
            throw new NotImplementedException();
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
