using API.ViewModels;
using System.Collections.Generic;
using System.Net.Http.Json;
using Tests.Integration.Fixtures;
using Xunit;
using Xunit.Abstractions;
using FluentAssertions;
using Business.Services.Implementation;
using System;
using Business.Services.Interfaces;
using System.Net;
using Tests.Integration.Auxiliary;

namespace Tests.Integration.API
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

            new ControllerTestingAuthTokenInjector(_integrationTestFixture).SetupDummyJWTBearerAuthentication();
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

            new ControllerTestingAuthTokenInjector(_integrationTestFixture).SetupDummyJWTBearerAuthentication();
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
            var response = await _integrationTestFixture.HttpClient.GetAsync(url);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK, "Response from the API should have status code 200");
            var parsedResponse = await response.Content.ReadFromJsonAsync<LargePaginatedListResponse<CustomSearchAPIImageResult>>(new System.Text.Json.JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            parsedResponse.Data.Results.Should().NotBeEmpty("valid data should have been retrieved from this query.");
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
            var response = await _integrationTestFixture.HttpClient.GetAsync(url);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK, "Response from the API should have status code 200");
            var parsedResponse = await response.Content.ReadFromJsonAsync<LargePaginatedListResponse<CustomSearchAPIImageResult>>(new System.Text.Json.JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            parsedResponse.Data.Results.Should().NotBeEmpty("valid data should have been retrieved from this query.");
            parsedResponse.Data.PageNumber.Should().Be(pageNumber?.ToString() ?? "1", "it should be equal to the value requested to the API, or revert to the default value (1)");
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
            response.Errors.Should().Contain(expectedErrorMessages);
        }
    }
    #endregion

    #region Audio API
    public class AudioAPITests : IClassFixture<IntegrationTestFixture>
    {
        protected readonly IntegrationTestFixture _integrationTestFixture;
        protected readonly ITestOutputHelper _output;

        public string BaseEndpoint { get; set; } = $"api/v1/RedactedAPI";

        public AudioAPITests(IntegrationTestFixture integrationTestFixture, ITestOutputHelper output)
        {
            _integrationTestFixture = integrationTestFixture;
            _output = output;

            Environment.SetEnvironmentVariable("webdriver.chrome.driver", @"C:\Drivers\chromedriver.exe");
            new ControllerTestingAuthTokenInjector(_integrationTestFixture).SetupDummyJWTBearerAuthentication(); 

        }

        public static IEnumerable<object[]> MakesSuccessfulRequestData =>
            new List<object[]>
            {
                new object[] { "love", "en-us", AudioAPIProviderType.REDACTED },
                new object[] { "bonjour", "fr", AudioAPIProviderType.REDACTED },
                new object[] { "hola", "es", AudioAPIProviderType.REDACTED },
                new object[] { "bonjour", "fr", null }, // This is valid because, due to how C# works, the default value for a enum will the 0-index value, even if 'null' is used.
            };

        [Theory, MemberData(nameof(MakesSuccessfulRequestData), Skip = "Test consumes external API. Ignore to avoid depleting daily comsumption limits")]
        public async void MakesSuccessfulRequest(string keyword, string languageCode, AudioAPIProviderType provider)
        {
            // Arrange
            var url = $"{BaseEndpoint}/search?keyword={keyword}&languageCode={languageCode}&provider={provider}";

            // Act
            var response = await _integrationTestFixture.HttpClient.GetAsync(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK, "the request is well-formed");
            var results = await response.Content.ReadFromJsonAsync<DataResponseModel<AudioAPIDTO>>();
            results.Data.Results.AudioLinks.Should().NotBeNullOrEmpty("valid data should have been retrieved from this query");
            results.Message.Should().NotBeNullOrEmpty("the request should have been successful");
            results.Errors.Should().BeNullOrEmpty("the request should have been successful");
        }

        public static IEnumerable<object[]> MakeRequestWithBrokenSearchTextData =>
            new List<object[]>
            {
                new object[] { "akhtiolhjkhnfjlhlds", "en-us", AudioAPIProviderType.REDACTED }, // Returns nothing, which is expected
                new object[] { "bonjour", "ahlkjhtlkshldj", AudioAPIProviderType.REDACTED }, // Returns the usual results (languageCode parameter is actually redundant)
            };

        [Theory, MemberData(nameof(MakeRequestWithBrokenSearchTextData), Skip = "Test consumes external API. Ignore to avoid depleting daily comsumption limits")]
        public async void MakeRequestWithBrokenSearchText(string keyword, string languageCode, AudioAPIProviderType provider)
        {
            // Arrange
            var url = $"{BaseEndpoint}/search?keyword={keyword}&languageCode={languageCode}&provider={provider}";

            // Act
            var response = await _integrationTestFixture.HttpClient.GetAsync(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK, "the request is well-formed");
            var results = await response.Content.ReadFromJsonAsync<DataResponseModel<AudioAPIDTO>>();
            results.Message.Should().NotBeNullOrEmpty("the request should have been successful");
            results.Errors.Should().BeNullOrEmpty("the request should have been successful");
        }

        public static IEnumerable<object[]> ReceiveBadRequestForInvalidInputData =>
            new List<object[]>
            {
                new object[] { "", "fr", AudioAPIProviderType.REDACTED },
                new object[] { "macchina", "", AudioAPIProviderType.REDACTED },
            };

        [Theory, MemberData(nameof(ReceiveBadRequestForInvalidInputData), Skip = "Test consumes external API. Ignore to avoid depleting daily comsumption limits")]
        public async void ReceiveBadRequestForInvalidInput(string keyword, string languageCode, AudioAPIProviderType provider)
        {
            // Arrange
            var url = $"{BaseEndpoint}/search?keyword={keyword}&languageCode={languageCode}&provider={provider}";

            // Act
            var response = await _integrationTestFixture.HttpClient.GetAsync(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest, "the request is faulty");
            var results = await response.Content.ReadFromJsonAsync<DataResponseModel<AudioAPIDTO>>();
            results.Message.Should().NotBeNullOrEmpty("the request should have a standard message");
            results.Errors.Should().NotBeNullOrEmpty("the request is faulty");
        }

        /// <summary>
        /// This specialized test is used to see what happens when an invalid value is set to the 'provider' parameter. I have to isolate this test because c# can't bind a custom text value to a 'Enum' at runtime.
        /// </summary>
        [Fact(Skip = "Test consumes external API. Ignore to avoid depleting daily comsumption limits")]
        public async void ReceiveBadRequestForInvalidProvider()
        {
            // Arrange
            var keyword = "cat";
            var languageCode = "en";
            var provider = "bogus";

            var url = $"{BaseEndpoint}/search?keyword={keyword}&languageCode={languageCode}&provider={provider}";

            // Act
            var response = await _integrationTestFixture.HttpClient.GetAsync(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest, "the request is faulty");
            var results = await response.Content.ReadFromJsonAsync<DataResponseModel<AudioAPIDTO>>();
            results.Message.Should().NotBeNullOrEmpty("the request should have a standard message");
            results.Errors.Should().NotBeNullOrEmpty("the request is faulty");
        }
    }
    #endregion
}
