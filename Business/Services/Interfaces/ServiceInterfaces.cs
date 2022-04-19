using Business.Services.Implementation;
using Business.Tools;
using Business.Tools.Interfaces;
using Data.Models.Implementation;
using Data.Tools.Sorting;
using Data.Tools.Filtering;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Services.Interfaces
{
    /// <summary>
    /// Interface to hold lexical metadata for internal FlashMEMO API calls. Ex: Audio API calls always require a keyword and a target language to provide results. There are many other places where this interface could be applied to make the code more readable... this will be added to FlashMEMO's to-do list.
    /// </summary>
    /// <typeparam name="T">Type representing the API's return type. Ex: Audio API will return a list of links to audio files.</typeparam>
    public interface ILexicalAPIResult<T> where T : class
    {
        string SearchText { get; set; }
        string LanguageCode { get; set; }
        T Results { get; set; }
    }

    public interface IRepositoryService<TEntity, TKey>
        where TEntity : class
    {
        public Task<bool> IdAlreadyExists(TKey id);
        public Task CreateAsync(TEntity entity, object[] auxParams = null); // to cover the 'CreateUserAsync' case (requires password)
        public Task UpdateAsync(TEntity entity);
        public Task<TEntity> GetbyIdAsync(TKey id);
        public IEnumerable<TEntity> ListAsync(GenericSortOptions<TEntity> sortOptions, int numRecords = 1000);
        public Task RemoveByIdAsync(TKey guid);
        public IEnumerable<TEntity> SearchAndOrder(IQueryFilterOptions<TEntity> filterOptions, GenericSortOptions<TEntity> sortOptions); // probably will transition towards this one

        // a new method has to be added, to cover the case where we want to retrieve all entries and sort them ('List' method of controller classes)

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
    /// Interface to be used as is (no implementation in it) for types that were mapped via json2csharp (https://json2csharp.com/).
    /// </summary>
    public interface IDictionaryAPIResponse
    {
        bool HasAnyResults();
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
        /// <summary>
        /// Checks is the provided credentials are valid, returning the user object is so. Otherwise, returns null.
        /// </summary>
        /// <param name="credentials">An email + password combination.</param>
        /// <returns>The user associated with the email if authentication is successful, or null instead.</returns>
        public Task<ApplicationUser> AreCredentialsValidAsync(IFlashMEMOCredentials credentials);
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

    #region AUDIO API
    public interface IAudioAPIServiceResponse : ILexicalAPIResult<string[]>
    {
    }

    public interface IAudioAPIService : IAPIService
    {
        /// <summary>
        /// Uses the internal lexical audio provider to retrieve a pronunciation file from external services. Returns a list of strings containing links to audio files hosted outside FlashMEMO.
        /// </summary>
        /// <param name="keyword">Target word for pronunciation search.</param>
        /// <param name="languageCode">Language code to be used for the search. In theory can allow searches as: pronunciation of 'hello' in spanish.</param>
        /// <returns></returns>
        IAudioAPIServiceResponse searchAudio(string keyword, string languageCode);
    }
    #endregion
}
