using Business.Services.Implementation;
using Business.Tools;
using Business.Tools.DictionaryAPI.Lexicala;
using Business.Tools.DictionaryAPI.Oxford;
using Business.Tools.Interfaces;
using Data.Models.Implementation;
using Data.Tools.Implementation;
using Data.Tools.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;

namespace Business.Services.Interfaces
{
    public interface IRepositoryService<TEntity, TKey>
        where TEntity : class
    {
        public Task<bool> IdAlreadyExists(TKey id);
        public Task CreateAsync(TEntity entity, object[] auxParams = null); // to cover the 'CreateUserAsync' case (requires password)
        public Task UpdateAsync(TEntity entity);
        public Task<TEntity> GetbyIdAsync(TKey id);
        public IEnumerable<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, GenericSortOptions<TEntity> sortOptions, int numRecords = 1000);
        public Task RemoveByIdAsync(TKey guid);
        public IEnumerable<TEntity> SearchAndOrder(IQueryFilterOptions<TEntity> filterOptions, GenericSortOptions<TEntity> sortOptions); // probably will transition towards this one
        public ValidatonResult CheckIfEntityIsValid(TEntity entity);
    }

    public interface IJWTServiceOptions
    {
        string ValidIssuer { get; set; }
        string ValidAudience { get; set; }
        double TimeToExpiration { get; set; }
        string Secret { get; set; }
    }
    public interface IJWTService
    {
        public string CreateLoginToken(ApplicationUser user);
    }

    #region DICTIONARY API

    public class DictionaryAPIDTO
    {
        public string SearchText { get; set; }
        /// <summary>
        /// String containing the ISO code for the language (ex: "en-us", "en-gb").
        /// </summary>
        public string LanguageCode { get; set; }
        public List<DictionaryAPIResult> Results { get; set; }
    }

    public class DictionaryAPIDTOMapper
    {
        public static DictionaryAPIDTO CreateDTO(IDictionaryAPIResponse dictionaryAPIResponse)
        {
            var dto = new DictionaryAPIDTO();

            switch (dictionaryAPIResponse)
            {
                case LexicalaAPIResponseModel lexicalaResponse:

                    dto.LanguageCode = lexicalaResponse.Results[0].Language;  // shouldn't be different accross entries anyway
                    dto.SearchText = lexicalaResponse.Results[0].Headword.Text;  // shouldn't be different accross entries anyway
                    dto.Results = new List<DictionaryAPIResult>();

                    foreach (var result in lexicalaResponse.Results)
                    {
                        var dictAPIResult = new DictionaryAPIResult()
                        {
                            LexicalCategory = result.Headword.Pos,
                            PhoneticSpelling = result.Headword.Pronunciation?.Value ?? "",
                            PronunciationFile = "",
                            Definitions = new List<string>(),
                            Examples = new List<string>(),
                        };

                        foreach (var sense in result.Senses)
                        {
                            if (sense.Definition is not null) dictAPIResult.Definitions.Add(sense.Definition);
                            if (sense.Examples is not null) dictAPIResult.Examples.AddRange(sense.Examples.Select(s => s.Text).ToList());
                        }

                        dto.Results.Add(dictAPIResult);
                    }

                    return dto;

                case OxfordAPIResponseModel oxfordResponse:

                    dto.SearchText = oxfordResponse.Id;
                    dto.LanguageCode = oxfordResponse.Results[0].Language; // shouldn't be different accross entries anyway
                    dto.Results = new List<DictionaryAPIResult>();

                    foreach (var result in oxfordResponse.Results)
                    {
                        foreach (var lexicalEntry in result.LexicalEntries)
                        {
                            var dictAPIResult = new DictionaryAPIResult()
                            {
                                LexicalCategory = lexicalEntry.LexicalCategory.Text,
                                PhoneticSpelling = "",
                                PronunciationFile = "",
                                Definitions = new List<string>(),
                                Examples = new List<string>(),
                            };

                            foreach (var entry in lexicalEntry.Entries)
                            {
                                dictAPIResult.PronunciationFile = entry.Pronunciations?.Select(p => p.AudioFile)?.FirstOrDefault() ?? ""; // won't bother with additional pronunciations/spellings for now
                                dictAPIResult.PhoneticSpelling = entry.Pronunciations?.Select(p => p.PhoneticSpelling)?.FirstOrDefault() ?? ""; // won't bother with additional pronunciations/spellings for now

                                foreach (var sense in entry.Senses)
                                {
                                    if (sense.Definitions is not null) dictAPIResult.Definitions.AddRange(sense.Definitions.ToList());
                                    if (sense.Examples is not null) dictAPIResult.Examples.AddRange(sense.Examples.Select(e => e.Text).ToList());
                                }
                            }

                            dto.Results.Add(dictAPIResult);
                        }
                    }

                    return dto;

                default:
                    throw new Exception();
            }
        }
    }

    /// <summary>
    /// Besides holding the configurations of the Dictionary APIs located in the config file, contains the logic that sets up and executes the web request to the APIs.
    /// </summary>
    public interface IDictionaryAPIRequestHandler
    {
        /// <summary>
        /// Builds the HTTPClient object and makes the web request to the API. Different APIs use different endpoints and have different ways of setting up authentication for the services (i.e., basic auth, simple headers, etc).
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="targetLanguage">String containing the ISO code for the language (ex: "en-us", "en-gb").</param>
        /// <returns></returns>
        Task<HttpResponseMessage> MakeRequestToAPIAsync(string searchText, string targetLanguage);
    }

    /// <summary>
    /// Interface to be used as is (no implementation in it) for types that were mapped via json2csharp (https://json2csharp.com/).
    /// </summary>
    public interface IDictionaryAPIResponse
    {

    }

    public interface IDictionaryAPIService<TDictionaryAPIResponse> : IAPIService
        where TDictionaryAPIResponse : IDictionaryAPIResponse
    {
        /// <summary>
        /// Queries the API to retrieve definitions for the filter text in the target language.
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="targetLanguage">Two letter ISO code for the target language (ex: "en-us", "en-gb").</param>
        /// <returns></returns>
        Task<DictionaryAPIDTO> SearchResults(string searchText, string targetLanguage);
    }
    #endregion

    #region AUTH
    public interface IAuthServiceOptions
    {
    }
    public interface IAuthService
    {
        public Task<bool> UserAlreadyExistsAsync(string email);
        public Task<IdentityResult> CreateUserAsync(ApplicationUser user, string cleanPassword);
        public Task<bool> AreCredentialsValidAsync(IFlashMEMOCredentials credentials);
    }
    #endregion

    // I was gonna add a 'ValidateInput' function here too, but I realized that some APIs might require more than one validation function. Even though this likely won't happen, I don't want to overcomplicate things for the moment.
    public interface IAPIService
    {
        /// <summary>
        /// Test if the API is reachable by the client.
        /// </summary>
        /// <returns>An HTTP response object. Not sure if this is the most appropriate class to be returned, but the most important thing is to check if a 200 will be returned or not. </returns>
        /// <exception cref="NotImplementedException"></exception>
        HttpResponse CheckAvailability();

        /// <summary>
        /// Asks the API provider for information on the comsumption of the API in a given time period. Ex: Google APIs have request limits, especially the free tiers of them.
        /// </summary>
        /// <returns>An HTTP response object. Not sure if this is the most appropriate class to be returned, but the most important thing is to check if the API has some sort of monitoring or not.</returns>
        /// <exception cref="NotImplementedException"></exception>
        HttpResponse CheckPeriodComsumption();
    }
}
