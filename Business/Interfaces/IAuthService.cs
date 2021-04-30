using Business.Services;
using Data.Models;

namespace Business.Interfaces
{
    public interface IAuthServiceOptions
    {
    }
    public interface IAuthService
    {
        public bool AreCredentialsValid(ApplicationUser user);
    }
}
