using Data.Context;
using Data.Models.Implementation;
using Data.Repository.Implementation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Tests.Integration.Fixtures;
using Xunit;
using Xunit.Abstractions;

namespace Tests.Unit_Tests.Data.Repository
{
    public class DeckRepositoryTests : IDisposable
    {
        protected readonly ITestOutputHelper _output;
        protected readonly DeckRepository _repository;
        protected readonly FlashMEMOContext _context;

        public DeckRepositoryTests(ITestOutputHelper output)
        {
            _output = output;

            var options = new DbContextOptionsBuilder<FlashMEMOContext>().UseInMemoryDatabase(databaseName: "FlashMEMOTest").Options;
            _context = new FlashMEMOContext(options);

            _repository = new DeckRepository(_context);
        }

        [Fact]
        public async void test()
        {
            var deck = new Deck()
            {
                DeckID = new Guid(),
                Name = "test deck",
                Description = "this is a test deck",
            };

            var flashcard = new Flashcard()
            {
                FrontContent = "nothing in front",
                BackContent = "nothing in back",
                Level = 1,
            };

            await _repository.CreateAsync(deck);

            var deckFromRepo = await _repository.GetByIdAsync(deck.DeckID);
            deck.Flashcards = new List<Flashcard> { flashcard };

            await _repository.SaveChangesAsync();

            deckFromRepo = await _repository.GetByIdAsync(deck.DeckID);
        }

        public void Dispose()
        {
            _repository.Dispose();
            _context?.Dispose();
        }
    }
}
