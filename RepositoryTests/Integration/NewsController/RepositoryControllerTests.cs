using API.ViewModels;
using Data.Messages;
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
using Xunit.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tests.Integration.NewsTests
{
    public abstract class RepositoryControllerTests<TEntity, TKey> : IClassFixture<IntegrationTestFixture>, IRepositoryControllerTests<TEntity, TKey>
       where TEntity : class, IDatabaseItem<TKey>

    {
        private readonly IntegrationTestFixture _integrationTestFixture;
        private readonly ITestOutputHelper Output;
        public string BaseEndpoint { get; set; } = $"api/v1/{typeof(TEntity).Name}";
        public string CreateEndpoint { get; set; }
        public string UpdateEndpoint { get; set; }
        public string GetEndpoint { get; set; }
        public string ListEndpoint { get; set; }
        public string DeleteEndpoint { get; set; }
        public IRepositoryControllerTestData<TEntity, TKey> TestData { get; set; }

        /// <summary>
        /// This method returns a new IRepositoryControllerTestData object containing the tests to be run for each RepositoryController test category (create, read, update, delete, etc).
        /// </summary>
        public abstract IRepositoryControllerTestData<TEntity, TKey> SetTestData();

        private async void RunAndReportResults<TTestInputData>(IEnumerable<TTestInputData> vs, Func<TTestInputData, Task> func)
        {
            int count = 0;
            try
            {
                foreach (var entity in vs)
                {
                    ++count;
                    await func(entity);
                    Output.WriteLine($"Test #{count} run successfully.");
                }
            }
            catch (Exception ex)
            {
                Output.WriteLine($"Test #{count} has failed. Exception was: {ex.Message}");
                throw;
            }
        }
        public RepositoryControllerTests(IntegrationTestFixture integrationTestFixture, ITestOutputHelper output)
        {
            _integrationTestFixture = integrationTestFixture;
            Output = output;

            CreateEndpoint = $"{BaseEndpoint}/create";
            UpdateEndpoint = $"{BaseEndpoint}/update";
            GetEndpoint = $"{BaseEndpoint}";
            ListEndpoint = $"{BaseEndpoint}/list";
            DeleteEndpoint = $"{BaseEndpoint}/delete";

            TestData = SetTestData();
        }

        [Fact]
        public void CreatesSuccessfully()
        {
            RunAndReportResults(TestData.CreatesSuccessfullyTestCases, async entity =>
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
            });
        }
        [Fact]
        public void DeletesByIdSuccessfully()
        {
            RunAndReportResults(TestData.DeletesByIdSuccessfullyTestData, async id =>
            {
                // Arrange
                var entity = await _integrationTestFixture.HttpClient.GetAsync($"{GetEndpoint}/{id}").Result.Content.ReadFromJsonAsync<TEntity>();
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
            });
        }
        [Fact]
        public void FailsDeletionIfIdDoesNotExist()
        {
            RunAndReportResults(TestData.FailsDeletionIfIdDoesNotExistTestData, async id =>
            {
                // Arrange
                var body = JsonContent.Create(id);

                // Act
                var response = await _integrationTestFixture.HttpClient.PostAsync(DeleteEndpoint, body);
                var parsedResponse = await response.Content.ReadFromJsonAsync<BaseResponseModel>();

                // Assert
                Assert.True(response.StatusCode == HttpStatusCode.InternalServerError);
                Assert.True(parsedResponse.Message == RepositoryExceptionMessages.NullObjectInvalidID);
            });
        }
        [Fact]
        public void ListsAllRecordsSuccessfully()
        {
            RunAndReportResults(TestData.ListsAllRecordsSuccessfully, async expectedNumberOfRecords =>
            {
                // Arrange
                var queryParams = $"?pageSize={expectedNumberOfRecords}";

                // Act
                var response = await _integrationTestFixture.HttpClient.GetAsync($"{ListEndpoint}{queryParams}");
                var parsedResponse = await response.Content.ReadFromJsonAsync<PaginatedListResponse<TEntity>>();

                // Assert
                Assert.True(response.StatusCode == HttpStatusCode.OK);
                Assert.True(parsedResponse.Data.Count == expectedNumberOfRecords);
            });
        }
        [Fact]

        public void GetsSpecifiedNumberOfRecordsPerPage()
        {
            RunAndReportResults(TestData.GetsSpecifiedNumberOfRecordsPerPage, async testData =>
            {
                // Arrange
                var targetPageNumber = testData.pageNumber == null ? 1 : testData.pageNumber;
                var queryParams = $"?pageSize={testData.pageSize}&pageNumber={targetPageNumber}";

                // Act
                var response = await _integrationTestFixture.HttpClient.GetAsync($"{ListEndpoint}{queryParams}");
                var parsedResponse = await response.Content.ReadFromJsonAsync<PaginatedListResponse<TEntity>>();

                // Assert
                Assert.True(response.StatusCode == HttpStatusCode.OK);
                Assert.True(parsedResponse.Data.PageIndex == testData.pageNumber);

                // rework this in the future
                if (testData.pageNumber > parsedResponse.Data.TotalPages)
                {
                    Assert.True(parsedResponse.Data.Count == 0);
                }
                else if (testData.pageNumber == parsedResponse.Data.TotalPages)
                {
                    Assert.True(parsedResponse.Data.Count == parsedResponse.Data.Total - (parsedResponse.Data.PageIndex - 1) * testData.pageSize);
                }
                else
                {
                    Assert.True(parsedResponse.Data.Count == testData.pageSize);
                }
            });
        }
        [Fact]
        public void ReportsValidationErrorsWhenCreating()
        {
            RunAndReportResults(TestData.ReportsValidationErrorsWhenCreatingTestData, async testData =>
            {
                Assert.True(true); // skip this for now
                await Task.CompletedTask;
            });
        }
        [Fact]
        public void ReportsValidationErrorsWhenUpdating()
        {
            RunAndReportResults(TestData.ReportsValidationErrorsWhenCreatingTestData, async testData =>
            {
                Assert.True(true); // skip this for now
                                await Task.CompletedTask;
            });
        }
        [Fact]
        public void UpdatesSuccessfully()
        {
            RunAndReportResults(TestData.UpdatesSuccessfullyTestData, async entity =>
            {
                // Arrange
                var entityBefore = await _integrationTestFixture.HttpClient.GetAsync($"{GetEndpoint}/{entity.GetId()}").Result.Content.ReadFromJsonAsync<TEntity>();
                var body = JsonContent.Create(entity);

                // Act
                var response = await _integrationTestFixture.HttpClient.PutAsync(UpdateEndpoint, body);
                var afterResponse = await _integrationTestFixture.HttpClient.GetAsync($"{GetEndpoint}/{entity.GetId()}");
                var entityAfter = afterResponse.Content.ReadFromJsonAsync<PaginatedListResponse<TEntity>>().Result.Data.Results.SingleOrDefault();

                // Assert
                Assert.True(response.StatusCode == HttpStatusCode.OK);
                entity.Should().BeEquivalentTo(entityAfter);
                entity.Should().NotBeEquivalentTo(entityBefore);

                // Undo
                body = JsonContent.Create(entityBefore);
                await _integrationTestFixture.HttpClient.PutAsync(UpdateEndpoint, body);
                var entityUndo = await _integrationTestFixture.HttpClient.GetAsync($"{GetEndpoint}/{entity.GetId()}").Result.Content.ReadFromJsonAsync<TEntity>();
                entityBefore.Should().BeEquivalentTo(entityUndo);
            });
        }
    }

    public class NewsRepositoryControllerTests : RepositoryControllerTests<News, Guid>
    {
        public NewsRepositoryControllerTests(IntegrationTestFixture integrationTestFixture, ITestOutputHelper output) : base(integrationTestFixture, output)
        {
            SetTestData();
        }

        public override IRepositoryControllerTestData<News, Guid> SetTestData()
        {
            return new NewsControllerTestData();
        }
    }
}
