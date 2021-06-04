using Data.Repository.Interfaces;
using Data.Models;
using Data.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Business.Services.Interfaces;
using Business.Tools.Interfaces;

namespace Business.Services
{
    public class AuthServiceOptions : IAuthServiceOptions
    {

    }
    public class AuthService : IAuthService
    {
        private readonly IAuthServiceOptions _options;
        private readonly IAuthRepository _authRepository;

        public AuthService(IOptions<AuthServiceOptions> options, IAuthRepository authRepository)
        {
            _options = options.Value;
            _authRepository = authRepository;
        }

        public async Task<bool> AreCredentialsValidAsync(ICredentials credentials)
        {
            if (await UserAlreadyExistsAsync(credentials.Email))
            {
                if (await GetUserByEmailAndCheckCredentialsAsync(credentials)) return true;
            }

            return false;
        }

        public async Task<IdentityResult> CreateUserAsync(ApplicationUser user, string cleanPassword)
        {
            return await _authRepository.CreateAsync(user, cleanPassword);
        }

        public async Task<bool> UserAlreadyExistsAsync(string email)
        {
            return (await _authRepository.SearchFirstAsync(u => u.Email == email)) != null;
        }
        public async Task<bool> GetUserByEmailAndCheckCredentialsAsync(ICredentials credentials)
        {
            var user = await _authRepository.SearchFirstAsync(u => u.Email == credentials.Email);
            return await _authRepository.CheckPasswordAsync(user, credentials.PasswordHash);
        }
    }
}