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
        protected ITestOutputHelper _output;
        protected RepositoryClass _repository;
        protected FlashMEMOContext _context;

        public GenericRepositoryUnitTests(ITestOutputHelper output)
        {
            // terrible design since context and repository are used throughout the class, but only set on the inherited class. Unfortunately, there is no easy/compact way to do this using fixtures (no time to pass to a member of the child class, resulting in the temporary object being disposed) or other methods (can not initialize repository class here since it uses a constructor with parameters).
            _output = output;
        }

        public async virtual void CreateEntity(TEntity entity)
        {
            // Arrange
            // Act
            await _repository.CreateAsync(entity);
            var entityFromRepository = _context.Set<TEntity>().Find(entity.GetId());

            // Assert
            entity.Should().Be(entityFromRepository);
        }

        //public virtual void ReadEntity() { }
        //public virtual void UpdateEntity() { }
        //public virtual void DeleteEntity() { }

        public void Dispose()
        {
            _repository?.Dispose();
            _context?.Dispose();
        }
    }

    public class DeckRepositoryUnitTests : GenericRepositoryUnitTests<DeckRepository, Deck, Guid>
    {
        public DeckRepositoryUnitTests(ITestOutputHelper output) : base(output)
        { 
            _context = new FlashMEMOContext(new DbContextOptionsBuilder<FlashMEMOContext>().UseInMemoryDatabase(databaseName: "FlashMEMOTest").Options);
            _repository = new DeckRepository(_context);
        }

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
