﻿using Data.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Business.Interfaces
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
