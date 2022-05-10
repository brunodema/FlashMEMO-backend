using Google.Apis.Services;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using static Google.Apis.CustomSearchAPI.v1.CseResource;
using static Google.Apis.CustomSearchAPI.v1.Data.Result;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Business.Tools.DictionaryAPI.Oxford;
using Business.Tools.DictionaryAPI.Lexicala;
using Business.Tools.Exceptions;
using System.Text.RegularExpressions;
using System.Numerics;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools;
using DevToolsSessionDomains = OpenQA.Selenium.DevTools.V96.DevToolsSessionDomains;
using Network = OpenQA.Selenium.DevTools.V96.Network;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;
using Business.Services.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using System.Linq;
using Business.Tools;
using OpenQA.Selenium;

namespace Business.Services.Implementation
{
    // no interfaces for this one because there are no other viable replacements for this API at the moment
    public class CustomSearchAPIImageResult
    {
        public string Title { get; set; }
        public ImageData Image { get; set; }
        public string Link { get; set; }

        public CustomSearchAPIImageResult() { }
    }

    /// <summary>
    /// This 'Response' class must exist here because otherwise there would be a circular dependency between the business layer and the API one. This class pretty much is a clone of a large paginated list.
    /// </summary>
    public class CustomSearchAPIResponse
    {
        public IEnumerable<CustomSearchAPIImageResult> Results { get; set; }
        public int PageSize { get; set; }
        public string PageNumber { get; set; }
        public string TotalAmount { get; set; }
        public string TotalPages { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
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

        public static class ErrorMessages
        {
            public static readonly string InvalidSearchText = "The search text used is not valid. It is either 'null' or empty.";
            public static readonly string InvalidPageNumber = "The page number has an invalid number (less or equal than 0).";
        }

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

        private List<string> IsInputValid(string searchText, long pageNumber)
        {
            var errorMessages = new List<string>();

            if (!IsSearchTextValid(searchText))
            {
                errorMessages.Add(ErrorMessages.InvalidSearchText);
            }
            if (pageNumber <= 0)
            {
                errorMessages.Add(ErrorMessages.InvalidPageNumber);
            }

            return errorMessages;
        }

        public async Task<CustomSearchAPIResponse> Search(string searchText, long pageNumber = 1)
        {
            try
            {
                using (var service = new Google.Apis.CustomSearchAPI.v1.CustomSearchAPIService(new BaseClientService.Initializer { ApplicationName = "FlashMEMO Image Search", ApiKey = _options.Token }))
                {
                    ListRequest listRequest = service.Cse.List();

                    var validationsMessages = IsInputValid(searchText, pageNumber);
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
                    listRequest.Start = (10 * pageNumber) - 10;
                    listRequest.Num = 10; // another interesting thing: if there are 10 results per page, and total number of results in the range of billions, this API is not ready to deal with the entire range of results pointed by the response. Not that it matters too much, but still...

                    var results = await listRequest.ExecuteAsync();

                    // this crazyness here is to *attempt* to avoid integer overflow, which is something that gave me a lot of headache in this API. BTW, there is no guarantee that the calculation below won't cause overflows - we just hope it doesn't
                    var parsedTotalResults = BigInteger.Parse(results.SearchInformation.TotalResults);
                    var totalPages = (parsedTotalResults / 10).ToString();

                    return new CustomSearchAPIResponse
                    {
                        Results = results.Items.Select(i => new CustomSearchAPIImageResult() { Title = i.Title, Image = i.Image, Link = i.Link }),
                        PageSize = results?.Items?.Count ?? 0,
                        PageNumber = pageNumber.ToString(),
                        TotalPages = totalPages,
                        TotalAmount = results?.SearchInformation.TotalResults ?? "0",
                        HasPreviousPage = results.Queries.PreviousPage?.Any() ?? false, // the 'null' check would be sufficient here, considering how Google returns the data (ex: 'PreviousPage' returns 'null')
                        HasNextPage = results.Queries.NextPage?.Any() ?? false,

                        // EXTREMELY IMPORTANT: GIVEN THAT THE LACK OF PROPERTIES CAUSED PROBLEMS ON THE FRONT-END (HAS NEXT/PREVIOUS PAGE, TOTALPAGES), ADD TESTS RELATED TO THIS!!!
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
    public class LexicalaDictionaryAPIRequestHandler : GenericDictionaryAPIRequestHandler
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public override async Task<HttpResponseMessage> MakeRequestToAPIAsync(string searchText, string targetLanguage)
        {
            var textValidation = ValidateSearchText(searchText);
            var codeValidation = ValidateLanguageCode(targetLanguage);

            if (!textValidation.IsValid || !codeValidation.IsValid)
            {
                var inputValidationErrors = new List<string>();
                if (textValidation.Errors is not null) inputValidationErrors.AddRange(textValidation.Errors);
                if (codeValidation.Errors is not null) inputValidationErrors.AddRange(codeValidation.Errors);

                throw new InputValidationException() { InputValidationErrors = inputValidationErrors };
            }

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            using (var client = new HttpClient(clientHandler))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes($"{Username}:{Password}")));

                return await client.GetAsync($"https://dictapi.lexicala.com/search-entries?source=global&language={targetLanguage}&text={searchText}");
            }
        }
    }
    #endregion

    #region Oxford
    public class OxfordDictionaryAPIRequestHandler : GenericDictionaryAPIRequestHandler
    {
        public string AppID { get; set; }
        public string AppKey { get; set; }

        public override async Task<HttpResponseMessage> MakeRequestToAPIAsync(string searchText, string targetLanguage)
        {
            var textValidation = ValidateSearchText(searchText);
            var codeValidation = ValidateLanguageCode(targetLanguage);

            if (!textValidation.IsValid || !codeValidation.IsValid)
            {
                var inputValidationErrors = new List<string>();
                if (textValidation.Errors is not null) inputValidationErrors.AddRange(textValidation.Errors);
                if (codeValidation.Errors is not null) inputValidationErrors.AddRange(codeValidation.Errors);

                throw new InputValidationException() { InputValidationErrors = inputValidationErrors };
            }

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("app_id", new List<string> { AppID });
                client.DefaultRequestHeaders.Add("app_key", new List<string> { AppKey });

                return await client.GetAsync($"https://od-api.oxforddictionaries.com:443/api/v2/entries/{targetLanguage}/{searchText}");
            }
        }
    }
    #endregion

