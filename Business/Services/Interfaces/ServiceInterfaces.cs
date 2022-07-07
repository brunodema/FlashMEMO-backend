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
using System.ComponentModel;
using static Data.Models.Implementation.StaticModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Business.Services.Interfaces
{
    /// <summary>
    /// Interface to hold lexical metadata for internal FlashMEMO API calls. Ex: Audio API calls always require a keyword and a target language to provide results. There are many other places where this interface could be applied to make the code more readable... this will be added to FlashMEMO's to-do list.
    /// </summary>
    /// <typeparam name="T">Type representing the API's return type. Ex: Audio API will return a list of links to audio files.</typeparam>
    public interface ILexicalAPIDTO<T> where T : class
    {
        /// <summary>
        /// Original search text used for the lexical query.
        /// </summary>
        string SearchText { get; set; }
        /// <summary>
        /// String containing the ISO code for the language (ex: "en-us", "en-gb").
        /// </summary>
        string LanguageCode { get; set; }
        /// <summary>
        /// Results obtained by the API. Can be a a list of dictionary objects, a list of audio files, etc.
        /// </summary>
        T Results { get; set; }
    }

    public interface IRepositoryService<TEntity, TKey>
        where TEntity : class
    {
        public Task<bool> IdAlreadyExists(TKey id);
        public Task<TKey> CreateAsync(TEntity entity, object[] auxParams = null); // to cover the 'CreateUserAsync' case (requires password)
        public Task<TKey> UpdateAsync(TEntity entity);
        public Task<TEntity> GetbyIdAsync(TKey id);
        public IEnumerable<TEntity> List(GenericSortOptions<TEntity> sortOptions, int numRecords = 1000);
        public Task<TKey> RemoveByIdAsync(TKey guid);
        public IEnumerable<TEntity> SearchAndOrder(IQueryFilterOptions<TEntity> filterOptions, GenericSortOptions<TEntity> sortOptions); // probably will transition towards this one

        // a new method has to be added, to cover the case where we want to retrieve all entries and sort them ('List' method of controller classes)

        public ValidatonResult CheckIfEntityIsValid(TEntity entity);
    }

    public interface IJWTServiceOptions
    {
        string ValidIssuer { get; set; }
        string ValidAudience { get; set; }
        int AccessTokenTTE { get; set; }
        int RefreshTokenTTE { get; set; }
        int ActivationTokenTTE { get; set; }
        int PasswordTokenTTE { get; set; }
        string Secret { get; set; }
    }
    public interface IJWTService
    {
        public string CreateAccessToken(User user);
        public string CreateRefreshToken(string accessToken, User user);
        public string CreateActivationToken(User user);
        public Task<bool> IsTokenExpired(string token);
        public JwtSecurityToken DecodeToken(string token);
        public Task<TokenValidationResult> ValidateTokenAsync(string token);
        public bool AreAuthTokensRelated(string accessToken, string refreshToken);
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

        /// <summary>
        /// Returns all available languages for the corresponding Dictionary provider.
        /// </summary>
        /// <returns></returns>
        Task<List<Language>> GetAvailableLanguages();
    }
    #endregion

    #region AUTH
    public interface IAuthServiceOptions
    {
        public int LockoutPeriod { get; set; }
    }

    public interface IAuthService<TKey>
    {
        public Task<bool> IsEmailAlreadyRegisteredAsync(string email);
        public Task<bool> IsIdAlreadyRegisteredAsync(TKey id);
        public Task<bool> IsUsernameAlreadyRegistered(string username);
        public Task<TKey> CreateUserAsync(User user, string cleanPassword, bool emailConfirmed);
        /// <summary>
        /// Checks is the provided credentials are valid, returning the user object is so. Otherwise, returns null.
        /// </summary>
        /// <param name="credentials">An email + password combination.</param>
        /// <returns>The user associated with the email if authentication is successful, or null instead.</returns>
        public Task<User> AreCredentialsValidAsync(IFlashMEMOCredentials credentials);
        /// <summary>
        /// Sets the 'LastLogin' property to the current time.
        /// </summary>
        /// <param name="user"></param>
        public Task UpdateLastLoginAsync(User user);
        public bool IsUserLocked(User user);

        public Task<string> GeneratePasswordResetToken(User user);
        public Task ResetPasswordAsync(User user, string resetToken, string newPassword);
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
    /// <summary>
    /// Enum declaring the available pronunciation providers.
    /// </summary>
    public enum AudioAPIProviderType
    {
        REDACTED,
    }

    /// <summary>
    /// Interface representing an individual result object returned by Audio API providers. Contains the list of links with pronunciation resources, and the time the back-end took to process it.
    /// </summary>
    public interface IAudioAPIResult
    {
        /// <summary>
        /// List of audio resources retrieved by the API.
        /// </summary>
        List<string> AudioLinks { get; set; }
        /// <summary>
        /// Time it took for the back-end to do its API 'magic' and get the audio resource. Note: from now on, all numbers returned to the front-end by FlashMEMO classes will be of 'string' type, to avoid issues with overflow, or whatever (even if the risk is low for the majoritiy of cases).
        /// </summary>
        string ProcessingTime { get; set; }
    }
    /// <summary>
    /// Interface holding the main functionalities of an Audio API service. This interface is assumed to be used for providers restricted to pronunciation resources, as many dictionary ones also provide pronunciation with their results, when applicable. In this case, the controllers will be responsible for routing the calls to the appropriate internal services (ex: search dictionary entry using Oxford API, but filter only audio link result).
    /// </summary>
    public interface IAudioAPIService : IAPIService
    {
        /// <summary>
        /// Uses the internal lexical audio provider to retrieve a pronunciation file from external services. Returns a list of strings containing links to audio files hosted outside FlashMEMO.
        /// </summary>
        /// <param name="keyword">Target word for pronunciation search.</param>
        /// <param name="languageCode">Language code to be used for the search. In theory can allow searches as: pronunciation of 'hello' in spanish.</param>
        /// <returns>List of links with pronunciation audios.</returns>
        Task<ILexicalAPIDTO<IAudioAPIResult>> SearchAudioAsync(string keyword, string languageCode, AudioAPIProviderType provider);
    }
    #endregion

    #region Language Service
    public interface ILanguageService
    {
        bool LanguageExists(string languageCode);
    }
    #endregion

    #region EmailService
    public interface IEmailService
    {
        Task SendRegistrationAsync(User user, string token);
        Task SendPasswordRecoveryAsync(User user, string token);
    }
    #endregion
}
