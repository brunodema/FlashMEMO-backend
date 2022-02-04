using Data.Context;
using Data.Models.Implementation;
using Data.Repository.Implementation;
using Data.Repository.Interfaces;
using System;
using Tests.Integration.Fixtures;
using Xunit;
using Xunit.Abstractions;

namespace Tests.Unit_Tests.Data.Repository
{
    public class DeckRepositoryTests : IClassFixture<IntegrationTestFixture>
    {
        protected readonly ITestOutputHelper _output;
        protected readonly DeckRepository _repository;
        protected readonly IntegrationTestFixture _fixture;

        public DeckRepositoryTests(IntegrationTestFixture integrationTestFixture, ITestOutputHelper output, DeckRepository deckRepository)
        {
            _output = output;
            _repository = deckRepository;
            _fixture = integrationTestFixture;
        }

        [Fact]
        public void test()
        {

        }

    }
}
