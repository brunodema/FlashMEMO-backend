﻿using Business.Services.Abstract;
using Business.Services.Interfaces;
using Business.Tools;
using Business.Tools.Interfaces;
using Business.Tools.DictionaryAPI;
using Data.Models.Implementation;
using Data.Repository.Implementation;
using Google.Apis.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Google.Apis.CustomSearchAPI.v1.CseResource;
using static Google.Apis.CustomSearchAPI.v1.Data.Result;

namespace Business.Services.Implementation
{
    public class NewsService : GenericRepositoryService<NewsRepository, Guid, News>
    {
        public NewsService(NewsRepository baseRepository, IOptions<GenericRepositoryServiceOptions> serviceOptions) : base(baseRepository, serviceOptions.Value) { }
        public override ValidatonResult CheckIfEntityIsValid(News entity)
        {
            bool areDatesValid = entity.CreationDate <= entity.LastUpdated;

            List<string> errors = new();
            if (!areDatesValid)
            {
                errors.Add("The last updated date must be more recent than the creation date.");
            }

            return new ValidatonResult 
            {
                IsValid = areDatesValid,
                Errors = errors
            };
        }
    }

    public class JWTServiceOptions : IJWTServiceOptions
    {
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
        public double TimeToExpiration { get; set; }
        public string Secret { get; set; }
    }

    public class JWTService : IJWTService
    {
        private readonly IJWTServiceOptions _options;

        public JWTService(IOptions<JWTServiceOptions> options)
        {
            _options = options.Value;
        }

