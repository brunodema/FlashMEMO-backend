using Business.Services.Interfaces;
using Business.Tools.Interfaces;
using Data.Models.Implementation;
using Data.Repository.Implementation;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
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

    public class JWTService : IJWTService
    {
        private readonly IJWTServiceOptions _options;

        public JWTService(IOptions<JWTServiceOptions> options)
        {
            _options = options.Value;
        }

        public JwtSecurityToken CreateAccessToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim("username", user.UserName),
                new Claim(JwtRegisteredClaimNames.Name, user.Name),
                new Claim("surname", user.Surname),
            };
            foreach (var role in user.UserRoles ?? Enumerable.Empty<UserRole>())
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Role.Name));
            }

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Secret));

            return new JwtSecurityToken(
                issuer: _options.ValidIssuer,
                audience: _options.ValidAudience,
                expires: DateTime.Now.AddSeconds(Convert.ToDouble(_options.AccessTokenTTE)),
                claims: claims,
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            );
        }

        public JwtSecurityToken CreateRefreshToken(JwtSecurityToken accessToken, User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            };

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Secret));

            return new JwtSecurityToken(
                issuer: _options.ValidIssuer,
                audience: _options.ValidAudience,
                expires: DateTime.Now.AddSeconds(Convert.ToDouble(_options.RefreshTokenTTE)),
                claims: claims,
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            );
        }
    }
}
