using Data.Context;
using Data.Models.Implementation;
using Data.Repository.Implementation;
using Data.Repository.Interfaces;
using System;
using Xunit;
using Xunit.Abstractions;

namespace Tests.Unit_Tests.Data.Repository
{
    public class DeckRepositoryTests
    {
        protected readonly ITestOutputHelper _output;
        protected readonly DeckRepository _repository;

        public DeckRepositoryTests(ITestOutputHelper output, DeckRepository deckRepository)
        {
            _output = output;
            _repository = deckRepository;
        }

        [Fact]
        public void test()
        {

        }

    }
}
