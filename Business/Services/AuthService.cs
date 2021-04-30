using Business.Interfaces;
using Data.Models;
using Microsoft.Extensions.Options;

namespace Business.Services
{
    namespace Business.Services
    {
        public class AuthService : IAuthService
        {
            private readonly IAuthServiceOptions _options;
            //private readonly IUser

            //public AuthService(IOptions<IAuthServiceOptions> options)
            //{
            //    _options = options.Value;
            //}

            public bool AreCredentialsValid(ApplicationUser user)
            {
                throw new System.NotImplementedException();
            }
        }
        public class AuthServiceOptions : IAuthServiceOptions
        {

        }
    }
}
