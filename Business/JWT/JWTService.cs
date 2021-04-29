using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Business.JWT
{
    public class JWTService
    {
        private readonly IJWTServiceOptions _config;

        public JWTService(IJWTServiceOptions config)
        {
            _config = config;
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

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Secret));

            return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
                issuer: _config.ValidIssuer, audience: _config.ValidAudience,
                expires: DateTime.Now.AddHours(Convert.ToDouble(_config.HoursToExpiration)), 
                claims: claims, 
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            ));

        }
    }

    public interface IJWTServiceOptions
    {
        string ValidIssuer { get; set; }
        string ValidAudience { get; set; }
        double HoursToExpiration { get; set; }
        string Secret { get; set; }
    }

    public class JWTServiceOptions : IJWTServiceOptions
    {
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
        public double HoursToExpiration { get; set; }
        public string Secret { get; set; }
    }

    public class UserInfo
    {
        // to be further expanded (if necessary)
        public string Email { get; set; }
    }

    public interface IUserIdentityData
    {
        UserInfo User { get; set; }
        IList<string> UserRoles { get; set; }
    }

    public class UserIdentityData : IUserIdentityData
    {
        public UserInfo User { get; set; }
        public IList<string> UserRoles { get; set; }
    }
}
