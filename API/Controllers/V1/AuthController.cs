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

        public AuthController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IJWTService JWTService) : base()
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _JWTService = JWTService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestModel model)
        {
            var userExists = await _userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
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
        public IActionResult Login(LoginRequestModel model)
        {
            ICollection<ApplicationUserRole> roles = new List<ApplicationUserRole>();

            var token = _JWTService.CreateLoginToken(new ApplicationUser
            {
                Email = model.Email,
                UserRoles = roles
            });

            return Ok(new LoginResponseModel { Status = "Success", Message = "User has logged in", JWTToken = token });
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
