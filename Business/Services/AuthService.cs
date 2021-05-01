using Business.Interfaces;
using Data.Models;
using Data.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Business.Services
{
    public class AuthServiceOptions : IAuthServiceOptions
    {

    }
    public class AuthService : IAuthService
    {
        private readonly IAuthServiceOptions _options;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthService(IOptions<AuthServiceOptions> options, UserManager<ApplicationUser> userManager)
        {
            _options = options.Value;
            _userManager = userManager;
        }

        public async Task<bool> AreCredentialsValidAsync(ICredentials credentials)
        {
            var user = await _userManager.FindByEmailAsync(credentials.Email);
            if (user != null)
            {
                if (await _userManager.CheckPasswordAsync(user, credentials.PasswordHash)) return true;
            }

            return false;
        }
    }
}