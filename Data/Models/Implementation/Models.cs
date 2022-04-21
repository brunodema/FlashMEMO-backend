using Data.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using static Data.Models.Implementation.StaticModels;
using static Data.Tools.FlashcardTools;

namespace Data.Models.Implementation
{
    public class News : IDatabaseItem<Guid>
    {
        // kids, ALWAYS set default values for properties in the database. This will avoid errors when dealing with ICollection items + LINQ, especially for cases when the current member of the lambda being analyzed is a "null" item. 
        public News() { }

        public Guid NewsID { get; set; } = new Guid();

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

        public Guid DeckID { get; set; } = new Guid();

        public IEnumerable<Flashcard> Flashcards { get; set; } = new List<Flashcard>();
        public ApplicationUser Owner { get; set; } = null;
        public Language Language { get; set; } = null;

        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime LastUpdated { get; set; } = DateTime.Now;

        public Guid DbId { get => DeckID; set => DeckID = value; }

        /// <summary>
        /// Work-around so errors are not thrown while unit-testing. More especifically, as far as I know, if a method is used for a selector lambda (ex: set the property to order a collection by), FluentAssertions will not allow such thing. However, a property that wraps a method works.
        /// </summary>
        [NotMapped]
        public int FlashcardCount { get => Flashcards.Count(); set => FlashcardCount = value; }
    }

    public class Flashcard : IDatabaseItem<Guid>
    {
        public Flashcard() { }

        public Guid FlashcardID { get; set; } = new Guid();

        public Deck Deck { get; set; } = null;

        public int Level { get; set; } = 0;
        public FlashcardContentLayout ContentLayout { get; set; } = FlashcardContentLayout.SINGLE;
        public string Content1 { get; set; } = "";
        public string Content2 { get; set; } = "";
        public string Content3 { get; set; } = "";
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime LastUpdated { get; set; } = DateTime.Now;
        public DateTime DueDate { get; set; } = DateTime.Now;

        public Guid DbId { get => FlashcardID; set => FlashcardID = value; }
    }
}
