using Business.Services.Abstract;
using Business.Services.Interfaces;
using Business.Tools;
using Business.Tools.Interfaces;
using Business.Tools.Validations;
using Data.Models.DTOs;
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
    public class RoleService : GenericRepositoryService<RoleRepository, string, Role>
    {

        public RoleService(RoleRepository roleRepository, IOptions<GenericRepositoryServiceOptions> serviceOptions) : base(roleRepository, serviceOptions.Value) { }

        public override ValidatonResult CheckIfEntityIsValid(Role entity)
        {
            List<string> errors = new();

            return new ValidatonResult() { IsValid = errors.Count == 0, Errors = errors }; // dummy function for now
        }
    }

    public class UserService : GenericRepositoryService<UserRepository, string, User>
    {
        public UserService(UserRepository userRepository, IOptions<GenericRepositoryServiceOptions> serviceOptions) : base(userRepository, serviceOptions.Value) { }

        public override ValidatonResult CheckIfEntityIsValid(User entity)
        {
            List<string> errors = new();

            var userByEmail = _baseRepository.GetByEmailAsync(entity.Email).Result;
            var userByUsername = _baseRepository.GetByUserNameAsync(entity.UserName).Result;
            var userById = _baseRepository.GetByIdAsync(entity.Id).Result;

            if (userById != null)
            {
                // this means that the user already exists (update mode)
                if (userByEmail != null)
                {
                    if (userById.Id != userByEmail.Id) errors.Add("An user already exists with the provided email.");
                }
                if (userByUsername != null)
                {
                    if (userById.Id != userByUsername.Id) errors.Add("An user already exists with the provided username.");
                }
            }
            else
            {
                // this means that the user does not exist (create mode)
                if (userByEmail != null)
                {
                    errors.Add("An user already exists with the provided email.");
                }
                if (userByUsername != null)
                {
                    errors.Add("An user already exists with the provided username.");
                }
            }

            return new ValidatonResult() { IsValid = errors.Count == 0, Errors = errors };
        }

        public async Task AddInitialPasswordToUser(User user, string password)
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
        public static class ErrorMessages
        {
            public static readonly string InvalidOwner = "The provided UserId does not point to a valid user.";
        }

        private readonly IAuthService<string> _authService;

        public NewsService(NewsRepository baseRepository, IAuthService<string> authService, IOptions<GenericRepositoryServiceOptions> serviceOptions) : base(baseRepository, serviceOptions.Value) { _authService = authService; }

        public override ValidatonResult CheckIfEntityIsValid(News entity)
        {
            List<string> errors = new();

            if (!_authService.UserExistsAsync(entity.OwnerId).Result) errors.Add(ErrorMessages.InvalidOwner);

            return new ValidatonResult
            {
                IsValid = !errors.Any(),
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

        public string CreateLoginToken(User user)
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

            return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
                issuer: _options.ValidIssuer, audience: _options.ValidAudience,
                expires: DateTime.Now.AddSeconds(Convert.ToDouble(_options.TimeToExpiration)),
                claims: claims,
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            ));

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
