using Business.Services.Abstract;
using Business.Services.Interfaces;
using Business.Tools;
using Business.Tools.Interfaces;
using Business.Tools.Validations;
using Data.Models.DTOs;
using Data.Models.Implementation;
using Data.Repository.Implementation;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _baseRepository.GetByEmailAsync(email);
        }

        public async Task<User> GetByUserNameAsync(string username)
        {
            return await _baseRepository.GetByUserNameAsync(username);
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
            if (!_authService.IsIdAlreadyRegisteredAsync(entity.OwnerId ?? null).Result) errors.Add(ServiceValidationMessages.InvalidUserId);

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

            if (!_authService.IsIdAlreadyRegisteredAsync(entity.OwnerId).Result) errors.Add(ErrorMessages.InvalidOwner);

            return new ValidatonResult
            {
                IsValid = !errors.Any(),
                Errors = errors
            };
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
