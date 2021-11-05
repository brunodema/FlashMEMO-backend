using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Business.Services.Interfaces;
using Business.Tools.Interfaces;
using Data.Repository.Implementation;
using Data.Models.Implementation;

namespace Business.Services.Implementation
{
    public class AuthServiceOptions : IAuthServiceOptions
    {

    }
    public class AuthService : IAuthService
    {
        private readonly IAuthServiceOptions _options;
        private readonly ApplicationUserRepository _applicationUserRepository;
        private readonly RoleRepository _roleRepository;

        public AuthService(IOptions<AuthServiceOptions> options, ApplicationUserRepository applicationUserRepository, RoleRepository roleRepository)
        {
            _options = options.Value;
            _applicationUserRepository = applicationUserRepository;
            _roleRepository = roleRepository;
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
            var result = await _applicationUserRepository.CreateAsync(user);
            await _applicationUserRepository.SetInitialPassword(user.Id, cleanPassword);
            return result;
        }

        public async Task<bool> UserAlreadyExistsAsync(string email)
        {
            return (await _applicationUserRepository.SearchFirstAsync(u => u.Email == email)) != null;
        }
        public async Task<bool> GetUserByEmailAndCheckCredentialsAsync(ICredentials credentials)
        {
            var user = await _applicationUserRepository.SearchFirstAsync(u => u.Email == credentials.Email);
            return await _applicationUserRepository.CheckPasswordAsync(user.Id, credentials.PasswordHash);
        }
    }
}