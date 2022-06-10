using Data.Models.Implementation;
using Data.Repository.Interfaces;
using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using static Data.Models.Implementation.StaticModels;
using static Data.Tools.FlashcardTools;

namespace Data.Models.DTOs
{
    public interface IModelDTO<T, TKey> where T : IDatabaseItem<TKey>
    {
        void PassValuesToEntity(T entity);
    }

    public class RoleDTO : IModelDTO<ApplicationRole, string>
    {
        [Required]
        public string Name { get; set; }

        public ApplicationRole CreateFromDTO()
        {
            return new ApplicationRole()
            {
                Name = Name
            };
        }

        public void PassValuesToEntity(ApplicationRole entity)
        {
            entity.Name = Name;
        }
    }

    public class DeckDTO : IModelDTO<Deck, Guid>
    {
        public string OwnerId { get; set; } = Guid.Empty.ToString();
        public string LanguageISOCode { get; set; } = "";
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime LastUpdated { get; set; } = DateTime.Now;

        public void PassValuesToEntity(Deck entity)
        {
            entity.OwnerId = OwnerId;
            entity.LanguageISOCode = LanguageISOCode;
            entity.Name = Name;
            entity.Description = Description;
            entity.CreationDate = CreationDate;
            entity.LastUpdated = LastUpdated;
        }
    }

    public class NewsDTO : IModelDTO<News, Guid>
    {
        public string Title { get; set; } = "";
        public string OwnerId { get; set; } = Guid.Empty.ToString();
        public string Subtitle { get; set; } = "";
        public string ThumbnailPath { get; set; } = "";
        public string Content { get; set; } = "";
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime LastUpdated { get; set; } = DateTime.Now;

        public void PassValuesToEntity(News entity)
        {
            entity.Title = Title;
            entity.OwnerId = OwnerId;
            entity.Subtitle = Subtitle;
            entity.ThumbnailPath = ThumbnailPath;
            entity.Content = Content;
            entity.CreationDate = CreationDate;
            entity.LastUpdated = LastUpdated;
        }
    }

    public class NewsDTOValidator : AbstractValidator<NewsDTO>
    {
        public NewsDTOValidator()
        {
            RuleFor(x => x.OwnerId).NotEmpty();
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.Subtitle).NotEmpty();
            RuleFor(x => x.Content).NotEmpty();
            RuleFor(x => x.LastUpdated).GreaterThanOrEqualTo((x) => x.CreationDate);
        }
    }

    public class FlashcardDTO : IModelDTO<Flashcard, Guid>
    {
        public Guid DeckId { get; set; } = Guid.Empty;
        public int Level { get; set; } = 0;
        public string Answer { get; set; } = "";
        public FlashcardContentLayout FrontContentLayout { get; set; } = FlashcardContentLayout.SINGLE_BLOCK;
        public FlashcardContentLayout BackContentLayout { get; set; } = FlashcardContentLayout.SINGLE_BLOCK;
        public string Content1 { get; set; } = "";
        public string Content2 { get; set; } = "";
        public string Content3 { get; set; } = "";
        public string Content4 { get; set; } = "";
        public string Content5 { get; set; } = "";
        public string Content6 { get; set; } = "";
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime LastUpdated { get; set; } = DateTime.Now;
        public DateTime DueDate { get; set; } = DateTime.Now;

        public void PassValuesToEntity(Flashcard entity)
        {
            entity.DeckId = DeckId;
            entity.Level = Level;
            entity.Answer = Answer;
            entity.FrontContentLayout = FrontContentLayout;
            entity.BackContentLayout = BackContentLayout;
            entity.Content1 = Content1;
            entity.Content2 = Content2;
            entity.Content3 = Content3;
            entity.Content4 = Content4;
            entity.Content5 = Content5;
            entity.Content6 = Content6;
            entity.CreationDate = CreationDate;
            entity.LastUpdated = LastUpdated;
            entity.DueDate = DueDate;
        }
    }

    public class FlashcardDTOValidator : AbstractValidator<FlashcardDTO>
    {
        public class ValidationMessages
        {
            public static readonly string FrontContentEmpty = "Main front content can not be empty.";
            public static readonly string BackContentEmpty = "Main back content can not be empty.";
        }

        public FlashcardDTOValidator()
        {
            RuleFor(x => x.DeckId).NotEmpty();
            RuleFor(x => x.Level).GreaterThanOrEqualTo(0);
            RuleFor(x => x.DueDate).GreaterThanOrEqualTo((x) => x.CreationDate);
            RuleFor(x => x.LastUpdated).GreaterThanOrEqualTo((x) => x.CreationDate);
            RuleFor(x => x.Content1).NotEmpty().WithMessage(ValidationMessages.FrontContentEmpty);
            RuleFor(x => x.Content4).NotEmpty().WithMessage(ValidationMessages.BackContentEmpty);

            When(flaschard => flaschard.FrontContentLayout == FlashcardContentLayout.HORIZONTAL_SPLIT || flaschard.FrontContentLayout == FlashcardContentLayout.VERTICAL_SPLIT, () => {
                RuleFor(flaschard => flaschard.Content2).NotEmpty();
            });
            When(flaschard => flaschard.FrontContentLayout == FlashcardContentLayout.TRIPLE_BLOCK || flaschard.FrontContentLayout == FlashcardContentLayout.FULL_CARD, () => {
                RuleFor(flaschard => flaschard.Content2).NotEmpty();
                RuleFor(flaschard => flaschard.Content3).NotEmpty();
            });
            When(flaschard => flaschard.BackContentLayout == FlashcardContentLayout.HORIZONTAL_SPLIT || flaschard.BackContentLayout == FlashcardContentLayout.VERTICAL_SPLIT, () => {
                RuleFor(flaschard => flaschard.Content5).NotEmpty();
            });
            When(flaschard => flaschard.BackContentLayout == FlashcardContentLayout.TRIPLE_BLOCK || flaschard.BackContentLayout == FlashcardContentLayout.FULL_CARD, () => {
                RuleFor(flaschard => flaschard.Content5).NotEmpty();
                RuleFor(flaschard => flaschard.Content6).NotEmpty();
            });
        }
    }

    public class LanguageDTO : IModelDTO<Language, string>
    {
        public string ISOCode { get; set; } = "";
        public string Name { get; set; } = "";

        public void PassValuesToEntity(Language entity)
        {
            entity.ISOCode = ISOCode;
            entity.Name = Name;
        }
    }

    public class LanguageDTOValidator : AbstractValidator<LanguageDTO>
    {
        public class ValidationMessages
        {
            // nothing necessary for now
        }

        public LanguageDTOValidator()
        {
            RuleFor(x => x.ISOCode).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
        }
    }

    public class UserDTO : IModelDTO<ApplicationUser, string>
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        // regex stolen from here: https://stackoverflow.com/questions/5859632/regular-expression-for-password-validation
        public string Password { get; set; }

        public void PassValuesToEntity(ApplicationUser entity)
        {
            entity.Email = Email;
            entity.UserName = Username;
            entity.Name = Name;
            entity.Surname = Surname;
            // does not use password here: it is set on the DTO so the endpoint is properly formed, but who does the actual assignment is the service, after the user has been created
        }
    }

    public class UserDTOValidator : AbstractValidator<UserDTO>
    {
        public class ValidationMessages
        {
            public static readonly string InvalidPasswordFormat = "Password must be between 6 and 20 characters and contain one uppercase letter, one lowercase letter, one digit and one special character.";
        }

        public UserDTOValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Surname).NotEmpty();
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Password).Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$").WithMessage(ValidationMessages.InvalidPasswordFormat);
        }
    }
}
