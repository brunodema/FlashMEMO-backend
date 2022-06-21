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
        public async override Task<IActionResult> Create(UserDTO entityDTO)
        {
            try
            {
                var brandNewId = await AttemptEntityCreation(entityDTO);
                await _userService.AddInitialPasswordToUser(await _userService.GetbyIdAsync(brandNewId), entityDTO.Password); // extra step

                if (brandNewId != null) return Ok(new DataResponseModel<string> { Message = $"object created successfully.", Data = brandNewId });
                return BadRequest(new BaseResponseModel {Message = $"Object was not able to be created within the database." });
            }
            catch (EntityValidationException ex)
            {
                return BadRequest(new BaseResponseModel { Message = $"Validation errors occured when creating object.", Errors = ex.ServiceValidationErrors });
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
        private readonly NewsService _newsService;
        private readonly UserService _userService;

        public NewsController(NewsService newsService, UserService userService) : base(newsService)
        {
            _newsService = newsService;
            _userService = userService;
        }

        [HttpGet]
        [Route("search/extended")]
        public IActionResult SearchExtendedNewsInfo([FromQuery] NewsFilterOptions filterOptions, [FromQuery] NewsSortOptions sortOptions = null, int pageSize = GenericRepositoryControllerDefaults.DefaultPageSize, int pageNumber = GenericRepositoryControllerDefaults.DefaultPageNumber)
        {
            var actionResult =  base.Search(filterOptions, sortOptions, pageSize, pageNumber) as ObjectResult;
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
                    FlashcardCount = flashcardsFromDeck.Count,
                    DueFlashcardCount = flashcardsFromDeck.Where(flashcard => flashcard.DueDate <= DateTime.Now).Count()
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

        public AuthController(IJWTService JWTService, IAuthService<string> authService) : base()
        {
            _JWTService = JWTService;
            _authService = authService;
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginRequestModel model)
        {
            ICollection<UserRole> roles = new List<UserRole>();

            var authenticatedUser = await _authService.AreCredentialsValidAsync(new FlashMEMOCredentials { Username = model.Username, Password = model.Password });
            if (authenticatedUser is not null)
            {
                var token = _JWTService.CreateAccessToken(authenticatedUser);

                return Ok(new LoginResponseModel { Message = "User has logged in", JWTToken = token.EncodedPayload });
            }

            return Unauthorized(new LoginResponseModel { Message = "The provided credentials could not be validated" });
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
