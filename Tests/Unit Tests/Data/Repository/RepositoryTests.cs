﻿using Data.Context;
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

        /// <summary>
        /// Used to copy values between two entity objects while at the same time avoiding that the context starts tracking both of them, which leads to exceptions when running the update method. After setting the property values of <paramref name="destination"/> to be equal to the ones from <paramref name="source"/>, it also makes sure that the Id of the object is not changed during the operation, otherwise EF will assign a third Id value when processing the changes at the database.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        protected void SafeEFEntityValueCopy(TEntity source, TEntity destination)
        {
            source.DbId = destination.DbId; // makes assignment here so the error is not thrown below (i.e., Id gets set to a value different than before)
            _context.Entry(destination).CurrentValues.SetValues(source);
        }

        /// <summary>
        /// Uses context functions to get an entity from the databse (isolate functionality from repository class).
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected TEntity GetEntityViaContext(TKey id)
        {
            return _context.Set<TEntity>().Find(id);
        }

        /// <summary>
        /// Uses context functions to add an entity to the database (isolate functionality from repository class).
        /// </summary>
        /// <param name="entity"></param>
        protected void AddEntityViaContext(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
            _context.SaveChanges();
            _context.Set<TEntity>().Find(entity.DbId).Should().Be(entity);
        }

        public async virtual void CreateEntity(TEntity entity)
        {
            // Arrange
            // Act
            await _repository.CreateAsync(entity);
            var entityFromRepository = GetEntityViaContext(entity.DbId);

            // Assert
            entity.Should().BeEquivalentTo(entityFromRepository);
        }

        public async virtual void ReadEntity(TEntity entity)
        {
            // Arrange
            AddEntityViaContext(entity);

            // Act
            var entityFromRepository = await _repository.GetByIdAsync(entity.DbId);

            // Assert
            entity.Should().BeEquivalentTo(entityFromRepository);
        }
        public async virtual void UpdateEntity(TEntity previousEntity, TEntity updatedEntity) 
        {
            // Arrange
            AddEntityViaContext(previousEntity);

            // Act
            var entityFromRepository = GetEntityViaContext(previousEntity.DbId);
            SafeEFEntityValueCopy(updatedEntity, entityFromRepository);
            await _repository.UpdateAsync(entityFromRepository);

            // Assert
            entityFromRepository = GetEntityViaContext(entityFromRepository.DbId);
            entityFromRepository.Should().BeEquivalentTo(updatedEntity);
            entityFromRepository.Should().BeEquivalentTo(previousEntity);
        }

        public async virtual void DeleteEntity(TEntity entity)
        {
            // Arrange
            AddEntityViaContext(entity);

            // Act
            await _repository.RemoveByIdAsync(entity.DbId);

            // Assert
            GetEntityViaContext(entity.DbId).Should().BeNull();
        }

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

        public static IEnumerable<object[]> ReadEntityData =>
            new List<object[]>
            {
                new object[] { new Deck { Name = "test", Description = "this is a test deck" } },
            };

        [Theory, MemberData(nameof(ReadEntityData))]
        public override void ReadEntity(Deck entity)
        {
            base.ReadEntity(entity);
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

        public static IEnumerable<object[]> DeleteEntityData =>
        new List<object[]>
        {
                new object[] { new Deck { Name = "test deck" } },
        };

        [Theory, MemberData(nameof(DeleteEntityData))]
        public override void DeleteEntity(Deck entity)
        {
            base.DeleteEntity(entity);
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

        public static IEnumerable<object[]> ReadEntityData =>
            new List<object[]>
            {
                new object[] { new News { Title = "test news" } },
            };

        [Theory, MemberData(nameof(ReadEntityData))]
        public override void ReadEntity(News entity)
        {
            base.ReadEntity(entity);
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

        public static IEnumerable<object[]> DeleteEntityData =>
        new List<object[]>
        {
                new object[] { new News { Title = "title", Subtitle = "subtitle", Content = "content" } },
        };

        [Theory, MemberData(nameof(DeleteEntityData))]
        public override void DeleteEntity(News entity)
        {
            base.DeleteEntity(entity);
        }
    }
}