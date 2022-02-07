using Data.Repository.Interfaces;
using System;
using System.Collections.Generic;

namespace Data.Models.Implementation
{
    public class News : IDatabaseItem<Guid>
    {
        // kids, ALWAYS set default values for properties in the database. This will avoid errors when dealing with ICollection items + LINQ, especially for cases when the current member of the lambda being analyzed is a "null" item. 
        public News() { }
        public Guid NewsID { get; set; }
        public string Title { get; set; } = "";
        public string Subtitle { get; set; } = "";
        public string ThumbnailPath { get; set; } = "";
        public string Content { get; set; } = "";
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime LastUpdated { get; set; } = DateTime.Now;

        public Guid DbId { get => NewsID; set => NewsID = value; }
    }

    public class Deck : IDatabaseItem<Guid>
    {
        public Deck() { }

        public Guid DeckID { get; set; }

        public IEnumerable<Flashcard> Flashcards { get; set; } = new List<Flashcard>();
        public ApplicationUser Owner { get; set; } = null;

        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime LastUpdated { get; set; } = DateTime.Now;

        public Guid DbId { get => DeckID; set => DeckID = value; }
    }

    public class Flashcard : IDatabaseItem<Guid>
    {
        public Flashcard() { }

        public Guid FlashcardID { get; set; }

        public int Level { get; set; } = 0;
        public string FrontContent { get; set; } = "";
        public string BackContent { get; set; } = "";
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime LastUpdated { get; set; } = DateTime.Now;
        public DateTime DueDate { get; set; } = DateTime.Now;

        public Guid DbId { get => FlashcardID; set => FlashcardID = value; }
    }
}
