using Data.Context;
using Data.Models.Implementation;
using Data.Repository.Abstract;
using Data.Repository.Implementation;
using Data.Repository.Interfaces;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace Tests.Unit_Tests.Data.Repository
{
    public abstract class GenericRepositoryUnitTests<RepositoryClass, TEntity, TKey>  : IDisposable
        where RepositoryClass : GenericRepository<TEntity, TKey, FlashMEMOContext>
        where TEntity : class, IDatabaseItem<TKey>
    {
        protected readonly ITestOutputHelper _output;
        protected readonly RepositoryClass _repository;
        protected readonly FlashMEMOContext _context;

        public GenericRepositoryUnitTests(ITestOutputHelper output, RepositoryClass repositoryClass)
        {
            _output = output;
            _repository = repositoryClass;
        }

        public async virtual void CreateEntity(TEntity entity)
        {
            // Arrange
            // Act
            await _repository.CreateAsync(entity);
            var entityFromRepository = await _repository.GetByIdAsync(entity.GetId());

            // Assert
            entity.Should().Be(entityFromRepository);
        }

        public void Dispose()
        {
            _repository?.Dispose();
            _context?.Dispose();
        }
        //public virtual void ReadEntity() { }
        //public virtual void UpdateEntity() { }
        //public virtual void DeleteEntity() { }
    }

    public class DeckRepositoryUnitTests : GenericRepositoryUnitTests<DeckRepository, Deck, Guid>
    {
        // Yes, the base constructor is atrocius at the moment, but I couldn't find a better way the repository class
        public DeckRepositoryUnitTests(ITestOutputHelper output) : base(output, new DeckRepository(new FlashMEMOContext(new DbContextOptionsBuilder<FlashMEMOContext>().UseInMemoryDatabase(databaseName: "FlashMEMOTest").Options))) { }

        public static IEnumerable<object[]> CreateEntityData =>
            new List<object[]>
            {
                new object[] { new Deck { Name = "test", Description = "this is a test deck" } },
                new object[] { new Deck { Name = "test number two", Description = "this is another test deck" } }
            };

        [Theory, MemberData(nameof(CreateEntityData))]
        public override void CreateEntity(Deck entity)
        {
            base.CreateEntity(entity);
        }
    }
}
