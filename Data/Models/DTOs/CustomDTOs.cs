using Data.Models.Implementation;

namespace Data.Models.DTOs
{
    /// <summary>
    /// Deck DTO that has extra info regarding the flashcards from the Deck.
    /// </summary>
    public class ExtendedDeckInfoDTO : Deck
    {
        public int FlashcardCount { get; set; } = 0;
        public int DueFlashcardCount { get; set; } = 0;
    }

    /// <summary>
    /// User DTO used to avoid over-posting information to the front-end.
    /// </summary>
    public class ReducedUserDTO
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string Surname { get; set; } = "";
        public string Email { get; set; }

        public ReducedUserDTO() { }

        public ReducedUserDTO(ApplicationUser user)
        {
            Id = user.Id;
            Email = user.Email;
            Name = user.Name;
            Surname = user.Surname;
        }
    }
}
