using Data.Models.Implementation;
using Data.Repository.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using static Data.Tools.FlashcardTools;

namespace Data.Models.DTOs
{
    public interface IModelDTO<T, TKey> where T : IDatabaseItem<TKey>
    {
        T CreateFromDTO();
    }

    public class UserDTO : IModelDTO<ApplicationUser, string>
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Username { get; set; }

        // regex stolen from here: https://stackoverflow.com/questions/5859632/regular-expression-for-password-validation
        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$", ErrorMessage = "Password must be between 6 and 20 characters and contain one uppercase letter, one lowercase letter, one digit and one special character.")]

        [DataType(DataType.Password)]
        public string Password { get; set; }

        public ApplicationUser CreateFromDTO()
        {
            return new ApplicationUser()
            {
                Email = Email,
                UserName = Username,
                // does not use password here: it is set on the DTO so the endpoint is properly formed, but who does the actual assignment is the service, after the user has been created
            };
        }
    }

    public class DeckDTO : IModelDTO<Deck, Guid>
    {
        public string OwnerId { get; set; } = Guid.Empty.ToString();
        public string LanguageId { get; set; } = "";
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime LastUpdated { get; set; } = DateTime.Now;

        public Deck CreateFromDTO()
        {
            return new Deck()
            {
                OwnerId = OwnerId,
                LanguageISOCode = LanguageId,
                Name = Name,
                Description = Description,
                CreationDate = CreationDate,
                LastUpdated = LastUpdated,

                DeckID = Guid.Empty,
            };
        }
    }

    public class NewsDTO : IModelDTO<News, Guid>
    {
        public string Title { get; set; } = "";
        public string Subtitle { get; set; } = "";
        public string ThumbnailPath { get; set; } = "";
        public string Content { get; set; } = "";
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime LastUpdated { get; set; } = DateTime.Now;

        public News CreateFromDTO()
        {
            return new News()
            {
                Title = Title,
                Subtitle = Subtitle,
                ThumbnailPath = ThumbnailPath,
                Content = Content,
                CreationDate = CreationDate,
                LastUpdated = LastUpdated,

                NewsID = Guid.Empty,
            };
        }
    }

    public class FlashcardDTO : IModelDTO<Flashcard, Guid>
    {
        public Guid DeckId { get; set; } = Guid.Empty;
        public int Level { get; set; } = 0;
        public string Answer { get; set; } = "";
        public FlashcardContentLayout ContentLayout { get; set; } = FlashcardContentLayout.SINGLE;
        public string Content1 { get; set; } = "";
        public string Content2 { get; set; } = "";
        public string Content3 { get; set; } = "";
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime LastUpdated { get; set; } = DateTime.Now;
        public DateTime DueDate { get; set; } = DateTime.Now;

        public Flashcard CreateFromDTO()
        {
            return new Flashcard()
            {
                DeckId = DeckId,
                Level = Level,
                Answer = Answer,
                ContentLayout = ContentLayout,
                Content1 = Content1,
                Content2 = Content2,
                Content3 = Content3,
                CreationDate = CreationDate,
                LastUpdated = LastUpdated,
                DueDate = DueDate,

                FlashcardID = Guid.Empty
            };
        }
    }
}
