using API.Controllers.Abstract;
using API.Tools;
using API.ViewModels;
using Business.Services.Implementation;
using Business.Services.Interfaces;
using Business.Tools;
using Business.Tools.DictionaryAPI.Lexicala;
using Business.Tools.DictionaryAPI.Oxford;
using Business.Tools.Exceptions;
using Data.Models.DTOs;
using Data.Models.Implementation;
using Data.Tools.Filtering;
using Data.Tools.Sorting;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Data.Models.Implementation.StaticModels;

namespace API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class RoleController : GenericRepositoryController<Role, string, RoleDTO, RoleFilterOptions, RoleSortOptions>
    {
        private readonly RoleService _roleService;

        public RoleController(RoleService roleService) : base(roleService)
        {
            _roleService = roleService;
        }
    }

    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UserController : GenericRepositoryController<User, string, UserDTO, UserFilterOptions, UserSortOptions>
    {
        private readonly UserService _userService;

        public UserController(UserService userService) : base(userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// This method ends up being pretty much a copy of the base one, just adding logic to set the initial password after creating the object.
        /// </summary>
        /// <param name="entityDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{create}")]
        [AllowAnonymous]
        public async override Task<IActionResult> Create(UserDTO entityDTO)
        {
            try
            {
                var brandNewId = await AttemptEntityCreation(entityDTO);
                await _userService.AddInitialPasswordToUser(await _userService.GetbyIdAsync(brandNewId), entityDTO.Password); // extra step

                if (brandNewId != null) return Ok(new DataResponseModel<string> { Message = $"User created successfully.", Data = brandNewId });
                return BadRequest(new BaseResponseModel { Message = $"User was not able to be created within the database." });
            }
            catch (EntityValidationException ex)
            {
                return BadRequest(new BaseResponseModel { Message = $"Validation errors occured when creating user.", Errors = ex.ServiceValidationErrors });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut]
        [Route("{id}")]
        public async override Task<IActionResult> Update(string id, [CustomizeValidator(Properties = "Email, Username, Name, Surname")] UserDTO entityDTO)
        {
            if (!String.IsNullOrEmpty(entityDTO.Password))
            {
                var passwordValidatorResult = new UserDTOValidator().Validate(entityDTO);
                if (!passwordValidatorResult.IsValid)
                {
                    return BadRequest(new BaseResponseModel() { Message = "Validation Errors have occurred while processing your request.", Errors = passwordValidatorResult.Errors.Select(x => x.ErrorMessage) });
                }
            }

            var actionResult = await base.Update(id, entityDTO) as ObjectResult;
            if (actionResult.StatusCode == 200)
            {
                var response = (DataResponseModel<string>)actionResult.Value;

                return Ok(new DataResponseModel<string>() { Message = "User updated successfully.", Data = id });
            }

            return actionResult;
        }

        [HttpGet]
        [Route("{id}")]
        [AllowAnonymous]
        public async override Task<IActionResult> Get(string id)
        {
            var actionResult = await base.Get(id) as ObjectResult;
            if (actionResult.StatusCode == 200)
            {
                var response = (DataResponseModel<User>)actionResult.Value;

                var user = new ReducedUserDTO(response.Data);

                return Ok(new DataResponseModel<ReducedUserDTO>() { Message = "User retrieved successfully.", Data = user });
            }

            return actionResult;
        }

        [HttpGet]
        [Route("list")]
        public override IActionResult List(int pageSize = GenericRepositoryControllerDefaults.DefaultPageSize, int pageNumber = GenericRepositoryControllerDefaults.DefaultPageNumber, [FromQuery] UserSortOptions sortOptions = null)
        {
            var actionResult = base.List(pageSize, pageNumber, sortOptions) as ObjectResult;
            if (actionResult.StatusCode == 200)
            {
                var response = (PaginatedListResponse<User>)actionResult.Value;

                var users = response.Data.Results.Select(user => new ReducedUserDTO(user)).ToList();

                return Ok(new PaginatedListResponse<ReducedUserDTO>() { Message = "User retrieved successfully.", Data = PaginatedList<ReducedUserDTO>.Create(users, pageNumber, pageSize) });
            }

            return actionResult;

        }
    }

    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class LanguageController : GenericRepositoryController<Language, string, LanguageDTO, LanguageFilterOptions, LanguageSortOptions>
    {
        private readonly LanguageService _roleService;

        public LanguageController(LanguageService roleService) : base(roleService)
        {
            _roleService = roleService;
        }
    }

    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class NewsController : GenericRepositoryController<News, Guid, NewsDTO, NewsFilterOptions, NewsSortOptions>
    {
        private readonly UserService _userService;

        public NewsController(NewsService newsService, UserService userService) : base(newsService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("list")]
        [AllowAnonymous]
        public override IActionResult List(int pageSize = 10, int pageNumber = 1, [FromQuery] NewsSortOptions sortOptions = null)
        {
            return base.List(pageSize, pageNumber, sortOptions);
        }

        [HttpGet]
        [Route("search")]
        [AllowAnonymous]
        public override IActionResult Search([FromQuery] NewsFilterOptions filterOptions, [FromQuery] NewsSortOptions sortOptions = null, int pageSize = 10, int pageNumber = 1)
        {
            return base.Search(filterOptions, sortOptions, pageSize, pageNumber);
        }

        [HttpGet]
        [Route("{id}")]
        [AllowAnonymous]
        public override Task<IActionResult> Get(Guid id)
        {
            return base.Get(id);
        }

        [HttpGet]
        [Route("search/extended")]
        [AllowAnonymous]
        public IActionResult SearchExtendedNewsInfo([FromQuery] NewsFilterOptions filterOptions, [FromQuery] NewsSortOptions sortOptions = null, int pageSize = GenericRepositoryControllerDefaults.DefaultPageSize, int pageNumber = GenericRepositoryControllerDefaults.DefaultPageNumber)
        {
            var actionResult = base.Search(filterOptions, sortOptions, pageSize, pageNumber) as ObjectResult;
            if (actionResult.StatusCode == 200)
            {
                var response = (PaginatedListResponse<News>)actionResult.Value;
                var extendedInfoList = new List<ExtendedNewsInfoDTO>();

                response.Data.Results.ToList().ForEach(news =>
                {
                    var extendedItem = new ExtendedNewsInfoDTO()
                    {
                        NewsId = news.NewsId,
                        Title = news.Title,
                        Content = news.Content,
                        CreationDate = news.CreationDate,
                        LastUpdated = news.LastUpdated,
                        OwnerId = news.OwnerId,
                        Subtitle = news.Subtitle,
                        ThumbnailPath = news.ThumbnailPath,
                        OwnerInfo = new ReducedUserDTO(_userService.GetbyIdAsync(news.OwnerId).Result)
                    };
                    extendedInfoList.Add(extendedItem);
                });

                return Ok(new PaginatedListResponse<ExtendedNewsInfoDTO>() { Message = "Extended News info successfully retrieved.", Data = new PaginatedList<ExtendedNewsInfoDTO>() { PageNumber = response.Data.PageNumber, ResultSize = response.Data.ResultSize, TotalPages = response.Data.TotalPages, TotalAmount = response.Data.TotalAmount, Results = extendedInfoList } });
            }

            return actionResult;
        }
    }

    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class DeckController : GenericRepositoryController<Deck, Guid, DeckDTO, DeckFilterOptions, DeckSortOptions>
    {
        private readonly DeckService _deckService;
        private readonly FlashcardService _flashcardService;

        public DeckController(DeckService deckService, FlashcardService flashcardService) : base(deckService)
        {
            _deckService = deckService;
            _flashcardService = flashcardService;
        }

        [HttpGet]
        [Route("list/extended")]
        public IActionResult GetExtendedDeckInfo([FromQuery] string ownerId = null)
        {
            var decks = new List<Deck>();
            if (ownerId == null) decks = _deckService.List().ToList();
            else decks = _deckService.GetAllDecksFromUser(ownerId);

            var extendedInfoList = new List<ExtendedDeckInfoDTO>();
            decks.ForEach(deck =>
            {
                var flashcardsFromDeck = _flashcardService.GetAllFlashcardsFromDeck(deck.DeckId);
                extendedInfoList.Add(new ExtendedDeckInfoDTO()
                {
                    DeckId = deck.DeckId,
                    CreationDate = deck.CreationDate,
                    Description = deck.Description,
                    LanguageISOCode = deck.LanguageISOCode,
                    Name = deck.Name,
                    OwnerId = deck.OwnerId,
                    LastUpdated = deck.LastUpdated,
                    LastStudySession = deck.LastStudySession,
                    FlashcardCount = flashcardsFromDeck.Count,
                    DueFlashcardCount = flashcardsFromDeck.Where(flashcard => flashcard.DueDate <= DateTime.Now.ToUniversalTime()).Count()
                });
            });

            return Ok(new DataResponseModel<List<ExtendedDeckInfoDTO>>() { Message = "Decks from user retrieved.", Data = extendedInfoList });
        }
    }

    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class FlashcardController : GenericRepositoryController<Flashcard, Guid, FlashcardDTO, FlashcardFilterOptions, FlashcardSortOptions>
    {
        private readonly FlashcardService _flashcardService;

        public FlashcardController(FlashcardService flashcardService) : base(flashcardService)
        {
            _flashcardService = flashcardService;
        }

        [HttpGet]
        [Route("GetAllFlashcardsFromDeck/{deckId}")]
        public IActionResult GetAllFlashcardsFromDeck(Guid deckId)
        {
            var ret = _flashcardService.GetAllFlashcardsFromDeck(deckId);

            return Ok(new DataResponseModel<List<Flashcard>>() { Message = "Flashcards retrieved successfully.", Data = ret });
        }
    }

    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class ImageAPIController : ControllerBase

    {
        private readonly CustomSearchAPIService _service;

        public ImageAPIController(CustomSearchAPIService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("search")]
        public async Task<IActionResult> Search(string searchText, long pageNumber = 1)
        {
            try
            {
                var results = await _service.Search(searchText, pageNumber);

                return Ok(new LargePaginatedListResponse<CustomSearchAPIImageResult>
                {
                    Message = "API results successfully retrieved.",
                    Data = new LargePaginatedList<CustomSearchAPIImageResult>()
                    {
                        Results = results.Results.ToList(),
                        ResultSize = results.PageSize,
                        PageNumber = results.PageNumber,
                        TotalPages = results.TotalPages,
                        TotalAmount = results.TotalAmount,
                        HasPreviousPage = results.HasPreviousPage,
                        HasNextPage = results.HasNextPage,
                    }
                });
            }
            catch (InputValidationException e)
            {
                return BadRequest(new BaseResponseModel { Message = e.Message, Errors = e.InputValidationErrors });
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly UserService _userService;

        public EmailController(IEmailService service, UserService userService)
        {
            _emailService = service;
            _userService = userService;
        }

        [HttpGet]
        [Route("test/{id}")]
        public async Task<ActionResult> Test(string id)
        {
            var user = await _userService.GetbyIdAsync(id);
            await _emailService.SendRegistrationAsync(user);
            return Ok();
        }
    }

    [Authorize]
    public class GenericDictionaryAPIController<TDictionaryAPIResponse> : ControllerBase
        where TDictionaryAPIResponse : IDictionaryAPIResponse
    {
        private readonly IDictionaryAPIService<TDictionaryAPIResponse> _service;

        public GenericDictionaryAPIController(IDictionaryAPIService<TDictionaryAPIResponse> service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("search")]
        public async Task<IActionResult> Search(string searchText, string languageCode)
        {
            try
            {
                return Ok(new DictionaryAPIResponse() { Message = "API results successfully retrieved.", Data = await _service.SearchResults(searchText, languageCode) });
            }
            catch (InputValidationException e)
            {
                return BadRequest(new BaseResponseModel { Message = e.Message, Errors = e.InputValidationErrors });
            }

            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("languages")]
        public async Task<IActionResult> GetAvailableLanguages()
        {
            return Ok(new DataResponseModel<List<Language>>() { Message = "Languages supported by the API successfully retrieved.", Data = await _service.GetAvailableLanguages() });
        }
    }

    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/dict/oxford")]
    public class OxfordDictionaryAPIController : GenericDictionaryAPIController<OxfordAPIResponseModel>

    {
        private readonly IDictionaryAPIService<OxfordAPIResponseModel> _service;

        public OxfordDictionaryAPIController(IOptions<OxfordDictionaryAPIRequestHandler> options) : base(new DictionaryAPIService<OxfordAPIResponseModel>(options))
        {
            _service = new DictionaryAPIService<OxfordAPIResponseModel>(options);
        }
    }

    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/dict/lexicala")]
    public class LexicalaDictionaryAPIController : GenericDictionaryAPIController<LexicalaAPIResponseModel>

    {
        private readonly IDictionaryAPIService<LexicalaAPIResponseModel> _service;

        public LexicalaDictionaryAPIController(IOptions<LexicalaDictionaryAPIRequestHandler> options) : base(new DictionaryAPIService<LexicalaAPIResponseModel>(options))
        {
            _service = new DictionaryAPIService<LexicalaAPIResponseModel>(options);
        }
    }

    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IJWTService _JWTService;
        private readonly IAuthService<string> _authService;
        private readonly IEmailService _emailService;
        private readonly UserService _userService;

        public static class ResponseMessages
        {
            public static readonly string USER_HAS_LOGGED_IN = "User has logged in.";
            public static readonly string CREDENTIALS_COULD_NOT_BE_VALIDATED = "The provided credentials could not be validated.";

            public static readonly string ACCESS_TOKEN_RENEWED = "The access token was renewed.";
            public static readonly string ACCESS_TOKEN_NOT_RENEWED = "The access token was not able to be renewed.";
            public static readonly string ACCES_TOKEN_STILL_VALID = "The access token is still valid. Returning current credentials.";

            public static readonly string ACCESS_TOKEN_INVALID = "The access token is not expired, but is invalid.";
            public static readonly string REFRESH_TOKEN_INVALID = "The refresh token is invalid. Please check if it is not expired.";
            public static readonly string REFRESH_TOKEN_NULL = "The request does not contain a refresh token.";
            public static readonly string UNRELATED_TOKENS = "The provided tokens are not related to each other.";
        }

        public AuthController(IJWTService JWTService, IAuthService<string> authService, IEmailService emailService, UserService userService) : base()
        {
            _JWTService = JWTService;
            _authService = authService;
            _emailService = emailService;
            _userService = userService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] UserDTO model)
        {
            if (await _authService.IsEmailAlreadyRegisteredAsync(model.Email))
            {
                return BadRequest(new BaseResponseModel() { Message = "Email is already user by another user in FlashMEMO." });
            }
            if (await _authService.IsUsernameAlreadyRegistered(model.Username))
            {
                return BadRequest(new BaseResponseModel() { Message = "Username is already user by another user in FlashMEMO." });
            }

            var user = new User();
            model.PassValuesToEntity(user);

            var brandNewId = await _authService.CreateUserAsync(user, model.Password, false);
            if (brandNewId == null)
            {
                return BadRequest(new BaseResponseModel() { Message = "It was not possible to register the new user." });
            }

            await this._emailService.SendRegistrationAsync(user);
            return Ok(new BaseResponseModel { Message = "User registered successfully." });
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginRequestModel model)
        {
            ICollection<UserRole> roles = new List<UserRole>();

            var authenticatedUser = await _authService.AreCredentialsValidAsync(new FlashMEMOCredentials { Username = model.Username, Password = model.Password }); // The fact that this method returns a user make absolutely no fucking sense...
            if (authenticatedUser is not null)
            {
                if (!authenticatedUser.EmailConfirmed) return BadRequest(new BaseResponseModel() { Message = "Please confirm your email account before logging in." });

                var accessToken = _JWTService.CreateAccessToken(authenticatedUser);
                var refreshToken = _JWTService.CreateRefreshToken(accessToken, authenticatedUser);

                await _authService.UpdateLastLoginAsync(authenticatedUser);
                return Ok(new LoginResponseModel { Message = ResponseMessages.USER_HAS_LOGGED_IN, AccessToken = accessToken, RefreshToken = refreshToken });
            }

            return Unauthorized(new LoginResponseModel { Message = ResponseMessages.CREDENTIALS_COULD_NOT_BE_VALIDATED });
        }

        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequestModel refreshRequest)
        {
            // I'm cheking this first to ensure that a legit 'refresh' call was made, and not just some random access token was sent to the back-end.
            var refreshToken = Request.Cookies["RefreshToken"];
            if (refreshToken == null) return BadRequest(new BaseResponseModel() { Message = ResponseMessages.REFRESH_TOKEN_NULL });

            if (!await _JWTService.IsTokenExpired(refreshRequest.ExpiredAccessToken))
            {
                // Must still check if the token is actually valid (maybe it's corrupted or something?)
                if ((await _JWTService.ValidateTokenAsync(refreshRequest.ExpiredAccessToken)).IsValid)
                {
                    return Ok(new LoginResponseModel() { Message = ResponseMessages.ACCES_TOKEN_STILL_VALID, AccessToken = refreshRequest.ExpiredAccessToken, RefreshToken = refreshToken });
                }
                return BadRequest(new BaseResponseModel() { Message = ResponseMessages.ACCESS_TOKEN_INVALID });
            }

            if (await _JWTService.IsTokenExpired(refreshRequest.ExpiredAccessToken))
            {
                if ((await _JWTService.ValidateTokenAsync(refreshToken)).IsValid)
                {
                    if (_JWTService.AreAuthTokensRelated(refreshRequest.ExpiredAccessToken, refreshToken))
                    {
                        var user = await _userService.GetbyIdAsync(_JWTService.DecodeToken(refreshRequest.ExpiredAccessToken).Subject);
                        var newAccessToken = _JWTService.CreateAccessToken(user);
                        var newRefreshToken = _JWTService.CreateRefreshToken(newAccessToken, user);

                        await _authService.UpdateLastLoginAsync(user);
                        return Ok(new LoginResponseModel() { Message = ResponseMessages.ACCESS_TOKEN_RENEWED, AccessToken = newAccessToken, RefreshToken = newRefreshToken });
                    }
                    return BadRequest(new BaseResponseModel() { Message = ResponseMessages.UNRELATED_TOKENS });
                }
                return BadRequest(new BaseResponseModel() { Message = ResponseMessages.REFRESH_TOKEN_INVALID });
            }
            return BadRequest(new BaseResponseModel() { Message = ResponseMessages.REFRESH_TOKEN_INVALID });
        }

        [HttpGet]
        [Authorize]
        [Route("test")]
        public IActionResult Test()
        {
            return Ok(new BaseResponseModel { Message = "You managed to get here!" });
        }

        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        [Route("test/admin")]
        public IActionResult AdminTest()
        {
            return Ok(new BaseResponseModel { Message = "You managed to get here! And you are an Admin! :O" });
        }
    }

    /// <summary>
    /// Toying around with this first... hehehe
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class RedactedAPIController : ControllerBase
    {
        private readonly IAudioAPIService _audioAPIService;

        public RedactedAPIController(IAudioAPIService audioAPIService) : base()
        {
            _audioAPIService = audioAPIService;
        }

        [HttpGet]
        [Route("search")]
        public async Task<IActionResult> Search(string keyword, string languageCode, AudioAPIProviderType provider = AudioAPIProviderType.REDACTED)
        {
            var results = await _audioAPIService.SearchAudioAsync(keyword, languageCode, provider);

            return Ok(new DataResponseModel<ILexicalAPIDTO<IAudioAPIResult>> { Message = $"{results.Results.AudioLinks.Count} results were retrieved.", Data = results });

        }
    }
}
