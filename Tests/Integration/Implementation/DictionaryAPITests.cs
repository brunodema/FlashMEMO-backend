using API.ViewModels;
using System.Collections.Generic;
using System.Net.Http.Json;
using Tests.Integration.Fixtures;
using Xunit;
using Xunit.Abstractions;

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
            };

        /// <summary>
        /// Ensure that a given combination of search text + language code for a specific provider yields a valid response from the API. This test case only considers cases where an entry is FOUND within the API's database (ex: 'air' for 'en-us', and not 'fjh4892h8' for 'en-us').
        /// </summary>
        [Theory, MemberData(nameof(MakesSuccessfulRequestData))]
        public async void MakesSuccessfulRequest(string provider, string searchText, string languageCode)
        {
            // Arrange
            var url = $"{BaseEndpoint}/{provider}/search?searchText={searchText}&languageCode={languageCode}";
            var entity = await _integrationTestFixture.HttpClient.GetAsync(url).Result.Content.ReadFromJsonAsync<DictionaryAPIResponse>();
            _output.WriteLine(entity.ToString());
            // Act


            // Assert



            // template
            //var entity = await _integrationTestFixture.HttpClient.GetAsync($"{GetEndpoint}/{id}").Result.Content.ReadFromJsonAsync<TEntity>();
            //var body = JsonContent.Create(id);

            //// Act
            //var response = await _integrationTestFixture.HttpClient.PostAsync(DeleteEndpoint, body);
            //var parsedResponse = await response.Content.ReadFromJsonAsync<BaseResponseModel>();

            //// Assert
            //Assert.True(response.StatusCode == HttpStatusCode.OK);
            //Assert.Null(parsedResponse.Errors);

            //// Undo
            //response = await _integrationTestFixture.HttpClient.PostAsync(CreateEndpoint, JsonContent.Create(entity));
            //parsedResponse = await response.Content.ReadFromJsonAsync<BaseResponseModel>();
            //Assert.True(response.StatusCode == HttpStatusCode.OK);
            //Assert.Null(parsedResponse.Errors);
        }
    }
}
