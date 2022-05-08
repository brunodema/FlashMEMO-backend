using API.ViewModels;
using System.Collections.Generic;
using System.Net.Http.Json;
using Tests.Integration.Fixtures;
using Xunit;
using Xunit.Abstractions;
using FluentAssertions;
using Business.Services.Implementation;
using System.Linq;
using System;

namespace Tests.Integration.Implementation.API
{
    #region Dictionary API
    public class DictionaryAPITests : IClassFixture<IntegrationTestFixture>
    {
        protected readonly IntegrationTestFixture _integrationTestFixture;
        protected readonly ITestOutputHelper _output;

        public string BaseEndpoint { get; set; } = $"api/v1/dict";

        public DictionaryAPITests(IntegrationTestFixture integrationTestFixture, ITestOutputHelper output)
        {
            _integrationTestFixture = integrationTestFixture;
            _output = output;
        }

        public static IEnumerable<object[]> MakesSuccessfulRequestData =>
            new List<object[]>
            {
                new object[] { "oxford", "air", "en-us" },
                new object[] { "lexicala", "air", "en" },
                new object[] { "oxford", "love", "en-us" },
                new object[] { "lexicala", "love", "en" },
                new object[] { "oxford", "faire", "fr" },
                new object[] { "lexicala", "faire", "fr" },
            };

        /// <summary>
        /// Ensure that a given combination of search text + language code for a specific provider yields a valid response from the API. This test case only considers cases where an entry is FOUND within the API's database (ex: 'air' for 'en-us', and not 'fjh4892h8' for 'en-us').
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="searchText"></param>
        /// <param name="languageCode"></param>
        [Theory, MemberData(nameof(MakesSuccessfulRequestData), Skip = "Test consumes external API. Ignore to avoid depleting daily comsumption limits")]
        public async void MakesSuccessfulRequest(string provider, string searchText, string languageCode)
        {
            // Arrange
            var url = $"{BaseEndpoint}/{provider}/search?searchText={searchText}&languageCode={languageCode}";

            // Act
            var response = await _integrationTestFixture.HttpClient.GetAsync(url).Result.Content.ReadFromJsonAsync<DictionaryAPIResponse>();

            // Assert
            response.Data.Results.Should().NotBeNull("valid data should have been retrieved from this query.");
            response.Status.Should().Be("Success", "this query contains valid parameters, and has been manually tested before");
        }

        public static IEnumerable<object[]> MakeRequestWithBrokenSearchTextData =>
            new List<object[]>
            {
                        new object[] { "oxford", "thisiscompletebogus", "en-us" },
                        new object[] { "lexicala", "thisiscompletebogus", "en" },
            };

        /// <summary>
        /// Ensure that a given combination of search text + language code for a specific provider yields a valid response from the API, even though no entries are returned by the API.
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="searchText"></param>
        /// <param name="languageCode"></param>
        [Theory, MemberData(nameof(MakeRequestWithBrokenSearchTextData), Skip = "Test consumes external API. Ignore to avoid depleting daily comsumption limits")]
        public async void MakeRequestWithBrokenSearchText(string provider, string searchText, string languageCode)
        {
            // Arrange
            var url = $"{BaseEndpoint}/{provider}/search?searchText={searchText}&languageCode={languageCode}";

            // Act
            var response = await _integrationTestFixture.HttpClient.GetAsync(url).Result.Content.ReadFromJsonAsync<DictionaryAPIResponse>();

            // Assert
            response.Data.Results.Should().BeEmpty("the search text is complete bogus, and should not provide any results from the API");
            response.Status.Should().Be("Success", "this query contains valid parameters (even though they return zero results), and has been manually tested before");
        }

        public static IEnumerable<object[]> ReceiveBadRequestForInvalidInputData =>
            new List<object[]>
            {
                                new object[] { "oxford", "invalid.//.request", "en-us", new List<string> { GenericDictionaryAPIRequestHandler.ErrorMessages.InvalidSearchText } },
                                new object[] { "lexicala", "invalid.//.request", "en", new List<string> { GenericDictionaryAPIRequestHandler.ErrorMessages.InvalidSearchText } },
                                new object[] { "oxford", "validtext", "invalid", new List<string> { string.Format(GenericDictionaryAPIRequestHandler.ErrorMessages.InvalidLanguageCode, "invalid") } },
                                new object[] { "lexicala", "validtext", "invalid",  new List<string> { string.Format(GenericDictionaryAPIRequestHandler.ErrorMessages.InvalidLanguageCode, "invalid") } },
                                new object[] { "oxford", "invalid.//.request", "invalid", new List<string> { GenericDictionaryAPIRequestHandler.ErrorMessages.InvalidSearchText, string.Format(GenericDictionaryAPIRequestHandler.ErrorMessages.InvalidLanguageCode, "invalid") } },
                                new object[] { "lexicala", "invalid.//.request", "invalid", new List<string> { GenericDictionaryAPIRequestHandler.ErrorMessages.InvalidSearchText, string.Format(GenericDictionaryAPIRequestHandler.ErrorMessages.InvalidLanguageCode, "invalid") } } ,
            };

