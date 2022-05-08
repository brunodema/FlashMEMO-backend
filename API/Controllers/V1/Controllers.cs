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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using DevToolsSessionDomains = OpenQA.Selenium.DevTools.V96.DevToolsSessionDomains;
using Network = OpenQA.Selenium.DevTools.V96.Network;

namespace API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class RoleController : GenericRepositoryController<ApplicationRole, string, RoleDTO, RoleFilterOptions, RoleSortOptions>
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
    public class UserController : GenericRepositoryController<ApplicationUser, string, UserDTO, UserFilterOptions, UserSortOptions>
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

                if (brandNewId != null) return Ok(new DataResponseModel<string> { Status = "Success", Message = $"object created successfully.", Data = brandNewId });
                return BadRequest(new BaseResponseModel { Status = "Bad Request", Message = $"Object was not able to be created within the database." });
            }
            catch (EntityValidationException ex)
            {
                return BadRequest(new BaseResponseModel { Status = "Error", Message = $"Validation errors occured when creating object.", Errors = ex.ServiceValidationErrors });
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
    public class NewsController : GenericRepositoryController<News, Guid, NewsDTO, NewsFilterOptions, NewsSortOptions>
    {
        private readonly NewsService _newsService;

        public NewsController(NewsService newsService) : base(newsService)
        {
            _newsService = newsService;
        }
    }

    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class DeckController : GenericRepositoryController<Deck, Guid, DeckDTO, DeckFilterOptions, DeckSortOptions>
    {
        private readonly DeckService _deckService;

        public DeckController(DeckService deckService) : base(deckService)
        {
            _deckService = deckService;
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
                    Status = "Success",
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
                return BadRequest(new BaseResponseModel { Status = "Bad Request", Message = e.Message, Errors = e.InputValidationErrors });
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
                return Ok(new DictionaryAPIResponse() { Status = "Success", Message = "API results successfully retrieved.", Data = await _service.SearchResults(searchText, languageCode) });
            }
            catch (InputValidationException e)
            {
                return BadRequest(new BaseResponseModel { Status = "Bad Request", Message = e.Message, Errors = e.InputValidationErrors });
            }

            catch (Exception)
            {
                throw;
            }
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
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                var errorList = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new BaseResponseModel { Status = "Bad Request", Message = "User creation failed. Please check user details and try again.", Errors = errorList });
            }

            if (await _authService.EmailAlreadyRegisteredAsync(model.Email))
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponseModel { Status = "Error", Message = "Email already exists in the database. Please use an unique email for registration, or contact one of our administrator to recover your password/account." });
            }

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Email,
            };

            var brandNewId = await _authService.CreateUserAsync(user, model.Password);
            return Ok(new BaseResponseModel { Status = "Success", Message = "User created successfully." });
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginRequestModel model)
        {
            ICollection<ApplicationUserRole> roles = new List<ApplicationUserRole>();

            var authenticatedUser = await _authService.AreCredentialsValidAsync(new FlashMEMOCredentials { Email = model.Email, PasswordHash = model.Password });
            if (authenticatedUser is not null)
            {
                var token = _JWTService.CreateLoginToken(authenticatedUser);

                return Ok(new LoginResponseModel { Status = "Success", Message = "User has logged in", JWTToken = token });
            }

            return Unauthorized(new LoginResponseModel { Status = "Error", Message = "The provided credentials could not be validated" });
        }

        [HttpGet]
        [Authorize]
        [Route("test")]
        public IActionResult Test()
        {
            return Ok(new BaseResponseModel { Status = "Success", Message = "You managed to get here!" });
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

            return Ok(new DataResponseModel<ILexicalAPIDTO<IAudioAPIResult>> { Status = "Success", Message = $"{results.Results.AudioLinks.Count} results were retrieved.", Data = results });

        }
    }
}
