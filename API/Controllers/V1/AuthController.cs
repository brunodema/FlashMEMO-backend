using API.ViewModels;
using Business.Services.Interfaces;
using Business.Tools;
using Data.Models;
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
