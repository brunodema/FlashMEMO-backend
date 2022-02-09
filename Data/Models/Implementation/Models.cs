using Data.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit.Abstractions;

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

    public class Deck : IDatabaseItem<Guid>, IXunitSerializable
    {
        public Deck() { }

        public Guid DeckID { get; set; } = Guid.NewGuid();

        public IEnumerable<Flashcard> Flashcards { get; set; } = new List<Flashcard>();
        public ApplicationUser Owner { get; set; } = null;

        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime LastUpdated { get; set; } = DateTime.Now;

        public Guid DbId { get => DeckID; set => DeckID = value; }

        /// <summary>
        /// Work-around so errors are not thrown while unit-testing. More especifically, as far as I know, if a method is used for a selector lambda (ex: set the property to order a collection by), FluentAssertions will not allow such thing. However, a property that wraps a method works.
        /// </summary>
        public int FlashcardCount { get => Flashcards.Count(); set => FlashcardCount = value; }

        /// <summary>
        /// According to the internet: "The Deserialize method is, I think, used if you have your test cases stored in an external file that is read in by XUnit.  For our case, it doesn’t matter, so we can just leave it blank". Source: https://darchuk.net/2019/04/12/serializing-xunit-test-cases/.
        /// </summary>
        /// <param name="info"></param>
        public void Deserialize(IXunitSerializationInfo info) { }

        public void Serialize(IXunitSerializationInfo info)
        {
            info.AddValue(nameof(DeckID), DeckID.ToString());
            info.AddValue(nameof(Owner), Owner?.UserName ?? "");
            info.AddValue(nameof(FlashcardCount), FlashcardCount.ToString());
            info.AddValue(nameof(Name), Name);
            info.AddValue(nameof(Description), Description);
            info.AddValue(nameof(CreationDate), CreationDate.ToString());
            info.AddValue(nameof(LastUpdated), LastUpdated.ToString());
        }
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
