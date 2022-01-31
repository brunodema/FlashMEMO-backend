using API.ViewModels;
using System.Collections.Generic;
using System.Net.Http.Json;
using Tests.Integration.Fixtures;
using Xunit;
using Xunit.Abstractions;
using FluentAssertions;

namespace Tests.Integration.Implementation
{
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
            var entity = await _integrationTestFixture.HttpClient.GetAsync(url).Result.Content.ReadFromJsonAsync<DictionaryAPIResponse>();

            // Assert
            entity.Data.Results.Should().NotBeNull("valid data should have been retrieved from this query.");
            entity.Status.Should().Be("Success", "this query contains valid parameters, and has been manually tested before");
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
            var entity = await _integrationTestFixture.HttpClient.GetAsync(url).Result.Content.ReadFromJsonAsync<DictionaryAPIResponse>();

            // Assert
            entity.Data.Results.Should().BeEmpty("the search text is complete bogus, and should not provide any results from the API");
            entity.Status.Should().Be("Success", "this query contains valid parameters (even though they return zero results), and has been manually tested before");
        }
    }
}
