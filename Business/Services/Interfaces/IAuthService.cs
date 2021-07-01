using Business.Tools.Interfaces;
using Data.Models.Implementation;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Business.Services.Interfaces
{
    public interface IAuthServiceOptions
    {
    }
    public interface IAuthService
    {
        public Task<bool> UserAlreadyExistsAsync(string email);
        public Task<IdentityResult> CreateUserAsync(ApplicationUser user, string cleanPassword);
        public Task<bool> AreCredentialsValidAsync(ICredentials credentials);
    }
}
