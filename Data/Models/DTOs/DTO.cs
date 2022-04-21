using Data.Models.Implementation;
using Data.Repository.Interfaces;
using System;
using static Data.Tools.FlashcardTools;

namespace Data.Models.DTOs
{
    public interface IModelDTO<T, TKey> where T : IDatabaseItem<TKey>
    {
    }

    public class DeckDTO : IModelDTO<Deck, Guid>
    {
        public Guid OwnerId { get; set; } = Guid.Empty;
        public Guid LanguageId { get; set; } = Guid.Empty;
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime LastUpdated { get; set; } = DateTime.Now;
    }

    public class NewsDTO : IModelDTO<News, Guid>
    {
        public string Title { get; set; } = "";
        public string Subtitle { get; set; } = "";
        public string ThumbnailPath { get; set; } = "";
        public string Content { get; set; } = "";
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime LastUpdated { get; set; } = DateTime.Now;
    }

    public class FlashcardDTO : IModelDTO<Flashcard, Guid>
    {
        public Guid DeckId { get; set; } = Guid.Empty;
        public int Level { get; set; } = 0;
        public FlashcardContentLayout ContentLayout { get; set; } = FlashcardContentLayout.SINGLE;
        public string Content1 { get; set; } = "";
        public string Content2 { get; set; } = "";
        public string Content3 { get; set; } = "";
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime LastUpdated { get; set; } = DateTime.Now;
        public DateTime DueDate { get; set; } = DateTime.Now;
    }
}
