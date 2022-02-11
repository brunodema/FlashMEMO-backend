using Data.Context;
using Data.Models.Implementation;
using Data.Repository.Abstract;
using Data.Repository.Implementation;
using Data.Repository.Interfaces;
using Data.Tools.Sorting;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using Xunit;
using Xunit.Abstractions;

namespace Tests.Unit_Tests.Data.Repository
{
    public abstract class GenericRepositoryUnitTests<TEntity, TKey> : IDisposable
        where TEntity : class, IDatabaseItem<TKey>
    {
        protected ITestOutputHelper _output;
        protected GenericRepository<TEntity, TKey, FlashMEMOContext> _repository;
        protected FlashMEMOContext _context;

        public GenericRepositoryUnitTests(ITestOutputHelper output)
        {
            //terrible design since context and repository are used throughout the class, but only set on the inherited class. Unfortunately, there is no easy/compact way to do this using fixtures(no time to pass to a member of the child class, resulting in the temporary object being disposed) or other methods(can not initialize repository class here since it uses a constructor with parameters).

            // maybe it would work if the repository classes were declared like Repository<News> or Repository<Deck>, so something like _repository = new Repository<TEntity>(_context) could be used? - The problem from doing this is that this would restrict the child class from running its own tests (i.e., NewsRepository might want to test functions specific to it). 
            _output = output;
            _context = new FlashMEMOContext(new DbContextOptionsBuilder<FlashMEMOContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options);
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
            _context.Set<TEntity>().Find(entity.DbId).Should().BeEquivalentTo(entity);
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

        public virtual void GetAll(TEntity[] entities)
        {
            // Arrange
            entities.ToList().ForEach(e => AddEntityViaContext(e));

            // Act
            var entitiesFromRepository = _repository.GetAll();

            // Assert
            entitiesFromRepository.Should().BeEquivalentTo(entities);
            entitiesFromRepository.Should().HaveCount(entities.Length);
        }

        /// <summary>
        /// Tests the 'SearchAndOrder' endpoint, but only the ordering aspect (easier to show intent of tests).
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="sortOptions"></param>
        public virtual void SearchAndOrder_ValidateOrdering(List<TEntity> entities, GenericSortOptions<TEntity> sortOptions)
        {
            // Arrange
            entities.ForEach(e => AddEntityViaContext(e));

            // Act
            var entitiesFromRepository = _repository.SearchAndOrder(_ => true, sortOptions, -1);

            // Assert
            entitiesFromRepository.Should().BeEquivalentTo(entities).And.HaveCount(entities.Count);

            switch (sortOptions.SortType)
            {
                case SortType.Ascending:
                    entitiesFromRepository.Should().BeInAscendingOrder(sortOptions.GetColumnToSort());
                    break;
                case SortType.Descending:
                    entitiesFromRepository.Should().BeInDescendingOrder(sortOptions.GetColumnToSort());
                    break;
            }
        }

        /// <summary>
        /// This class is only here because C# is dumb and does not allow Expressions to be declared in loco. Therefore, I must create a strongly-typed object so that the lambda (Func) I declare later is automatically converted into an Expression.
        /// </summary>
        public class ValidateFilteringTestData
        {
            public List<TEntity> entities { get; set; }
            public Expression<Func<TEntity, bool>> predicate { get; set; }
        }

        /// <summary>
        /// Tests the 'SearchAndOrder' endpoint, but only the filtering aspect (easier to show intent of tests).
        /// </summary>
        /// <param name="testData"></param>
        public virtual void SearchAndOrder_ValidateFiltering(ValidateFilteringTestData testData)
        {
            // Arrange
            testData.entities.ForEach(e => AddEntityViaContext(e));

            // Act
            var entitiesFromRepository = _repository.SearchAndOrder(testData.predicate, null, -1);

            // Assert
            entitiesFromRepository.Should().BeEquivalentTo(testData.entities.AsQueryable().Where(testData.predicate));
        }

        public void Dispose()
        {
            _repository?.Dispose();
            _context?.Dispose();
        }
    }

    public class DeckRepositoryUnitTests : GenericRepositoryUnitTests<Deck, Guid>
    {
        public DeckRepositoryUnitTests(ITestOutputHelper output) : base(output)
        {
            _repository = new DeckRepository(_context);
        }

        public static IEnumerable<object[]> CreateEntityData =>
            new List<object[]>
            {
                new object[] { new Deck { Name = "test", Description = "this is a test deck" } },
                new object[] { new Deck { Name = "test number two", Description = "this is another test deck" } },
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

        // test data
        private static readonly ApplicationUser TestUser1 = new ApplicationUser() { Email = "testuser1@email.com", UserName = "testuser1" };
        private static readonly ApplicationUser TestUser2 = new ApplicationUser() { Email = "testuser2@email.com", UserName = "testuser2" };

        // 10 different flashcards since one deck can have many flashcards, but a flashcard can have only one deck (thinking about business rules, a flashcard should be copied from a deck to another, if necessary)
        private static readonly Flashcard TestFlashcard1 = new() { };
        private static readonly Flashcard TestFlashcard2 = new() { };
        private static readonly Flashcard TestFlashcard3 = new() { };
        private static readonly Flashcard TestFlashcard4 = new() { };
        private static readonly Flashcard TestFlashcard5 = new() { };
        private static readonly Flashcard TestFlashcard6 = new() { };
        private static readonly Flashcard TestFlashcard7 = new() { };
        private static readonly Flashcard TestFlashcard8 = new() { };
        private static readonly Flashcard TestFlashcard9 = new() { };
        private static readonly Flashcard TestFlashcard10 = new() { };

        private static readonly Deck TestEntity1 = new() { Name = "test deck 1", CreationDate = DateTime.Now.Subtract(TimeSpan.FromDays(1)), LastUpdated = DateTime.Now.Subtract(TimeSpan.FromDays(1)), Owner = TestUser1, Flashcards = new List<Flashcard> { TestFlashcard1, TestFlashcard2, TestFlashcard3, TestFlashcard4 }, Description = "E" };
        private static readonly Deck TestEntity2 = new() { Name = "test deck 2", CreationDate = DateTime.Now.Subtract(TimeSpan.FromDays(2)), LastUpdated = DateTime.Now.Subtract(TimeSpan.FromDays(2)), Owner = TestUser1, Flashcards = new List<Flashcard> { TestFlashcard5, TestFlashcard6, TestFlashcard7 }, Description = "D" };
        private static readonly Deck TestEntity3 = new() { Name = "test deck 3", CreationDate = DateTime.Now.Subtract(TimeSpan.FromDays(3)), LastUpdated = DateTime.Now.Subtract(TimeSpan.FromDays(3)), Owner = TestUser2, Flashcards = new List<Flashcard> { TestFlashcard8, TestFlashcard9 }, Description = "C" };
        private static readonly Deck TestEntity4 = new() { Name = "test deck 4", CreationDate = DateTime.Now.Subtract(TimeSpan.FromDays(4)), LastUpdated = DateTime.Now.Subtract(TimeSpan.FromDays(4)), Owner = TestUser2, Flashcards = new List<Flashcard> { TestFlashcard10 }, Description = "B" };
        private static readonly Deck TestEntity5 = new() { Name = "test deck 5", CreationDate = DateTime.Now.Subtract(TimeSpan.FromDays(5)), LastUpdated = DateTime.Now.Subtract(TimeSpan.FromDays(5)), Owner = TestUser2, Flashcards = new List<Flashcard>(), Description = "A" };

        private static readonly List<Deck> FullEntityList = new() { TestEntity1, TestEntity2, TestEntity3, TestEntity4, TestEntity5 };

        public static IEnumerable<object[]> GetAllEntityData =>
        new List<object[]>
        {
                new object[] { FullEntityList.ToArray() }, // full list
                new object[] { new Deck[] { TestEntity1, TestEntity2, TestEntity3 } }, // only some
                new object[] { new Deck[] { } } // nothing
        };

        [Theory, MemberData(nameof(GetAllEntityData))]
        public override void GetAll(Deck[] decks)
        {
            base.GetAll(decks);
        }

        public static IEnumerable<object[]> SearchAndOrder_ValidateOrderingData =>
        new List<object[]>
        {
                new object[] { new List<Deck>(FullEntityList), new DeckSortOptions(SortType.Ascending, "name") },
                new object[] { new List<Deck>(FullEntityList), new DeckSortOptions(SortType.Descending, "name") },
                new object[] { new List<Deck>(FullEntityList), new DeckSortOptions(SortType.Ascending, "flashcards") },
                new object[] { new List<Deck>(FullEntityList), new DeckSortOptions(SortType.Descending, "flashcards") },
                new object[] { new List<Deck>(FullEntityList), new DeckSortOptions(SortType.Ascending, "description") },
                new object[] { new List<Deck>(FullEntityList), new DeckSortOptions(SortType.Descending, "description") },
                new object[] { new List<Deck>(FullEntityList), new DeckSortOptions(SortType.Ascending, "owner") },
                new object[] { new List<Deck>(FullEntityList), new DeckSortOptions(SortType.Descending, "owner") },
                new object[] { new List<Deck>(FullEntityList), new DeckSortOptions(SortType.Ascending, "creationdate") },
                new object[] { new List<Deck>(FullEntityList), new DeckSortOptions(SortType.Descending, "creationdate") },
                new object[] { new List<Deck>(FullEntityList), new DeckSortOptions(SortType.Ascending, "lastupdated") },
                new object[] { new List<Deck>(FullEntityList), new DeckSortOptions(SortType.Descending, "lastupdated") },
        };

        [Theory, MemberData(nameof(SearchAndOrder_ValidateOrderingData))]
        public override void SearchAndOrder_ValidateOrdering(List<Deck> entities, GenericSortOptions<Deck> sortOptions)
        {
            base.SearchAndOrder_ValidateOrdering(entities, sortOptions);
        }

        public static IEnumerable<object[]> SearchAndOrder_ValidateFilteringData =>
        new List<object[]>
        {
            new object[] { new ValidateFilteringTestData { entities = FullEntityList, predicate = _ => true } },
            new object[] { new ValidateFilteringTestData { entities = FullEntityList, predicate = e => e.Name == "test deck 1" } },
            new object[] { new ValidateFilteringTestData { entities = FullEntityList, predicate = e => e.Owner.UserName.Contains("user2") } },
            new object[] { new ValidateFilteringTestData { entities = FullEntityList, predicate = e => e.FlashcardCount > 1 } },
            new object[] { new ValidateFilteringTestData { entities = FullEntityList, predicate = e => e.Description == "A" || e.Description == "B" } },
            new object[] { new ValidateFilteringTestData { entities = FullEntityList, predicate = e => e.CreationDate < DateTime.Now.AddDays(-2) } },
            new object[] { new ValidateFilteringTestData { entities = FullEntityList, predicate = e => e.LastUpdated > DateTime.Now } }
        };

        [Theory, MemberData(nameof(SearchAndOrder_ValidateFilteringData))]
        public override void SearchAndOrder_ValidateFiltering(ValidateFilteringTestData testData)
        {
            base.SearchAndOrder_ValidateFiltering(testData);
        }
    }

    // this class is here just to prove if the concept of the generic class works or not for multiple types
    public class NewsRepositoryUnitTests : GenericRepositoryUnitTests<News, Guid>
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