        /// <summary>
        /// Ensure that the proper error notifications are shown when invalid input is given for the API (Bad Request).
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="searchText"></param>
        /// <param name="languageCode"></param>
        [Theory, MemberData(nameof(ReceiveBadRequestForInvalidInputData), Skip = "Test consumes external API. Ignore to avoid depleting daily comsumption limits")]
        public async void ReceiveBadRequestForInvalidInput(string provider, string searchText, string languageCode, List<string> expectedErrorMessages)
        {
            // Arrange
            var url = $"{BaseEndpoint}/{provider}/search?searchText={searchText}&languageCode={languageCode}";

            // Act
            var response = await _integrationTestFixture.HttpClient.GetAsync(url).Result.Content.ReadFromJsonAsync<BaseResponseModel>();

            // Assert
            response.Status.Should().Be("Bad Request", "response should be 'Bad Request' because pre-emptive validations for the input should fail");
            response.Errors.Should().Contain(expectedErrorMessages);
        }
    }
    #endregion

    #region Image API
    public class ImageAPITests : IClassFixture<IntegrationTestFixture>
    {
        protected readonly IntegrationTestFixture _integrationTestFixture;
        protected readonly ITestOutputHelper _output;

        public string BaseEndpoint { get; set; } = $"api/v1/imageapi";

        public ImageAPITests(IntegrationTestFixture integrationTestFixture, ITestOutputHelper output)
        {
            _integrationTestFixture = integrationTestFixture;
            _output = output;
        }

        public static IEnumerable<object[]> MakesSuccessfulRequestData =>
            new List<object[]>
            {
                new object[] { "lol" },
                new object[] { "dogs" },
            };

        /// <summary>
        /// Ensure that a valid search text yields a valid response from the API.
        /// </summary>
        /// <param name="searchText"></param>
        [Theory, MemberData(nameof(MakesSuccessfulRequestData), Skip = "Test consumes external API. Ignore to avoid depleting daily comsumption limits")]
        public async void MakesSuccessfulRequest(string searchText)
        {
            // Arrange
            var url = $"{BaseEndpoint}/search?searchText={searchText}";

            // Act
            var response = await _integrationTestFixture.HttpClient.GetAsync(url).Result.Content.ReadFromJsonAsync<LargePaginatedListResponse<CustomSearchAPIImageResult>>(new System.Text.Json.JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            // Assert
            response.Data.Results.Should().NotBeEmpty("valid data should have been retrieved from this query.");
            response.Status.Should().Be("Success", "this query contains valid parameters, and has been manually tested before");
        }

        public static IEnumerable<object[]> MakesSuccessfulRequestWithPaginationData =>
            new List<object[]>
            {
                        new object[] { "dogs", 1 },
                        new object[] { "cats", null },
                        new object[] { "cows", 2 },
            };

        /// <summary>
        /// Ensure that a valid search text yields a valid response from the API.
        /// </summary>
        /// <param name="searchText"></param>
        [Theory, MemberData(nameof(MakesSuccessfulRequestWithPaginationData), Skip = "Test consumes external API. Ignore to avoid depleting daily comsumption limits")]
        public async void MakesSuccessfulRequestWithPagination(string searchText, long? pageNumber)
        {
            // Arrange
            var url = $"{BaseEndpoint}/search?searchText={searchText}&pageNumber={pageNumber ?? 1}";

            // Act
            var response = await _integrationTestFixture.HttpClient.GetAsync(url).Result.Content.ReadFromJsonAsync<LargePaginatedListResponse<CustomSearchAPIImageResult>>(new System.Text.Json.JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            // Assert
            response.Data.Results.Should().NotBeEmpty("valid data should have been retrieved from this query.");
            response.Status.Should().Be("Success", "this query contains valid parameters, and has been manually tested before");
            response.Data.PageNumber.Should().Be(pageNumber?.ToString() ?? "1", "it should be equal to the value requested to the API, or revert to the default value (1)");
        }

        public static IEnumerable<object[]> MakeRequestWithBrokenSearchTextData =>
            new List<object[]>
            {
                    new object[] { "cats", "-1", new List<string> { CustomSearchAPIService.ErrorMessages.InvalidPageNumber } }, // invalid page number
                    new object[] { "", "1", new List<string> { CustomSearchAPIService.ErrorMessages.InvalidSearchText } }, // invalid search text
                    new object[] { "", "-1", new List<string> { CustomSearchAPIService.ErrorMessages.InvalidPageNumber, CustomSearchAPIService.ErrorMessages.InvalidSearchText } }, // everything together
            };

        /// <summary>
        /// Ensure that the proper errors are thrown when input validations fail from the FlashMEMO side.
        /// </summary>
        [Theory, MemberData(nameof(MakeRequestWithBrokenSearchTextData), Skip = "Test consumes external API. Ignore to avoid depleting daily comsumption limits")]
        public async void MakeRequestWithBrokenSearchText(string searchText, string pageNumber, IEnumerable<string> expectedErrorMessages) // the reason for a 'string' pageNumber is to simulate the controller behavior, which through the middleware is able to recognize pre-emptively that negative values are not valid. If set to 'ulong', the test framework will start throwing errors that are not interesting for the sake of these tests. Therefore, a string value is passed into the test to better simulate the normal API behavior.
        {
            var url = $"{BaseEndpoint}/search?searchText={searchText}&pageSize=10&pageNumber={pageNumber ?? "1"}";

            // Act
            var response = await _integrationTestFixture.HttpClient.GetAsync(url).Result.Content.ReadFromJsonAsync<BaseResponseModel>(new System.Text.Json.JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            // Assert
            response.Status.Should().Be("Bad Request", "data should not be validated by the internal FlashMEMO filters");
            response.Errors.Should().Contain(expectedErrorMessages);
        }
    }
    #endregion
}
