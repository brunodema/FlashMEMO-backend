using API.Controllers.Abstract;
using API.Tools;
using API.ViewModels;
using Business.Services.Implementation;
using Business.Services.Interfaces;
using Business.Tools;
using Business.Tools.DictionaryAPI.Lexicala;
using Business.Tools.DictionaryAPI.Oxford;
using Business.Tools.Exceptions;
using Data.Models.Implementation;
using Data.Tools.Filtering;
using Data.Tools.Sorting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class NewsController : GenericRepositoryController<News, Guid, NewsFilterOptions, NewsSortOptions>
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
                        PageIndex = results.PageIndex,
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
                throw;
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
            catch(InputValidationException e)
            {
                return BadRequest(new BaseResponseModel { Status = "Bad Request", Message = e.Message, Errors= e.InputValidationErrors });
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
        private readonly IAuthService _authService;

        public AuthController(IJWTService JWTService, IAuthService authService) : base()
        {
            _JWTService = JWTService;
            _authService = authService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestModel model)
        {
            if (await _authService.UserAlreadyExistsAsync(model.Email))
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponseModel { Status = "Error", Message = "Email already exists in the database. Please use an unique email for registration, or contact one of our administrator to recover your password/account." });
            }

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Email,
            };

            var result = await _authService.CreateUserAsync(user, model.Password);
            if (!result.Succeeded)
            {
                var errorList = result.Errors.Select(e => e.Description);
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponseModel { Status = "Error", Message = "User creation failed. Please check user details and try again.", Errors = errorList });
            }

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
            return Ok(new BaseResponseModel { Status = "Sucess", Message = "You managed to get here!" });
        }
    }
}
