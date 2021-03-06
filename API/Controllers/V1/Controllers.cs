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
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
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
    public class HealthController : ControllerBase
    {
        public HealthController() : base() { }

        [HttpGet]
        [Route("{ping}")]
        [AllowAnonymous]
        public IActionResult Ping()
        {
            return Ok(new BaseResponseModel() { Message = "pong :)" });
        }
    }

    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class RoleController : GenericRepositoryController<Role, string, RoleDTO, RoleFilterOptions, RoleSortOptions>
    {
        private readonly RoleService _roleService;

        public RoleController(RoleService service, ILogger<RoleController> logger) : base(service, logger)
        {
            _roleService = service;
        }
    }

    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UserController : GenericRepositoryController<User, string, UserDTO, UserFilterOptions, UserSortOptions>
    {
        private readonly UserService _userService;

        public UserController(UserService service, ILogger<UserController> logger) : base(service, logger)
        {
            _userService = service;
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
        private readonly LanguageService _languageService;

        public LanguageController(LanguageService service, ILogger<LanguageController> logger) : base(service, logger)
        {
            _languageService = service;
        }
    }

    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class NewsController : GenericRepositoryController<News, Guid, NewsDTO, NewsFilterOptions, NewsSortOptions>
    {
        private readonly UserService _userService;

        public NewsController(NewsService newsService, UserService userService, ILogger<NewsController> logger) : base(newsService, logger)
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

        public DeckController(DeckService deckService, FlashcardService flashcardService, ILogger<DeckController> logger) : base(deckService, logger)
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

        public FlashcardController(FlashcardService service, ILogger<FlashcardController> logger) : base(service, logger)
        {
            _flashcardService = service;
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
        private readonly ICachingService _cachingService;

        public ImageAPIController(CustomSearchAPIService service, ICachingService cachingService)
        {
            _service = service;
            _cachingService = cachingService;
        }

        [HttpGet]
        [Route("search")]
        public async Task<IActionResult> Search(string searchText, long pageNumber = 1)
        {
            try
            {
                var searchHash = $"image_{HashCode.Combine(searchText, pageNumber)}";
                var cacheResult = await _cachingService.GetAsync<CustomSearchAPIResponse>(searchHash);
                if (cacheResult != null)
                {
                    return Ok(new LargePaginatedListResponse<CustomSearchAPIImageResult>
                    {
                        Message = "API results successfully retrieved.",
                        Data = new LargePaginatedList<CustomSearchAPIImageResult>()
                        {
                            Results = cacheResult.Results.ToList(),
                            ResultSize = cacheResult.PageSize,
                            PageNumber = cacheResult.PageNumber,
                            TotalPages = cacheResult.TotalPages,
                            TotalAmount = cacheResult.TotalAmount,
                            HasPreviousPage = cacheResult.HasPreviousPage,
                            HasNextPage = cacheResult.HasNextPage,
                        }
                    });
                }

                var results = await _service.Search(searchText, pageNumber);
                await _cachingService.SetAsync(searchHash, results);
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
        private readonly JWTService _JWTService;

        public EmailController(IEmailService service, UserService userService, JWTService JWTService)
        {
            _emailService = service;
            _userService = userService;
            _JWTService = JWTService;
        }

        [HttpGet]
        [Route("test/{id}")]
        public async Task<ActionResult> Test(string id)
        {
            var user = await _userService.GetbyIdAsync(id);
            await _emailService.SendRegistrationAsync(user, _JWTService.CreateActivationToken(user));
            return Ok();
        }
    }

    [Authorize]
    public class GenericDictionaryAPIController<TDictionaryAPIResponse> : ControllerBase
        where TDictionaryAPIResponse : IDictionaryAPIResponse
    {
        private readonly IDictionaryAPIService<TDictionaryAPIResponse> _service;
        private readonly ICachingService _cachingService;

        public GenericDictionaryAPIController(IDictionaryAPIService<TDictionaryAPIResponse> service, ICachingService cachingService)
        {
            _service = service;
            _cachingService = cachingService;
        }

        [HttpGet]
        [Route("search")]
        public async Task<IActionResult> Search(string searchText, string languageCode)
        {
            try
            {
                var searchHash = $"dictionary_{searchText}_{languageCode}";
                var cacheResult = await _cachingService.GetAsync<DictionaryAPIDTO>(searchHash);
                if (cacheResult != null)
                {
                    return Ok(new DictionaryAPIResponse() { Message = "API results successfully retrieved.", Data = cacheResult });
                }

                var results = await _service.SearchResults(searchText, languageCode);
                await _cachingService.SetAsync(searchHash, results);

                return Ok(new DictionaryAPIResponse() { Message = "API results successfully retrieved.", Data = results });
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

        public OxfordDictionaryAPIController(IOptions<OxfordDictionaryAPIRequestHandler> options, ICachingService cachingService) : base(new DictionaryAPIService<OxfordAPIResponseModel>(options), cachingService)
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

        public LexicalaDictionaryAPIController(IOptions<LexicalaDictionaryAPIRequestHandler> options, ICachingService cachingService) : base(new DictionaryAPIService<LexicalaAPIResponseModel>(options), cachingService)
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
            // Login
            public static readonly string LOGIN_USER_HAS_LOGGED_IN = "User has logged in.";
            public static readonly string LOGIN_CREDENTIALS_COULD_NOT_BE_VALIDATED = "The provided credentials could not be validated.";
            public static readonly string LOGIN_ACTIVATION_PENDING = "Please confirm your email account before logging in.";
            public static readonly string LOGIN_ACCOUNT_IS_LOCKED = "The account is currently locked. Please check your email or contact the admin team.";

            // Email confirmation
            public static readonly string EMAIL_ACCOUNT_IS_ALREADY_ACTIVATED = "The account has already been activated.";
            public static readonly string EMAIL_ACCOUNT_ACTIVATION_SUCCESSFUL = "The account was successfully activated.";
            public static readonly string EMAIL_TOKEN_IS_NOT_VALID = "The token provided is not valid.";

            // Password recovery
            public static readonly string PASSWORD_REQUEST_USER_IS_NOT_UNLOCKED = "The user is not locked. This could mean he/she did not request password recovery.";

            // Password reset
            public static readonly string PASSWORD_RESET_SUCCESSFUL = "Password successfully reset.";
            public static readonly string PASSWORD_RESET_NOT_SUCCESSFUL = "Password could not be reset.";

            // Renewal
            public static readonly string RENEWAL_ACCESS_TOKEN_RENEWED = "The access token was renewed.";
            public static readonly string RENEWAL_ACCESS_TOKEN_NOT_RENEWED = "The access token was not able to be renewed.";
            public static readonly string RENEWAL_ACCES_TOKEN_STILL_VALID = "The access token is still valid. Returning current credentials.";
            public static readonly string RENEWAL_ACCESS_TOKEN_INVALID = "The access token is not expired, but is invalid.";
            public static readonly string RENEWAL_REFRESH_TOKEN_INVALID = "The refresh token is invalid. Please check if it is not expired.";
            public static readonly string RENEWAL_REFRESH_TOKEN_NULL = "The request does not contain a refresh token.";
            public static readonly string RENEWAL_UNRELATED_TOKENS = "The provided tokens are not related to each other.";

            // Registration
            public static readonly string REGISTRATION_EMAIL_ALREADY_IN_USE = "Email is already user by another user in FlashMEMO.";
            public static readonly string REGISTRATION_USERNAME_ALREADY_IN_USE = "Username is already user by another user in FlashMEMO.";
            public static readonly string REGISTRATION_NOT_POSSIBLE_REGISTER_USER = "It was not possible to register the new user.";
            public static readonly string REGISTRATION_SUCCESSFUL = "User registered successfully.";

            // General use
            public static readonly string GENERAL_ACTIVATION_PENDING = "The email confirmation is still pending for the user.";
            public static readonly string GENERAL_ACCOUNT_LOCKED = "The account is currently locked.";
            public static readonly string GENERAL_REQUEST_PROCESSED = "Your request was successfully processed.";
            public static readonly string GENERAL_INVALID_TOKEN = "The token is not valid.";
            public static readonly string GENERAL_USER_NOT_FOUND = "The user was not found within FlashMEMO.";

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
                return BadRequest(new BaseResponseModel() { Message = ResponseMessages.REGISTRATION_EMAIL_ALREADY_IN_USE });
            }
            if (await _authService.IsUsernameAlreadyRegistered(model.Username))
            {
                return BadRequest(new BaseResponseModel() { Message = ResponseMessages.REGISTRATION_USERNAME_ALREADY_IN_USE });
            }

            var user = new User();
            model.PassValuesToEntity(user);

            var brandNewId = await _authService.CreateUserAsync(user, model.Password, false);
            if (brandNewId == null)
            {
                return BadRequest(new BaseResponseModel() { Message = ResponseMessages.REGISTRATION_NOT_POSSIBLE_REGISTER_USER });
            }

            await this._emailService.SendRegistrationAsync(user, _JWTService.CreateActivationToken(user));
            return Ok(new BaseResponseModel { Message = ResponseMessages.REGISTRATION_SUCCESSFUL });
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginRequestModel model)
        {
            ICollection<UserRole> roles = new List<UserRole>();

            var authenticatedUser = await _authService.AreCredentialsValidAsync(new FlashMEMOCredentials { Username = model.Username, Password = model.Password }); // The fact that this method returns a user make absolutely no fucking sense...

            if (authenticatedUser is not null)
            {
                // I have to use 'StatusCode' here, because the 'Forbid' method does not accept messages. When I eventually remove these messages from the responses, I could go back to 'Forbid' 
                if (!authenticatedUser.EmailConfirmed) return StatusCode(403, new BaseResponseModel() { Message = ResponseMessages.LOGIN_ACTIVATION_PENDING });

                if (_authService.IsUserLocked(authenticatedUser)) return StatusCode(403, new BaseResponseModel() { Message = ResponseMessages.LOGIN_ACCOUNT_IS_LOCKED });

                var accessToken = _JWTService.CreateAccessToken(authenticatedUser);
                var refreshToken = _JWTService.CreateRefreshToken(accessToken, authenticatedUser);

                await _authService.UpdateLastLoginAsync(authenticatedUser);
                return Ok(new LoginResponseModel { Message = ResponseMessages.LOGIN_USER_HAS_LOGGED_IN, AccessToken = accessToken, RefreshToken = refreshToken });
            }

            return Unauthorized(new LoginResponseModel { Message = ResponseMessages.LOGIN_CREDENTIALS_COULD_NOT_BE_VALIDATED });
        }

        [HttpPost]
        [Route("activate")]
        public async Task<IActionResult> ActivateAccount([FromBody] string activationToken)
        {
            var validationResult = await _JWTService.ValidateTokenAsync(activationToken);

            if (validationResult.IsValid)
            {
                var user = await _userService.GetbyIdAsync(_JWTService.DecodeToken(activationToken).Subject);

                if (user != null)
                {
                    if (user.EmailConfirmed == true) return BadRequest(new BaseResponseModel() { Message = ResponseMessages.EMAIL_ACCOUNT_IS_ALREADY_ACTIVATED });

                    user.EmailConfirmed = true;
                    await _userService.UpdateAsync(user);

                    return Ok(new BaseResponseModel() { Message = ResponseMessages.EMAIL_ACCOUNT_ACTIVATION_SUCCESSFUL });
                }
            }

            return BadRequest(new BaseResponseModel() { Message = ResponseMessages.EMAIL_TOKEN_IS_NOT_VALID });
        }

        [HttpPost]
        [Route("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] string email)
        {
            var user = await _userService.GetByEmailAsync(email);

            if (user != null)
            {
                if (!user.EmailConfirmed) return StatusCode(403, new BaseResponseModel() { Message = ResponseMessages.GENERAL_ACTIVATION_PENDING });

                await _emailService.SendPasswordRecoveryAsync(user, await _authService.GeneratePasswordResetTokenAsync(user));
            }

            // As can be seen here, the response will always be successful, regardless if the email is valid or not. This is to avoid people from "fishing" emails from the API.
            return Ok(new BaseResponseModel() { Message = ResponseMessages.GENERAL_REQUEST_PROCESSED });
        }

        [HttpPost]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestModel resetRequest)
        {
            var user = await _userService.GetByUserNameAsync(resetRequest.Username);
            if (user != null)
            {
                if (!user.EmailConfirmed) StatusCode(403, new BaseResponseModel() { Message = ResponseMessages.GENERAL_ACTIVATION_PENDING });
                if (_authService.IsUserLocked(user)) StatusCode(403, new BaseResponseModel() { Message = ResponseMessages.GENERAL_ACCOUNT_LOCKED });

                var succeeded = await _authService.ResetPasswordAsync(user, resetRequest.Token, resetRequest.NewPassword);

                if (succeeded) return Ok(new BaseResponseModel() { Message = ResponseMessages.PASSWORD_RESET_SUCCESSFUL });

                return BadRequest(new BaseResponseModel() { Message = ResponseMessages.PASSWORD_RESET_NOT_SUCCESSFUL });
            }

            return BadRequest(new BaseResponseModel() { Message = ResponseMessages.GENERAL_USER_NOT_FOUND });
        }

        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequestModel refreshRequest)
        {
            // I'm checking this first to ensure that a legit 'refresh' call was made, and not just some random access token was sent to the back-end.
            var refreshToken = Request.Cookies["RefreshToken"];
            if (refreshToken == null) return BadRequest(new BaseResponseModel() { Message = ResponseMessages.RENEWAL_REFRESH_TOKEN_NULL });

            if (!await _JWTService.IsTokenExpired(refreshRequest.ExpiredAccessToken))
            {
                // Must still check if the token is actually valid (maybe it's corrupted or something?)
                if ((await _JWTService.ValidateTokenAsync(refreshRequest.ExpiredAccessToken)).IsValid)
                {
                    return Ok(new LoginResponseModel() { Message = ResponseMessages.RENEWAL_ACCES_TOKEN_STILL_VALID, AccessToken = refreshRequest.ExpiredAccessToken, RefreshToken = refreshToken });
                }
                return BadRequest(new BaseResponseModel() { Message = ResponseMessages.RENEWAL_ACCESS_TOKEN_INVALID });
            }

            if (await _JWTService.IsTokenExpired(refreshRequest.ExpiredAccessToken))
            {
                if ((await _JWTService.ValidateTokenAsync(refreshToken)).IsValid)
                {
                    if (_JWTService.AreAuthTokensRelated(refreshRequest.ExpiredAccessToken, refreshToken))
                    {
                        var user = await _userService.GetbyIdAsync(_JWTService.DecodeToken(refreshRequest.ExpiredAccessToken).Subject);

                        if (!user.EmailConfirmed) return StatusCode(403, new BaseResponseModel() { Message = ResponseMessages.GENERAL_ACTIVATION_PENDING });
                        if (_authService.IsUserLocked(user)) return StatusCode(403, new BaseResponseModel() { Message = ResponseMessages.GENERAL_ACCOUNT_LOCKED });

                        var newAccessToken = _JWTService.CreateAccessToken(user);
                        var newRefreshToken = _JWTService.CreateRefreshToken(newAccessToken, user);

                        await _authService.UpdateLastLoginAsync(user);
                        return Ok(new LoginResponseModel() { Message = ResponseMessages.RENEWAL_ACCESS_TOKEN_RENEWED, AccessToken = newAccessToken, RefreshToken = newRefreshToken });
                    }
                    return BadRequest(new BaseResponseModel() { Message = ResponseMessages.RENEWAL_UNRELATED_TOKENS });
                }
                return BadRequest(new BaseResponseModel() { Message = ResponseMessages.RENEWAL_REFRESH_TOKEN_INVALID });
            }
            return BadRequest(new BaseResponseModel() { Message = ResponseMessages.RENEWAL_REFRESH_TOKEN_INVALID });
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
    [Route("api/v{version:apiVersion}/audio/flashmemo")]
    [Authorize]
    public class RedactedAPIController : ControllerBase
    {
        private readonly IAudioAPIService _audioAPIService;
        private readonly ICachingService _cachingService;

        public RedactedAPIController(IAudioAPIService audioAPIService, ICachingService cachingService) : base()
        {
            _audioAPIService = audioAPIService;
            _cachingService = cachingService;
        }

        [HttpGet]
        [Route("search")]
        public async Task<IActionResult> Search([FromQuery] AudioAPIRequestModel requestParams)
        {
            // Important: no value/type checking happens here because the 'DataAnnotations' from 'AudioAPIRequestModel' should take care of that.
            var searchHash = $"audio_{requestParams.Provider}_{requestParams.GetCacheHash()}";
            var cacheResult = await _cachingService.GetAsync<AudioAPIDTO>(searchHash);
            if (cacheResult != null)
            {
                return Ok(new DataResponseModel<AudioAPIDTO> { Message = $"{cacheResult.Results.AudioLinks.Count} results were retrieved.", Data = cacheResult });
            }

            var results = await _audioAPIService.SearchAudioAsync(requestParams.Keyword, requestParams.LanguageCode, requestParams.Provider);
            await _cachingService.SetAsync(searchHash, results);

            return Ok(new DataResponseModel<AudioAPIDTO> { Message = $"{results.Results.AudioLinks.Count} results were retrieved.", Data = results });
        }
    }

    /// <summary>
    /// Toying around with this first... hehehe
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class LoggingController : ControllerBase
    {
        private readonly ILogger<LoggingController> _logger;

        public LoggingController(ILogger<LoggingController> logger) : base()
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("relay")]
        public IActionResult RelayMessage([FromBody] LoggingRequestModel logParams)
        {
            _logger.Log<LoggingRequestModel>(logParams.LogLevel, new EventId(1, "Website Logging"), logParams, null, (log, ex) =>
            {
                return $"{log.Message} - ({log.FileName} on line {log.LineNumber}:{log.ColumnNumber}) at {log.Timestamp}. Additional info: {String.Join(";", log.Args.Select(arg => JsonConvert.SerializeObject(arg))) }.";
            });
            return Ok(new BaseResponseModel { Message = "Log message relayed successfully." });
        }
    }
}
