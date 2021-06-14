using API.ViewModels;
using Data.Messages;
using Data.Repository;
using Data.Repository.Interfaces;
using System;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using Tests.Integration.Fixtures;
using Tests.Integration.Interfaces;
using Xunit;
using FluentAssertions;
using Data.Models;

namespace Tests.Integration.NewsTests
{
    public abstract class RepositoryControllerTests<TEntity, TKey> : IClassFixture<IntegrationTestFixture>//, IRepositoryControllerTests<TEntity, TKey>
       where TEntity : class, IDatabaseItem<TKey>

    {
        private readonly IntegrationTestFixture _integrationTestFixture;
        public string BaseEndpoint { get; set; } = $"api/v1/{typeof(TEntity).Name}";
        public string CreateEndpoint { get; set; }
        public string UpdateEndpoint { get; set; }
        public string GetEndpoint { get; set; }
        public string ListEndpoint { get; set; }
        public string DeleteEndpoint { get; set; }
        public IRepositoryControllerTestData<TEntity, TKey> TestData { get; set; }

        public RepositoryControllerTests(IntegrationTestFixture integrationTestFixture)
        {
            _integrationTestFixture = integrationTestFixture;
            CreateEndpoint = $"{BaseEndpoint}/create";
            UpdateEndpoint = $"{BaseEndpoint}/update";
            GetEndpoint = $"{BaseEndpoint}";
            ListEndpoint = $"{BaseEndpoint}/list";
            DeleteEndpoint = $"{BaseEndpoint}/delete";
        }
        //[Theory]
        //[MemberData(nameof(IRepositoryControllerTestData.CreatesSuccessfullyTestCases), MemberType = typeof(IRepositoryControllerTestData))]
        [Fact]
        public async void CreatesSuccessfully()
        {
            foreach (var entity in TestData.CreatesSuccessfullyTestCases)
            {
                // Arrange
                var body = JsonContent.Create(entity);

                // Act
                var response = await _integrationTestFixture.HttpClient.PostAsync(CreateEndpoint, body);
                var parsedResponse = await response.Content.ReadFromJsonAsync<BaseResponseModel>();

                // Assert
                Assert.True(response.StatusCode == HttpStatusCode.OK);
                Assert.Null(parsedResponse.Errors);

                // Undo
                response = await _integrationTestFixture.HttpClient.PostAsync(DeleteEndpoint, JsonContent.Create(entity.GetId()));
                parsedResponse = await response.Content.ReadFromJsonAsync<BaseResponseModel>();
                Assert.True(response.StatusCode == HttpStatusCode.OK);
                Assert.Null(parsedResponse.Errors);
            }
        }
        //[Theory]
        //[MemberData(nameof(IRepositoryControllerTestData.DeletesByIdSuccessfullyTestData), MemberType = typeof(IRepositoryControllerTestData))]
        //public async void DeletesByIdSuccessfully(TKey id)
        //{
        //    // Arrange
        //    var entity = await BaseRepository.GetByIdAsync(id);
        //    var body = JsonContent.Create(id);

        //    // Act
        //    var response = await _integrationTestFixture.HttpClient.PostAsync(DeleteEndpoint, body);
        //    var parsedResponse = await response.Content.ReadFromJsonAsync<BaseResponseModel>();

        //    // Assert
        //    Assert.True(response.StatusCode == HttpStatusCode.OK);
        //    Assert.Null(parsedResponse.Errors);

        //    // Undo
        //    response = await _integrationTestFixture.HttpClient.PostAsync(CreateEndpoint, JsonContent.Create(entity));
        //    parsedResponse = await response.Content.ReadFromJsonAsync<BaseResponseModel>();
        //    Assert.True(response.StatusCode == HttpStatusCode.OK);
        //    Assert.Null(parsedResponse.Errors);
        //}
        //[Theory]
        //[MemberData(nameof(IRepositoryControllerTestData.FailsDeletionIfIdDoesNotExistTestData), MemberType = typeof(IRepositoryControllerTestData))]
        //public async void FailsDeletionIfIdDoesNotExist(TKey id)
        //{
        //    // Arrange
        //    var body = JsonContent.Create(id);

        //    // Act
        //    var response = await _integrationTestFixture.HttpClient.PostAsync(DeleteEndpoint, body);
        //    var parsedResponse = await response.Content.ReadFromJsonAsync<BaseResponseModel>();

        //    // Assert
        //    Assert.True(response.StatusCode == HttpStatusCode.InternalServerError);
        //    Assert.True(parsedResponse.Message == RepositoryExceptionMessages.NullObjectInvalidID);
        //}
        //[Theory]
        //[InlineData(100)]
        //public async void ListsAllRecordsSuccessfully(int expectedNumberOfRecords)
        //{
        //    // Arrange
        //    var queryParams = $"?pageSize={expectedNumberOfRecords}";

        //    // Act
        //    var response = await _integrationTestFixture.HttpClient.GetAsync($"{ListEndpoint}{queryParams}");
        //    var parsedResponse = await response.Content.ReadFromJsonAsync<PaginatedListResponse<TEntity>>();

        //    // Assert
        //    Assert.True(response.StatusCode == HttpStatusCode.OK);
        //    Assert.True(parsedResponse.Data.Count == expectedNumberOfRecords, $"Expected value was {expectedNumberOfRecords}, returned value is {parsedResponse.Data.Count}");
        //}
        //[Theory]
        //[InlineData(10, 1, 10)]
        //[InlineData(10, 2, 10)]
        //[InlineData(100, 1, 100)]
        //[InlineData(99, 2, 1)]
        //public async void GetsSpecifiedNumberOfRecordsPerPage(int pageSize, int? pageNumber, int expectedNumberOfRecords)
        //{
        //    // Arrange
        //    var count = (await BaseRepository.GetAllAsync()).Count;
        //    var targetPageNumber = pageNumber == null ? 1 : pageNumber;
        //    var queryParams = $"?pageSize={pageSize}&pageNumber={targetPageNumber}";

        //    // Act
        //    var response = await _integrationTestFixture.HttpClient.GetAsync($"{ListEndpoint}{queryParams}");
        //    var parsedResponse = await response.Content.ReadFromJsonAsync<PaginatedListResponse<TEntity>>();

        //    // Assert
        //    Assert.True(response.StatusCode == HttpStatusCode.OK);
        //    Assert.True(parsedResponse.Data.Count == expectedNumberOfRecords);
        //    Assert.True(parsedResponse.Data.PageIndex == pageNumber);
        //    Assert.True(parsedResponse.Data.Total == count);
        //}
        //[Theory]
        //[MemberData(nameof(IRepositoryControllerTestData.ReportsValidationErrorsWhenCreatingTestData), MemberType = typeof(IRepositoryControllerTestData))]
        //public async void ReportsValidationErrorsWhenCreating(TEntity entity, string[] expectedErrors)
        //{
        //    Assert.True(true); // skip this for now
        //}
        //[Theory]
        //[MemberData(nameof(IRepositoryControllerTestData.ReportsValidationErrorsWhenUpdatingTestData), MemberType = typeof(IRepositoryControllerTestData))]
        //public async void ReportsValidationErrorsWhenUpdating(TEntity entity, string[] expectedErrors)
        //{
        //    Assert.True(true); // skip this for now
        //}
        //[Theory]
        //[MemberData(nameof(IRepositoryControllerTestData.UpdatesSuccessfullyTestData), MemberType = typeof(IRepositoryControllerTestData))]
        //public async void UpdatesSuccessfully(TEntity entity)
        //{
        //    // Arrange
        //    var entityBefore = await BaseRepository.GetByIdAsync(entity.GetId());
        //    var body = JsonContent.Create(entity);

        //    // Act
        //    var response = await _integrationTestFixture.HttpClient.PutAsync(UpdateEndpoint, body);
        //    var afterResponse = await _integrationTestFixture.HttpClient.GetAsync($"{GetEndpoint}/{entity.GetId()}");
        //    var entityAfter = afterResponse.Content.ReadFromJsonAsync<PaginatedListResponse<TEntity>>().Result.Data.Results.SingleOrDefault();

        //    // Assert
        //    Assert.True(response.StatusCode == HttpStatusCode.OK);
        //    entity.Should().BeEquivalentTo(entityAfter);
        //    entity.Should().NotBeEquivalentTo(entityBefore);

        //    // Undo
        //    await BaseRepository.UpdateAsync(entityBefore);
        //    var entityUndo = await BaseRepository.GetByIdAsync(entity.GetId());
        //    entityBefore.Should().BeEquivalentTo(entityUndo);
        //}
    }

    public class NewsRepositoryControllerTests : RepositoryControllerTests<News, Guid>
    {
        public NewsRepositoryControllerTests(IntegrationTestFixture integrationTestFixture) : base(integrationTestFixture)
        {
            TestData = new NewsControllerTestData();
        }
    }
}
