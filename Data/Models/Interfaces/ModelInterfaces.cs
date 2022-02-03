using Data.Models.Implementation;
using Data.Repository.Interfaces;
using System;
using System.Collections.Generic;

namespace Data.Models.Interfaces
{
    public interface INews : IDatabaseItem<Guid>
    {
        public Guid NewsID { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string ThumbnailPath { get; set; }
        public string Content { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastUpdated { get; set; }
    }

    // Both Decks and Flashcards will have simplistic implementations for the moment, since several issues will arise from the initial coding.
    public interface IDeck : IDatabaseItem<Guid>
    {
        public Guid DeckID { get; set; }
        public IEnumerable<IFlashcard> Flashcards { get; set; }
        public ApplicationUser Owner { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastUpdated { get; set; }
    }

    public interface IFlashcard : IDatabaseItem<Guid>
    {
        public Guid FlashcardID { get; set; }
        public ApplicationUser Owner { get; set; }

        public int Level { get; set; }
        public string FrontContent { get; set; }
        public string BackContent { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastUpdated { get; set; }
        public DateTime DueDate { get; set; }
    }
}
