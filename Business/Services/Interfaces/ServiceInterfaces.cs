using Business.Tools;
using Business.Tools.Interfaces;
using Data.Models.Implementation;
using Data.Tools.Implementation;
using Data.Tools.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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
    /// <summary>
    /// Interface representing the FlashMEMO (minimalistic) representation of a individual result of a dictionary API request.
    /// </summary>
    public interface IDictionaryAPIResult
    {
        string LexicalCategory { get; set; }
        string PronunciationFile { get; set; }
        string PhoneticSpelling { get; set; }
        List<string> Definitions { get; set; }
        List<string> Examples { get; set; }
    }

    /// <summary>
    /// Interface representing the FlashMEMO (minimalistic) response of a dictionary API request.
    /// </summary>
    public interface IDictionaryAPIResponse
    {
        string SearchText { get; set; }
        /// <summary>
        /// String containing the ISO code for the language (ex: "en-us", "en-gb").
        /// </summary>
        string LanguageCode { get; set; }
        List<IDictionaryAPIResult> Results { get; set; }
    }

    public interface IDictionaryAPIServiceOptions
    {
        /// <summary>
        /// Returns the full URL to be used by the service when querying the API.
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="targetLanguage">String containing the ISO code for the language (ex: "en-us", "en-gb").</param>
        /// <returns></returns>
        string BuildSearchURL(string searchText, string targetLanguage);
        /// <summary>
        /// Setups key-value pairs to be used as headers in the web request to the API (ex: "app_id: {app_idd_value}").
        /// </summary>
        /// <returns>It might return 'null' if the operation is not necessary (i.e., authentication is made via query parameters), otherwise it returns the headers in a dictionary format.</returns>
        Dictionary<string, IEnumerable<string>> SetupCredentials();
    }

    public interface IDictionaryAPIService : IAPIService
    {
        /// <summary>
        /// Queries the API to retrieve definitions for the filter text in the target language.
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="targetLanguage">Two letter ISO code for the target language (ex: "en-us", "en-gb").</param>
        /// <returns></returns>
        Task<IDictionaryAPIResponse> SearchResults(string searchText, string targetLanguage);
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
