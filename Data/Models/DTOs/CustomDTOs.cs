using Data.Models.Implementation;
using System;

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
    /// News DTO that has extra info regarding the user that published it.
    /// </summary>
    public class ExtendedNewsInfoDTO : News
    {
        public ReducedUserDTO OwnerInfo { get; set; } = new ReducedUserDTO();
    }

    /// <summary>
    /// User DTO used to avoid over-posting information to the front-end.
    /// </summary>
    public class ReducedUserDTO
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string Surname { get; set; } = "";
        public string Username { get; set; } = "";
        public string Email { get; set; } = "";
        public DateTime? LastLogin { get; set; } = null;

        public ReducedUserDTO() { }

        public ReducedUserDTO(User user)
        {
            Id = user.Id;
            Email = user.Email;
            Name = user.Name;
            Surname = user.Surname;
            Username = user.UserName;
            LastLogin = user.LastLogin;
        }
    }
}
