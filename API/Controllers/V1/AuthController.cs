using API.ViewModels;
using Business.Interfaces;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJWTService _JWTService;
        private readonly IAuthService _authService;

        public AuthController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IJWTService JWTService, IAuthService authService) : base()
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _JWTService = JWTService;
            _authService = authService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestModel model)
        {
            if (await _userManager.FindByEmailAsync(model.Email) != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponseModel { Status = "Error", Message = "User already exists!" });

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Email
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                var errorList = result.Errors.Select(e => e.Description);
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponseModel { Status = "Error", Message = "User creation failed! Please check user details and try again.", Errors = errorList });
            }

            return Ok(new BaseResponseModel { Status = "Success", Message = "User created successfully!" });
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
