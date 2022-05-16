using Data.Models.Implementation;

namespace Data.Models.DTOs
{
    public class ExtendedDeckInfoDTO : Deck
    {
        public int FlashcardCount { get; set; } = 0;
        public int DueFlashcardCount { get; set; } = 0;
    }
}
