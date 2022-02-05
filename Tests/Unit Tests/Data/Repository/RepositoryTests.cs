using Data.Context;
using Data.Models.Implementation;
using Data.Repository.Abstract;
using Data.Repository.Implementation;
using Data.Repository.Interfaces;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
            //terrible design since context and repository are used throughout the class, but only set on the inherited class. Unfortunately, there is no easy/compact way to do this using fixtures(no time to pass to a member of the child class, resulting in the temporary object being disposed) or other methods(can not initialize repository class here since it uses a constructor with parameters).

            // maybe it would work if the repository classes were declared like Repository<News> or Repository<Deck>, so something like _repository = new Repository<TEntity>(_context) could be used?
            _output = output;
            _context = new FlashMEMOContext(new DbContextOptionsBuilder<FlashMEMOContext>().UseInMemoryDatabase(databaseName: "FlashMEMOTest").Options);
        }

        public async virtual void CreateEntity(TEntity entity)
        {
            // Arrange
            // Act
            await _repository.CreateAsync(entity);
            var entityFromRepository = _context.Set<TEntity>().Find(entity.DbId);

            // Assert
            entity.Should().Be(entityFromRepository);
        }

        public async virtual void ReadEntity(TEntity entity)
        {
            // Arrange
            _context.Set<TEntity>().Add(entity);
            _context.SaveChanges();

            // Act
            var entityFromRepository = await _repository.GetByIdAsync(entity.DbId);

            // Assert
            entity.Should().Be(entityFromRepository);
        }
        public async virtual void UpdateEntity(TEntity previousEntity, TEntity updatedEntity) 
        {
            // Arrange
            _context.Set<TEntity>().Add(previousEntity);
            _context.SaveChanges();

            // Act
            var entityFromRepository = _context.Set<TEntity>().FirstOrDefault(x => x == previousEntity);
            entityFromRepository = updatedEntity; // this is causing problems: it starts tracking the fucking updatedentity object
            await _repository.UpdateAsync(entityFromRepository);

            // Assert
            entityFromRepository = _context.Set<TEntity>().Find(entityFromRepository.DbId);
            entityFromRepository.Should().Be(updatedEntity);
            entityFromRepository.Should().NotBe(previousEntity);
        }
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

        public static IEnumerable<object[]> UpdateEntityData =>
        new List<object[]>
        {
                new object[] { new Deck { Name = "deck", Description = "this is a test deck" }, new Deck { Name = "updated deck", Description = "this is an updated test deck" } },
        };

        [Theory, MemberData(nameof(UpdateEntityData))]
        public override void UpdateEntity(Deck previousEntity, Deck updatedEntity)
        {
            base.UpdateEntity(previousEntity, updatedEntity);
        }
    }

    // this class is here just to prove if the concept of the generic class works or not for multiple types
    public class NewsRepositoryUnitTests : GenericRepositoryUnitTests<NewsRepository, News, Guid>
    {
        public NewsRepositoryUnitTests(ITestOutputHelper output) : base(output)
        {
            _repository = new NewsRepository(_context);
        }

        public static IEnumerable<object[]> CreateEntityData =>
            new List<object[]>
            {
                new object[] { new News { Title = "title", Subtitle = "subtitle", Content = "content" } },
                new object[] { new News { Title = "title1", Subtitle = "subtitle1", Content = "content1" } },
            };

        [Theory, MemberData(nameof(CreateEntityData))]
        public override void CreateEntity(News entity)
        {
            base.CreateEntity(entity);
        }

        public static IEnumerable<object[]> UpdateEntityData =>
        new List<object[]>
        {
                new object[] { new News { Title = "title", Subtitle = "subtitle", Content = "content" }, new News { Title = "new title", Subtitle = "new subtitle", Content = "new content" } },
        };

        [Theory, MemberData(nameof(UpdateEntityData))]
        public override void UpdateEntity(News previousEntity, News updatedEntity)
        {
            base.UpdateEntity(previousEntity, updatedEntity);
        }
    }
}
