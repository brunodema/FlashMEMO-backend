using Business.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Business.JWT
{
    public class JWTService : IJWTService
    {
        private readonly IJWTServiceOptions _options;

        public JWTService(IOptions<JWTServiceOptions> options)
        {
            _options = options.Value;
        }

        public string CreateLoginToken(IUserIdentityData userIdentityData)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, userIdentityData.User.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            foreach (var role in userIdentityData.UserRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Secret));

            return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
                issuer: _options.ValidIssuer, audience: _options.ValidAudience,
                expires: DateTime.Now.AddSeconds(Convert.ToDouble(_options.TimeToExpiration)), 
                claims: claims, 
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            ));

        }
    }
    public class JWTServiceOptions : IJWTServiceOptions
    {
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
        public double TimeToExpiration { get; set; }
        public string Secret { get; set; }
    }

    public class UserIdentityData : IUserIdentityData
    {
        public UserInfo User { get; set; }
        public IList<string> UserRoles { get; set; }
    }
}
