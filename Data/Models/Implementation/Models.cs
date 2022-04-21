﻿using Data.Repository.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using static Data.Models.Implementation.StaticModels;
using static Data.Tools.FlashcardTools;

namespace Data.Models.Implementation
{
    public class News : IDatabaseItem<Guid>
    {
        // kids, ALWAYS set default values for properties in the database. This will avoid errors when dealing with ICollection items + LINQ, especially for cases when the current member of the lambda being analyzed is a "null" item. 
        public News() { }

        [JsonIgnore]
        public Guid NewsID { get; set; } = new Guid();

        public string Title { get; set; } = "";
        public string Subtitle { get; set; } = "";
        public string ThumbnailPath { get; set; } = "";
        public string Content { get; set; } = "";
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime LastUpdated { get; set; } = DateTime.Now;

        [NotMapped, JsonIgnore]
        public Guid DbId { get => NewsID; set => NewsID = value; }
    }

    public class Deck : IDatabaseItem<Guid>
    {
        public Deck() { }

        [JsonIgnore]
        public Guid DeckID { get; set; } = new Guid();

        [JsonIgnore]
        public List<Flashcard> Flashcards { get; set; } = new List<Flashcard>();
        [JsonIgnore]
        public ApplicationUser Owner { get; set; } = null;
        public Guid OwnerId { get; set; } = Guid.Empty;
        [JsonIgnore]
        public Language Language { get; set; } = null;
        public Guid LanguageId { get; set; } = Guid.Empty;

        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime LastUpdated { get; set; } = DateTime.Now;

        [NotMapped, JsonIgnore]
        public Guid DbId { get => DeckID; set => DeckID = value; }
    }

    public class Flashcard : IDatabaseItem<Guid>
    {
        public Flashcard() { }

        [JsonIgnore]
        public Guid FlashcardID { get; set; } = new Guid();

        [JsonIgnore]
        public Deck Deck { get; set; } = null;
        public Guid DeckId { get; set; } = Guid.Empty;

        public int Level { get; set; } = 0;
        public FlashcardContentLayout ContentLayout { get; set; } = FlashcardContentLayout.SINGLE;
        public string Content1 { get; set; } = "";
        public string Content2 { get; set; } = "";
        public string Content3 { get; set; } = "";
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime LastUpdated { get; set; } = DateTime.Now;
        public DateTime DueDate { get; set; } = DateTime.Now;

        [NotMapped, JsonIgnore]
        public Guid DbId { get => FlashcardID; set => FlashcardID = value; }
    }
}