    /// <summary>
    /// Besides holding the configurations of the Dictionary APIs located in the config file, contains the logic that sets up and executes the web request to the APIs.
    /// </summary>
    public abstract class GenericDictionaryAPIRequestHandler
    {
        public static class ErrorMessages
        {
            public const string InvalidSearchText = "The search text provided is not valid for this API. Only characters (A-Z or a-z) and numbers (0-9) are allowed.";
            public const string InvalidLanguageCode = "The language code provided ('{0}') is not valid for this API.";
        }

        /// <summary>
        /// List of supported languages by the API (2-digit ISO code).
        /// </summary>
        public IEnumerable<string> SupportedLanguages { get; set; }

        public virtual ValidatonResult ValidateSearchText(string searchText)
        {
            var validationResult = new ValidatonResult();

            var regexValidation = new Regex("^[A-Za-z0-9]*$").Match(searchText);
            validationResult.IsValid = regexValidation.Success;
            validationResult.Errors = validationResult.IsValid ? null : new List<string>() { ErrorMessages.InvalidSearchText };

            return validationResult;
        }

        public virtual ValidatonResult ValidateLanguageCode(string languageCode)
        {
            var validationResult = new ValidatonResult();

            validationResult.IsValid = SupportedLanguages.Contains(languageCode, StringComparer.OrdinalIgnoreCase);
            validationResult.Errors = validationResult.IsValid ? null : new List<string>() { String.Format(ErrorMessages.InvalidLanguageCode, languageCode) };

            return validationResult;
        }

        /// <summary>
        /// Builds the HTTPClient object and makes the web request to the API. Different APIs use different endpoints and have different ways of setting up authentication for the services (i.e., basic auth, simple headers, etc).
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="targetLanguage">String containing the ISO code for the language (ex: "en-us", "en-gb").</param>
        /// <returns></returns>
        public abstract Task<HttpResponseMessage> MakeRequestToAPIAsync(string searchText, string targetLanguage);
    }

    /// <summary>
    /// Minimalistic object holding information related to the API request made to the dicionary providers of FlashMEMO, and the results obtained by said call.
    /// </summary>
    public class DictionaryAPIDTO : ILexicalAPIDTO<List<DictionaryAPIResult>>
    {
        public string SearchText { get; set; }
        public string LanguageCode { get; set; }
        public List<DictionaryAPIResult> Results { get; set; }
    }

