﻿using Data.Repository.Interfaces;
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

        public Guid NewsID { get; set; } = Guid.Empty;

        public string Title { get; set; } = "";
        public string Subtitle { get; set; } = "";
        public string ThumbnailPath { get; set; } = "";
        public string Content { get; set; } = "";
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime LastUpdated { get; set; } = DateTime.Now;

        [NotMapped]
        public Guid DbId { get => NewsID; set => NewsID = value; }

        public override bool Equals(object obj)
        {
            return obj is News model &&
                   Title == model.Title &&
                   Subtitle == model.Subtitle &&
            ThumbnailPath == model.ThumbnailPath &&
            Content == model.Content &&
            CreationDate == model.CreationDate &&
            LastUpdated == model.LastUpdated;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(NewsID, Subtitle, ThumbnailPath, Content, CreationDate, LastUpdated);
        }
    }

    public class Deck : IDatabaseItem<Guid>
    {
        public Deck() { }

        public Guid DeckID { get; set; } = Guid.Empty;

        public List<Flashcard> Flashcards { get; set; } = new List<Flashcard>();
        public ApplicationUser Owner { get; set; } = null;
        public string OwnerId { get; set; } = Guid.Empty.ToString();
        public Language Language { get; set; } = null;
        public string LanguageISOCode { get; set; } = "";

        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime LastUpdated { get; set; } = DateTime.Now;

        [NotMapped]
        public Guid DbId { get => DeckID; set => DeckID = value; }

        public override bool Equals(object obj)
        {
            return obj is Deck model &&
                   DeckID == model.DeckID &&
                   OwnerId == model.OwnerId &&
                LanguageISOCode == model.LanguageISOCode &&
                Name == model.Name &&
                Description == model.Description &&
                CreationDate == model.CreationDate &&
                LastUpdated == model.LastUpdated;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(DeckID, OwnerId, LanguageISOCode, Name, Description, CreationDate, LastUpdated);
        }
    }

    public class Flashcard : IDatabaseItem<Guid>
    {
        public Flashcard() { }

        public Guid FlashcardID { get; set; } = Guid.Empty;

        public Deck Deck { get; set; } = null;
        public Guid DeckId { get; set; } = Guid.Empty;

        public int Level { get; set; } = 0;
        public FlashcardContentLayout FrontContentLayout { get; set; } = FlashcardContentLayout.SINGLE_BLOCK;
        public FlashcardContentLayout BackContentLayout { get; set; } = FlashcardContentLayout.SINGLE_BLOCK;
        public string Content1 { get; set; } = "";
        public string Content2 { get; set; } = "";
        public string Content3 { get; set; } = "";
        public string Content4 { get; set; } = "";
        public string Content5 { get; set; } = "";
        public string Content6 { get; set; } = "";
        public string Answer { get; set; } = "";
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime LastUpdated { get; set; } = DateTime.Now;
        public DateTime DueDate { get; set; } = DateTime.Now;

        [NotMapped]
        public Guid DbId { get => FlashcardID; set => FlashcardID = value; }

        public override bool Equals(object obj)
        {
            return obj is Flashcard model &&
                FlashcardID == model.FlashcardID &&
                DeckId == model.DeckId &&
                Level == model.Level &&
                FrontContentLayout == model.FrontContentLayout &&
                BackContentLayout == model.BackContentLayout &&
                Content1 == model.Content1 &&
                Content2 == model.Content2 &&
                Content3 == model.Content3 &&
                Content4 == model.Content4 &&
                Content5 == model.Content5 &&
                Content6 == model.Content6 &&
                Answer == model.Answer &&
                CreationDate == model.CreationDate &&
                LastUpdated == model.LastUpdated &&
                DueDate == model.DueDate;
        }

        public override int GetHashCode()
        {
            // ATTENTION: the 'Combine' method doesn't have any overload with more than 8 arguments, therefore I chose the following ones for the function. TBH, what I'm currently using for these 'Combine' arguments are probably overkill... I should try to understand this better in the future.
            return HashCode.Combine(FlashcardID, Content1, Content2, Content3, Content4, Content5, Content6, Answer);
        }
    }
}