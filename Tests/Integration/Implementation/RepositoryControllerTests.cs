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
using Xunit.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tests.Integration.TestData;
using Data.Models.Implementation;

namespace Tests.Integration.Implementation
{
    public abstract class RepositoryControllerTests<TEntity, TKey> : IClassFixture<IntegrationTestFixture>, IRepositoryControllerTests<TEntity, TKey>
       where TEntity : class, IDatabaseItem<TKey>

    {
        protected readonly IntegrationTestFixture _integrationTestFixture;
        protected readonly ITestOutputHelper Output;
        public string BaseEndpoint { get; set; } = $"api/v1/{typeof(TEntity).Name}";
        public string CreateEndpoint { get; set; }
        public string UpdateEndpoint { get; set; }
        public string GetEndpoint { get; set; }
        public string ListEndpoint { get; set; }
        public string DeleteEndpoint { get; set; }
        public string SearchEndpoint { get; set; }
        public IRepositoryControllerTestData<TEntity, TKey> TestData { get; set; }

        /// <summary>
        /// This method returns a new IRepositoryControllerTestData object containing the tests to be run for each RepositoryController test category (create, read, update, delete, etc).
        /// </summary>
        public abstract IRepositoryControllerTestData<TEntity, TKey> SetTestData();

        protected async Task<int> GetTotalNumberOfRecordsOnDatabase()
        {
            var queryParams = $"?pageSize={100000}"; // should be large enough, right?
            var response = await _integrationTestFixture.HttpClient.GetAsync($"{ListEndpoint}{queryParams}");
            return response.Content.ReadFromJsonAsync<PaginatedListResponse<TEntity>>().Result.Data.Count;
        }

        protected async Task<IEnumerable<TEntity>> GetAllObjectsOnDatabase()
        {
            var queryParams = $"?pageSize={100000}"; // should be large enough, right?
            var response = await _integrationTestFixture.HttpClient.GetAsync($"{ListEndpoint}{queryParams}");
            return response.Content.ReadFromJsonAsync<PaginatedListResponse<TEntity>>().Result.Data.Results;
        }

        protected async Task<TEntity> GetEntityById(string id)
        {
            return await _integrationTestFixture.HttpClient.GetAsync($"{GetEndpoint}/{id}").Result.Content.ReadFromJsonAsync<TEntity>();
        }

        protected async void RunAndReportResults<TTestInputData>(IEnumerable<TTestInputData> vs, Func<TTestInputData, Task> func)
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
            SearchEndpoint = $"{BaseEndpoint}/search";

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
                var targetPageNumber = testData.PageNumber <= 0? 1 : testData.PageNumber;
                var queryParams = $"?pageSize={testData.PageSize}&pageNumber={targetPageNumber}";

                // Act
                var response = await _integrationTestFixture.HttpClient.GetAsync($"{ListEndpoint}{queryParams}");
                var parsedResponse = await response.Content.ReadFromJsonAsync<PaginatedListResponse<TEntity>>();

                // Assert
                Assert.True(response.StatusCode == HttpStatusCode.OK);
                Assert.True(parsedResponse.Data.PageIndex == testData.PageNumber);

                // rework this in the future
                if (testData.PageNumber > parsedResponse.Data.TotalPages)
                {
                    Assert.True(parsedResponse.Data.Count == 0);
                }
                else if (testData.PageNumber == parsedResponse.Data.TotalPages)
                {
                    Assert.True(parsedResponse.Data.Count == parsedResponse.Data.Total - (parsedResponse.Data.PageIndex - 1) * testData.PageSize);
                }
                else
                {
                    Assert.True(parsedResponse.Data.Count == testData.PageSize);
                }
            });
        }
        [Fact]
        public void ReportsValidationErrorsWhenCreating()
        {
            RunAndReportResults(TestData.ReportsValidationErrorsWhenCreatingTestData, async testData =>
            {
                // Arrange
                var body = JsonContent.Create(testData.Entiy);

                // Act
                var response = await _integrationTestFixture.HttpClient.PostAsync(CreateEndpoint, body);
                var parsedResponse = await response.Content.ReadFromJsonAsync<BaseResponseModel>();

                // Assert
                Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
                Assert.True(parsedResponse.Message == testData.Message);
                parsedResponse.Errors.Should().BeEquivalentTo(testData.Errors);
            });
        }
        [Fact]
        public void ReportsValidationErrorsWhenUpdating()
        {
            RunAndReportResults(TestData.ReportsValidationErrorsWhenUpdatingTestData, async testData =>
            {
                // Arrange
                var body = JsonContent.Create(testData.Entiy);

                // Act
                var response = await _integrationTestFixture.HttpClient.PutAsync(UpdateEndpoint, body);
                var parsedResponse = await response.Content.ReadFromJsonAsync<BaseResponseModel>();

                // Assert
                Assert.True(response.StatusCode == HttpStatusCode.InternalServerError);
                Assert.True(parsedResponse.Message == testData.Message);
                parsedResponse.Errors.Should().BeEquivalentTo(testData.Errors);
            });
        }

        [Fact]
        public abstract void ShouldSearchRecordsAppropriately();

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

                // Had to add this tolerance so errors regarding <= 500ms time differences are not reported.
                entityBefore.Should().BeEquivalentTo(entityUndo, options => options.Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, TimeSpan.FromMilliseconds(500))).WhenTypeIs<DateTime>());
            });
        }
    }

    public class NewsRepositoryControllerTests : RepositoryControllerTests<News, Guid>
    {
        public NewsRepositoryControllerTests(IntegrationTestFixture integrationTestFixture, ITestOutputHelper output) : base(integrationTestFixture, output) { }

        public override IRepositoryControllerTestData<News, Guid> SetTestData()
        {
            return new NewsControllerTestData();
        }

        public override void ShouldSearchRecordsAppropriately()
        {
            RunAndReportResults(TestData.ShouldSearchRecordsAppropriately, async testData =>
            {
                // Arrange
                var referenceElements = GetAllObjectsOnDatabase().Result.AsQueryable();
                var referenceVectorSize = referenceElements.Count();

                if (testData.SortOptions != null) referenceElements = testData.SortOptions.GetSortedResults(referenceElements).AsQueryable();
                if (testData.FilterOptions != null) referenceElements = testData.FilterOptions.GetFilteredResults(referenceElements).AsQueryable();

                var totalPages = Math.Ceiling((decimal)referenceVectorSize / (decimal)testData.PageSize);
                var currentPage = 1;

                // Act
                while (currentPage <= totalPages)
                {
                    var queryParams = $"?pageSize={testData.PageSize}" +
                    $"&pageNumber={currentPage}" +
                    $"&sortType={testData.SortOptions.SortType}" +
                    $"&columnToSort={testData.SortOptions.ColumnToSort}";

                    // this attrocity over here takes care of concatenating the query parameters. Will fix this in the future
                    if (testData.FilterOptions != null)
                    {
                        if (testData.FilterOptions.Content != null) queryParams = String.Concat(queryParams, $"&Content={testData.FilterOptions.Content}");
                        if (testData.FilterOptions.Subtitle != null) queryParams = String.Concat(queryParams, $"&Subtitle={testData.FilterOptions.Subtitle}");
                        if (testData.FilterOptions.Title != null) queryParams = String.Concat(queryParams, $"&Title={testData.FilterOptions.Title}");
                        if (testData.FilterOptions.FromDate != null) queryParams = String.Concat(queryParams, $"&FromDate={testData.FilterOptions.FromDate.Value.ToString("yyyy-MM-dd")}");
                        if (testData.FilterOptions.ToDate != null) queryParams = String.Concat(queryParams, $"&ToDate={testData.FilterOptions.ToDate.Value.ToString("yyyy-MM-dd")}");
                    }
                    
                    var response = await _integrationTestFixture.HttpClient.GetAsync($"{SearchEndpoint}{queryParams}");
                    var parsedResponse = await response.Content.ReadFromJsonAsync<PaginatedListResponse<News>>();
                    var returnedElements = parsedResponse.Data.Results;

                    // Assert
                    var comparisonVector = referenceElements.Skip(testData.PageSize * (currentPage - 1)).Take(testData.PageSize);
                    returnedElements.Should().BeEquivalentTo(comparisonVector);

                    ++currentPage;
                }
            });
        }
    }
}
