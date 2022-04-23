using Business.Services.Abstract;
using Business.Services.Interfaces;
using Business.Tools;
using Business.Tools.Interfaces;
using Data.Models.Implementation;
using Data.Repository.Implementation;
using Microsoft.AspNetCore.Identity;
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
    public class UserService : GenericRepositoryService<ApplicationUserRepository, string, ApplicationUser>
    {
        private readonly ApplicationUserRepository _userRepository;

        public UserService(ApplicationUserRepository userRepository, IOptions<GenericRepositoryServiceOptions> serviceOptions) : base(userRepository, serviceOptions.Value)
        {
            _userRepository = userRepository;
        }

        public override ValidatonResult CheckIfEntityIsValid(ApplicationUser entity)
        {
            List<string> errors = new();

            if (_userRepository.GetByEmailAsync(entity.Email).Result != null) errors.Add("An user already exists with the provided email.");

            return new ValidatonResult() { IsValid = errors.Count == 0, Errors = errors };
        }
    }

    public class DeckService : GenericRepositoryService<DeckRepository, Guid, Deck>
    {
        private readonly LanguageService _languageService;
        private readonly IAuthService<string> _authService;

        public DeckService(DeckRepository baseRepository, LanguageService languageService, IAuthService<string> authService, IOptions<GenericRepositoryServiceOptions> serviceOptions) : base(baseRepository, serviceOptions.Value) 
        {
            _languageService = languageService;
            _authService = authService;
        }

        public override ValidatonResult CheckIfEntityIsValid(Deck entity)
        {
            List<string> errors = new();

            if (!_languageService.LanguageExists(entity.LanguageISOCode ?? null)) errors.Add("The language code provided is not valid.");
            if(!_authService.UserExistsAsync(entity.OwnerId ?? null).Result) errors.Add($"The user owner does not seem to exist within FlashMEMO.");

            return new ValidatonResult() { IsValid = errors.Count == 0, Errors = errors }; // dummy function for now
        }
    }

    public class FlashcardService : GenericRepositoryService<FlashcardRepository, Guid, Flashcard>
    {
        public FlashcardService(FlashcardRepository baseRepository, IOptions<GenericRepositoryServiceOptions> serviceOptions) : base(baseRepository, serviceOptions.Value) { }

        public override ValidatonResult CheckIfEntityIsValid(Flashcard entity)
        {
            return new ValidatonResult() { IsValid = true }; // I actually went through this one (21/04), and saw that there is no immediate need to implement complex checks for Flashcard objects. Yes, there are some properties which could profit from some extra logic (ex: 'Content' columns should match the chosen layout, 'Level' should not be negative, date checks, etc), but adding that would be distracting right now.
        }
    }

    public class NewsService : GenericRepositoryService<NewsRepository, Guid, News>
    {
        public NewsService(NewsRepository baseRepository, IOptions<GenericRepositoryServiceOptions> serviceOptions) : base(baseRepository, serviceOptions.Value) { }
        public override ValidatonResult CheckIfEntityIsValid(News entity)
        {
            bool areDatesValid = entity.CreationDate <= entity.LastUpdated;

            List<string> errors = new();
            if (!areDatesValid)
            {
                errors.Add("The last updated date must be more recent than the creation date.");
            }

            return new ValidatonResult 
            {
                IsValid = areDatesValid,
                Errors = errors
            };
        }
    }

    public class JWTServiceOptions : IJWTServiceOptions
    {
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
        public double TimeToExpiration { get; set; }
        public string Secret { get; set; }
    }

    public class JWTService : IJWTService
    {
        private readonly IJWTServiceOptions _options;

        public JWTService(IOptions<JWTServiceOptions> options)
        {
            _options = options.Value;
        }

        public string CreateLoginToken(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("username", user.UserName) // this is potentially dangerous, but showing the entire schema name on the decoded jwt looked ridiculous
            };
            foreach (var role in user.UserRoles ?? Enumerable.Empty<ApplicationUserRole>())
            {
                claims.Add(new Claim(ClaimTypes.Role, role.RoleId));
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

    public class AuthServiceOptions : IAuthServiceOptions
    {

    }
    public class AuthService : IAuthService<string>
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

        public async Task<ApplicationUser> AreCredentialsValidAsync(IFlashMEMOCredentials credentials)
        {
            if (await EmailAlreadyRegisteredAsync(credentials.Email))
            {
                if (await GetUserByEmailAndCheckCredentialsAsync(credentials)) return _applicationUserRepository.GetAll().SingleOrDefault(u => u.Email == credentials.Email); // this is horrible design, only doing this instead of creating the appropriate function in the repository class to speed up development on the front-end
            }
            return null;
        }

        public async Task<string> CreateUserAsync(ApplicationUser user, string cleanPassword)
        {
            var result = await _applicationUserRepository.CreateAsync(user);
            await _applicationUserRepository.SetInitialPasswordAsync(user, cleanPassword);
            return result;
        }

        public async Task<bool> EmailAlreadyRegisteredAsync(string email)
        {
            return (await _applicationUserRepository.GetByEmailAsync(email)) != null;
        }

        public async Task<bool> UserExistsAsync(string id)
        {
            return (await _applicationUserRepository.GetByIdAsync(id)) != null;
        }

        public async Task<bool> GetUserByEmailAndCheckCredentialsAsync(IFlashMEMOCredentials credentials)
        {
            var user = await _applicationUserRepository.GetByEmailAsync(credentials.Email);
            return await _applicationUserRepository.CheckPasswordAsync(user, credentials.PasswordHash);
        }
    }

    #region Language Service
    /// <summary>
    /// Service containing auxiliary functions related to languages within FlashMEMO.
    /// </summary>
    public class LanguageService : ILanguageService
    {
        private readonly LanguageRepository _languageRepository;

        public LanguageService(LanguageRepository languageRepository)
        {
            _languageRepository = languageRepository;
        }

        public bool LanguageExists(string languageCode)
        {
            return _languageRepository.GetAll().Any(l => l.ISOCode == languageCode);
        }
    }
    #endregion
}
