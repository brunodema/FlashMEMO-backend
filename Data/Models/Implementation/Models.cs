﻿using Data.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace Data.Models.Implementation
{
    public class News : INews
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

        public Guid GetId()
        {
            return NewsID;
        }
    }

    public class Deck : IDeck
    {
        public Deck() { }

        public Guid DeckID { get; set; }
        public IEnumerable<IFlashcard> Flashcards { get; set; }
        public ApplicationUser Owner { get; set; }

        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime LastUpdated { get; set; } = DateTime.Now;

        public Guid GetId()
        {
            return DeckID;
        }
    }

    public class Flashcard : IFlashcard
    {
        public Flashcard() { }

        public Guid FlashcardID { get; set; }
        public ApplicationUser Owner { get; set; }

        public int Level { get; set; } = 0;
        public string FrontContent { get; set; } = "";
        public string BackContent { get; set; } = "";
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime LastUpdated { get; set; } = DateTime.Now;
        public DateTime DueDate { get; set; } = DateTime.Now;

        public Guid GetId()
        {
            return FlashcardID;
        }
    }
}
