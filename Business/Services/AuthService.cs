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
        private readonly ApplicationUserRepository _repository;

        public AuthService(IOptions<AuthServiceOptions> options, ApplicationUserRepository repository)
        {
            _options = options.Value;
            _repository = repository;
        }

        public async Task<bool> AreCredentialsValidAsync(ICredentials credentials)
        {
            var user = await _repository.FindFirstAsync(u => u.Email == credentials.Email);
            if (user != null)
            {
                if (new PasswordHasher<ApplicationUser>().VerifyHashedPassword(user, user.PasswordHash, credentials.PasswordHash) == PasswordVerificationResult.Success) return true;
            }

            return false;
        }
    }
}