    /// <summary>
    /// Static class that provides functions to map dictionary API response objects to structures used by FlashMEMO, reducing the information overhead across external calls.
    /// </summary>
    public static class DictionaryAPIDTOMapper
    {
        public static DictionaryAPIDTO CreateDTO(IDictionaryAPIResponse dictionaryAPIResponse)
        {
            var dto = new DictionaryAPIDTO();

            switch (dictionaryAPIResponse)
            {
                case LexicalaAPIResponseModel lexicalaResponse:

                    if (lexicalaResponse.Results.Count == 0) return null; // API's way to  

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
    /// Class representing the FlashMEMO (minimalistic) representation of a individual result of a dictionary API request. There is no interface for this implementation since the current implementations (Lexicala and Oxford) share the exact same properties between themselves (DRY principle).
    /// </summary>
    public class DictionaryAPIResult
    {
        public string LexicalCategory { get; set; }
        public string PronunciationFile { get; set; }
        public string PhoneticSpelling { get; set; }
        public List<string> Definitions { get; set; }
        public List<string> Examples { get; set; }
    }

    /// <summary>
    /// Standard service used to contact the Dictionary APIs used by FlashMEMO.
    /// </summary>
    /// <typeparam name="TDictionaryAPIResponse">A DictionaryAPIResponse class is injected as a template argument during the deserialization process.</typeparam>
    public class DictionaryAPIService<TDictionaryAPIResponse> : IDictionaryAPIService<TDictionaryAPIResponse>
        where TDictionaryAPIResponse : IDictionaryAPIResponse
    {
        private readonly GenericDictionaryAPIRequestHandler _requestHandler;

        public DictionaryAPIService(IOptions<GenericDictionaryAPIRequestHandler> requestHandlerConfig)
        {
            _requestHandler = requestHandlerConfig.Value;
        }

        public HttpResponse CheckAvailability()
        {
            throw new NotImplementedException();
        }

        public HttpResponse CheckPeriodComsumption()
        {
            throw new NotImplementedException();
        }

        public async Task<DictionaryAPIDTO> SearchResults(string searchText, string targetLanguage)
        {
            using (var response = await _requestHandler.MakeRequestToAPIAsync(searchText, targetLanguage))
            {
                var parsedResponse = JsonConvert.DeserializeObject<TDictionaryAPIResponse>(await response.Content.ReadAsStringAsync());

                if (parsedResponse.HasAnyResults()) return DictionaryAPIDTOMapper.CreateDTO(parsedResponse);
                else
                    return new DictionaryAPIDTO()
                    {
                        SearchText = searchText,
                        LanguageCode = targetLanguage,
                        Results = new List<DictionaryAPIResult>()
                    };
            }
        }
    }
    #endregion

    #region AUDIO API
    public class AudioAPIResult : IAudioAPIResult
    {
        public List<string> AudioLinks { get; set; } = new List<string>();
        public string ProcessingTime { get; set; } = "";
    }

    public class AudioAPIDTO : ILexicalAPIDTO<IAudioAPIResult>
    {
        public string SearchText { get; set; } = "";
        public string LanguageCode { get; set; } = "";
        public IAudioAPIResult Results { get; set; } = new AudioAPIResult();
    }

    public class AudioAPIService : IAudioAPIService
    {
        public HttpResponse CheckAvailability()
        {
            throw new NotImplementedException();
        }

        public HttpResponse CheckPeriodComsumption()
        {
            throw new NotImplementedException();
        }

        public async Task<ILexicalAPIDTO<IAudioAPIResult>> SearchAudioAsync(string keyword, string languageCode, AudioAPIProviderType provider)
        {
            switch (provider)
            {
                case AudioAPIProviderType.REDACTED:
                    try
                    {
                        // Whatever the hell this is, this happened thanks to these resources: https://www.youtube.com/watch?v=m3Hgu2CW_Co and https://github.com/executeautomation/Selenium4NetCore/blob/master/Selenium4NetCoreProj/UnitTest1.cs.

                        // probably will put this inside an individual AudioAPIProvider class in the future. During concept design, this is acceptable
                        var audioLinks = new List<string>();

                        var chromeOptions = new ChromeOptions();
                        chromeOptions.AddArguments("--headless");

                        var driver = new ChromeDriver(chromeOptions);

                        var devTools = driver as IDevTools;
                        var session = devTools.GetDevToolsSession();

                        var devToolsSession = session.GetVersionSpecificDomains<DevToolsSessionDomains>();
                        await devToolsSession.Network.Enable(new Network.EnableCommandSettings());

                        devToolsSession.Network.ResponseReceived += (object sender, Network.ResponseReceivedEventArgs args) =>
                        {
                            if (args.Type == Network.ResourceType.Media)
                            {
                                audioLinks.Add($"{args.Response.Url}");
                            }
                        };

                        driver.Url = $"https://forvo.com/word/{keyword}/#{languageCode}/";

                        var pronunciations = driver.FindElement(OpenQA.Selenium.By.ClassName("show-all-pronunciations")).FindElements(OpenQA.Selenium.By.ClassName("play"));
                        foreach (var item in pronunciations)
                        {
                            item.Click();
                        }

                        var timer = Stopwatch.StartNew();
                        // this is implementation is probably very wrong... but it works, for now. What I mean with 'it works': waits until array reaches pre-determined state, without waiting the full timeout period, if possible.
                        bool spinUntil = SpinWait.SpinUntil(() => audioLinks.Count == pronunciations.Count, TimeSpan.FromSeconds(15));
                        timer.Stop();

                        return new AudioAPIDTO() { SearchText = keyword, LanguageCode = languageCode, Results = { AudioLinks = audioLinks, ProcessingTime = timer.Elapsed.ToString() } };
                    }
                    catch (NoSuchElementException) // this should mean that no results were found
                    {
                        return new AudioAPIDTO() { SearchText = keyword, LanguageCode = languageCode, Results = { AudioLinks = { }, ProcessingTime = "0" } };
                    }

                default:
                    throw new Exception($"Audio API provider '{provider}' does not exist.");
            }
        }
    }
    #endregion
}
