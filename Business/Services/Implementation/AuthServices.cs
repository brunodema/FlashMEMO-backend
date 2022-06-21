using Business.Services.Interfaces;
using Business.Tools.Interfaces;
using Data.Models.Implementation;
using Data.Repository.Implementation;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Business.Services.Implementation
{
    public class AuthServiceOptions : IAuthServiceOptions
    {

    }
    public class AuthService : IAuthService<string>
    {
        private readonly IAuthServiceOptions _options;
        private readonly UserRepository _userRepository;
        private readonly RoleRepository _roleRepository;

        public AuthService(IOptions<AuthServiceOptions> options, UserRepository userRepository, RoleRepository roleRepository)
        {
            _options = options.Value;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<User> AreCredentialsValidAsync(IFlashMEMOCredentials credentials)
        {
            var user = await _userRepository.GetByUserNameAsync(credentials.Username);
            if (user != null)
            {
                if (await _userRepository.CheckPasswordAsync(user, credentials.Password))
                    return user;
            }

            return null;
        }

        public async Task<string> CreateUserAsync(User user, string cleanPassword)
        {
            var result = await _userRepository.CreateAsync(user);
            await _userRepository.SetInitialPasswordAsync(user, cleanPassword);
            return result;
        }

        public async Task<bool> EmailAlreadyRegisteredAsync(string email)
        {
            return (await _userRepository.GetByEmailAsync(email)) != null;
        }

        public async Task<bool> UserExistsAsync(string id)
        {
            return (await _userRepository.GetByIdAsync(id)) != null;
        }

        public async Task<bool> GetUserByUserNameAndCheckCredentialsAsync(IFlashMEMOCredentials credentials)
        {
            var user = await _userRepository.GetByUserNameAsync(credentials.Username);
            return await _userRepository.CheckPasswordAsync(user, credentials.Password);
        }
    }
}
