using Business.Services.Abstract;
using Business.Services.Interfaces;
using Business.Tools;
using Business.Tools.Interfaces;
using Business.Tools.Validations;
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
using static Data.Models.Implementation.StaticModels;

namespace Business.Services.Implementation
{
    public class RoleService : GenericRepositoryService<RoleRepository, string, ApplicationRole>
    {

        public RoleService(RoleRepository roleRepository, IOptions<GenericRepositoryServiceOptions> serviceOptions) : base(roleRepository, serviceOptions.Value) { }

        public override ValidatonResult CheckIfEntityIsValid(ApplicationRole entity)
        {
            List<string> errors = new();

            return new ValidatonResult() { IsValid = errors.Count == 0, Errors = errors }; // dummy function for now
        }
    }

    public class UserService : GenericRepositoryService<ApplicationUserRepository, string, ApplicationUser>
    {
        public UserService(ApplicationUserRepository userRepository, IOptions<GenericRepositoryServiceOptions> serviceOptions) : base(userRepository, serviceOptions.Value) { }

        public override ValidatonResult CheckIfEntityIsValid(ApplicationUser entity)
        {
            List<string> errors = new();

            if (_baseRepository.GetByEmailAsync(entity.Email).Result != null) errors.Add("An user already exists with the provided email.");

            return new ValidatonResult() { IsValid = errors.Count == 0, Errors = errors };
        }

        public async Task AddInitialPasswordToUser(ApplicationUser user, string password)
        {
            await _baseRepository.SetInitialPasswordAsync(user, password);
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

            bool areDatesValid = entity.CreationDate <= entity.LastUpdated;
            if (!areDatesValid)
            {
                errors.Add(ServiceValidationMessages.CreationDateMoreRecentThanLastUpdated);
            }

            if (!_languageService.LanguageExists(entity.LanguageISOCode ?? null)) errors.Add(ServiceValidationMessages.InvalidLanguageCode);
            if (!_authService.UserExistsAsync(entity.OwnerId ?? null).Result) errors.Add(ServiceValidationMessages.InvalidUserId);

            return new ValidatonResult() { IsValid = errors.Count == 0, Errors = errors };
        }

        public List<Deck> GetAllDecksFromUser(string ownerId)
        {
            return _baseRepository.GetAll().Where(deck => deck.OwnerId == ownerId).ToList();
        }
    }

    public class FlashcardService : GenericRepositoryService<FlashcardRepository, Guid, Flashcard>
    {
        private readonly DeckService _deckService;

        public FlashcardService(FlashcardRepository baseRepository, DeckService deckService, IOptions<GenericRepositoryServiceOptions> serviceOptions) : base(baseRepository, serviceOptions.Value)
        {
            _deckService = deckService;
        }

        public override ValidatonResult CheckIfEntityIsValid(Flashcard entity)
        {
            // check if DeckId leads to a valid Deck
            var deck = _deckService.GetbyIdAsync(entity.DeckId).Result;

            if (deck == null) return new ValidatonResult() { IsValid = false, Errors = new() { ServiceValidationMessages.InvalidDeckId } };

            return new ValidatonResult() { IsValid = true };
        }

        public List<Flashcard> GetAllFlashcardsFromDeck(Guid deckId)
        {
            return _baseRepository.GetAll().Where(x => x.DeckId == deckId).ToList();
        }
    }

    public class NewsService : GenericRepositoryService<NewsRepository, Guid, News>
    {
        public NewsService(NewsRepository baseRepository, IOptions<GenericRepositoryServiceOptions> serviceOptions) : base(baseRepository, serviceOptions.Value) { }
        public override ValidatonResult CheckIfEntityIsValid(News entity)
        {
            List<string> errors = new();

            bool areDatesValid = entity.CreationDate <= entity.LastUpdated;
            if (!areDatesValid)
            {
                errors.Add(ServiceValidationMessages.CreationDateMoreRecentThanLastUpdated);
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
                                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
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
    public class LanguageService : GenericRepositoryService<LanguageRepository, string, Language>, ILanguageService
    {
        public LanguageService(LanguageRepository baseRepository, IOptions<GenericRepositoryServiceOptions> serviceOptions) : base(baseRepository, serviceOptions.Value) { }

        public override ValidatonResult CheckIfEntityIsValid(Language entity)
        {
            return new ValidatonResult() { IsValid = true }; // no internal validations to run
        }

        public bool LanguageExists(string languageCode)
        {
            return _baseRepository.GetAll().Any(l => l.ISOCode == languageCode);
        }
    }
    #endregion
}
