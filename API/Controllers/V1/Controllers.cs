using API.Controllers.Abstract;
using API.Tools;
using API.ViewModels;
using Business.Services.Implementation;
using Business.Services.Interfaces;
using Business.Tools;
using Data.Models.Implementation;
using Data.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Search(string searchText, int pageSize = 10, int pageNumber = 1)
        {
            try
            {
                var results = await _service.Search(searchText, pageSize, pageNumber);

                return Ok(new PaginatedListResponse<CustomSearchAPIImageResult>
                {
                    Status = "Success",
                    Message = "API results successfully retrieved.",
                    Data = new PaginatedList<CustomSearchAPIImageResult>()
                    {
                        Results = results.Results.ToList(),
                        ResultSize = results.ResultSize,
                        PageIndex = results.PageIndex,
                        TotalAmount = results.TotalAmount,
                        TotalPages = results.TotalPages,
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
                return StatusCode(500);
                throw;
            }
        }
    }

    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class DictionaryAPIController : ControllerBase

    {
        private readonly DictionaryAPIService _service;

        public DictionaryAPIController(DictionaryAPIService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("search")]
        public IActionResult Search(string searchText)
        {
            try
            {
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500);
                throw;
            }
        }
    }

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

            if (await _authService.AreCredentialsValidAsync(new Credentials { Email = model.Email, PasswordHash = model.Password }))
            {
                var token = _JWTService.CreateLoginToken(new ApplicationUser
                {
                    Email = model.Email,
                    UserRoles = roles
                });

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