        public string CreateLoginToken(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            foreach (var role in user.UserRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.RoleId));
            }

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Secret));

            return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
                issuer: _options.ValidIssuer, audience: _options.ValidAudience,
                expires: DateTime.Now.AddSeconds(Convert.ToDouble(_options.TimeToExpiration)),
                claims: claims,
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            ));

        }
    }

    // no interfaces for this one because there are no other viable replacements for this API at the moment
    public class CustomSearchAPIImageResult
    {
        public string Title { get; set; }
        public ImageData Image { get; set; }
        public string Link { get; set; }

        public CustomSearchAPIImageResult(string title, ImageData imageData, string link)
        {
            Title = title;
            Image = imageData;
            Link = link;
        }
    }

    public class CustomSearchAPIResponse
    {
        public IEnumerable<CustomSearchAPIImageResult> Results { get; set; }
        public int ResultSize { get; set; }
        public UInt64 PageIndex { get; set; }
        public UInt64 TotalAmount { get; set; }
        public UInt64 TotalPages { get; set; }
    }

    public class CustomSearchAPIServiceOptions
    {
        public string BaseURL { get; set; }
        public string EngineID { get; set; }
        public string Token { get; set; }
    }
    public class CustomSearchAPIService : IAPIService
    {
        private readonly CustomSearchAPIServiceOptions _options;

        public CustomSearchAPIService(IOptions<CustomSearchAPIServiceOptions> options)
        {
            _options = options.Value;
        }

        public HttpResponse CheckAvailability()
        {
            throw new NotImplementedException();
        }

        public HttpResponse CheckPeriodComsumption()
        {
            throw new NotImplementedException();
        }

        private bool IsSearchTextValid(string searchText)
        {
            return !String.IsNullOrEmpty(searchText);
        }

        private List<string> IsInputValid(string searchText, int pageSize, int pageNumber)
        {
            var errorMessages = new List<string>();

            if (!IsSearchTextValid(searchText))
            {
                errorMessages.Add("The search text used is not valid. It is either 'null' or empty.");
            }
            if (pageSize <= 0 || pageSize > 10) // the GT part is a restriction of the Google API itself
            {
                errorMessages.Add("The page size has an invalid number (less or equal than 0, or greater than 10).");
            }
            if (pageNumber <= 0)
            {
                errorMessages.Add("The page size has an invalid number (less or equal than 0).");
            }

            return errorMessages;
        }

        public async Task<CustomSearchAPIResponse> Search(string searchText, int pageSize = 10, int pageNumber = 1)
        {
            try
            {
                using (var service = new Google.Apis.CustomSearchAPI.v1.CustomSearchAPIService(new BaseClientService.Initializer { ApplicationName = "FlashMEMO Image Search", ApiKey = _options.Token }))
                {
                    ListRequest listRequest = service.Cse.List();

                    var validationsMessages = IsInputValid(searchText, pageSize, pageNumber);
                    if (validationsMessages.Count > 0)
                    {
                        throw new InputValidationException()
                        {
                            InputValidationErrors = validationsMessages
                        };
                    }

                    listRequest.Q = searchText;
                    listRequest.SearchType = ListRequest.SearchTypeEnum.Image;
                    listRequest.Cx = _options.EngineID;
                    listRequest.Start = (pageSize * pageNumber) - pageSize;
                    listRequest.Num = pageSize;
                    var results = await listRequest.ExecuteAsync();

                    var totalAmount = Convert.ToUInt64(results?.SearchInformation.TotalResults ?? "0");
                    return new CustomSearchAPIResponse
                    {
                        Results = results.Items.Select(i => new CustomSearchAPIImageResult(i.Title, i.Image, i.Link)).ToList(),
                        ResultSize = results?.Items?.Count ?? 0,
                        PageIndex = Convert.ToUInt64(pageNumber),
                        TotalAmount = totalAmount,
                        TotalPages = Convert.ToUInt64(Math.Ceiling(totalAmount / (double)pageSize))

                    };
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    #region DICTIONARY API
    #region Lexicala
    public class LexicalaDictionaryAPIServiceOptions : IDictionaryAPIServiceOptions
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public string BuildSearchURL(string searchText, string targetLanguage)
        {
            return $"https://dictapi.lexicala.com/search?source=global&language={targetLanguage}&text={searchText}";
        }

        public Dictionary<string, IEnumerable<string>> SetupCredentials()
        {
            var dict = new Dictionary<string, IEnumerable<string>>();
            dict.Add("username", new List<string> { Username });
            dict.Add("password", new List<string> { Password });

            return dict;
        }
    }
    #endregion
    #region Oxford
    public class OxfordDictionaryAPIResult : IDictionaryAPIResult
    {
        public string LexicalCategory { get; set; }
        public string PronunciationFile { get; set; }
        public string PhoneticSpelling { get; set; }
        public List<string> Definitions { get; set; }
        public List<string> Examples { get; set; }
    }

    public class OxfordDictionaryAPIServiceOptions : IDictionaryAPIServiceOptions
    {
        public string AppID { get; set; }
        public string AppKey { get; set; }

        public string BuildSearchURL(string searchText, string targetLanguage)
        {
            return $"https://od-api.oxforddictionaries.com:443/api/v2/entries/{targetLanguage}/{searchText}";
        }

        public Dictionary<string, IEnumerable<string>> SetupCredentials()
        {
            var dict = new Dictionary<string, IEnumerable<string>>();
            dict.Add("app_id", new List<string>{ AppID });
            dict.Add("app_key", new List<string> { AppKey });

            return dict;
        }
    }

    public class DictionaryAPIService : IDictionaryAPIService
    {
        private IDictionaryAPIServiceOptions _serviceOptions;

        public DictionaryAPIService(IOptions<IDictionaryAPIServiceOptions> options)
        {
            _serviceOptions = options.Value;
        }

        public HttpResponse CheckAvailability()
        {
            throw new NotImplementedException();
        }

        public HttpResponse CheckPeriodComsumption()
        {
            throw new NotImplementedException();
        }

        public async Task<IDictionaryAPIDTO> SearchResults(string searchText, string targetLanguage)
        {
            using (var client = new HttpClient())
            {
                foreach (var item in _serviceOptions.SetupCredentials())
                {
                    client.DefaultRequestHeaders.Add(item.Key, item.Value);
                }
                
                var response = await client.GetAsync(_serviceOptions.BuildSearchURL(searchText, targetLanguage));

                var parsedResponse = JsonSerializer.Deserialize<OxfordAPIResponseModel>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true} );

                return new OxfordAPIDTO(parsedResponse);
            }
        }
    }
    #endregion
    #endregion


    public class AuthServiceOptions : IAuthServiceOptions
    {

    }
    public class AuthService : IAuthService
    {
        private readonly IAuthServiceOptions _options;
        private readonly ApplicationUserRepository _applicationUserRepository;
        private readonly RoleRepository _roleRepository;

        public AuthService(IOptions<AuthServiceOptions> options, ApplicationUserRepository applicationUserRepository, RoleRepository roleRepository)
        {
            _options = options.Value;
            _applicationUserRepository = applicationUserRepository;
            _roleRepository = roleRepository;
        }

        public async Task<bool> AreCredentialsValidAsync(IFlashMEMOCredentials credentials)
        {
            if (await UserAlreadyExistsAsync(credentials.Email))
            {
                if (await GetUserByEmailAndCheckCredentialsAsync(credentials)) return true;
            }

            return false;
        }

        public async Task<IdentityResult> CreateUserAsync(ApplicationUser user, string cleanPassword)
        {
            var result = await _applicationUserRepository.CreateAsync(user);
            await _applicationUserRepository.SetInitialPassword(user.Id, cleanPassword);
            return result;
        }

        public async Task<bool> UserAlreadyExistsAsync(string email)
        {
            return (await _applicationUserRepository.SearchFirstAsync(u => u.Email == email)) != null;
        }
        public async Task<bool> GetUserByEmailAndCheckCredentialsAsync(IFlashMEMOCredentials credentials)
        {
            var user = await _applicationUserRepository.SearchFirstAsync(u => u.Email == credentials.Email);
            return await _applicationUserRepository.CheckPasswordAsync(user.Id, credentials.PasswordHash);
        }
    }
}
