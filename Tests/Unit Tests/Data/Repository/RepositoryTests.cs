using Data.Context;
using Data.Models.Implementation;
using Data.Repository.Abstract;
using Data.Repository.Implementation;
using Data.Repository.Interfaces;
using Data.Tools.Sorting;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;
using Xunit.Abstractions;

namespace Tests.Unit_Tests.Data.Repository
{
    public abstract class GenericRepositoryUnitTests<TEntity, TKey>  : IDisposable
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

        public virtual void GetAll(List<TEntity> entities)
        {
            // Arrange
            entities.ForEach(e => AddEntityViaContext(e));

            // Act
            var entitiesFromRepository = _repository.GetAll();

            // Assert
            entitiesFromRepository.Should().BeEquivalentTo(entities);
            entitiesFromRepository.Should().HaveCount(entities.Count);
        }

        // guess what: more parallelization problems with xunit! ('GetAll' fails when run together with all tests)
        public class SearchAndOrderTestData
        {
            public List<TEntity> entities { get; set; }
            public List<TEntity> expectedEntities { get; set; }
            public Expression<Func<TEntity, bool>> predicate { get; set; }
            public GenericSortOptions<TEntity> sortOptions { get; set; }
            public int numRecords { get; set; }
        }

        public virtual void SearchAndOrder(SearchAndOrderTestData data)
        {
            // Arrange
            data.entities.ForEach(e => AddEntityViaContext(e));

            // Act
            var entitiesFromRepository = _repository.SearchAndOrder(data.predicate, data.sortOptions, data.numRecords);

            // Assert
            if (data.sortOptions is null)
            {
                entitiesFromRepository.Should().BeEquivalentTo(data.expectedEntities.Take(data.numRecords)); // no need for strict ordering if no sorting was specified
            }
            else
            {
                entitiesFromRepository.Should().BeEquivalentTo(data.expectedEntities.Take(data.numRecords), opt => opt.WithStrictOrdering());
            }
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

        // test data
        private static readonly ApplicationUser TestUser1 = new ApplicationUser() { Email = "testuser1@email.com", UserName = "testuser1" };
        private static readonly ApplicationUser TestUser2 = new ApplicationUser() { Email = "testuser2@email.com", UserName = "testuser2" };

        private static readonly Flashcard TestFlashcard1 = new() { Level = 1, FrontContent = "front1", BackContent = "back1", CreationDate = DateTime.Now.Subtract(TimeSpan.FromDays(1)), DueDate = DateTime.Now.Add(TimeSpan.FromDays(1)), LastUpdated = DateTime.Now.Subtract(TimeSpan.FromDays(1)) };
        private static readonly Flashcard TestFlashcard2 = new() { Level = 1, FrontContent = "front2", BackContent = "back2", CreationDate = DateTime.Now.Subtract(TimeSpan.FromDays(2)), DueDate = DateTime.Now.Add(TimeSpan.FromDays(2)), LastUpdated = DateTime.Now.Subtract(TimeSpan.FromDays(2)) };
        private static readonly Flashcard TestFlashcard3 = new() { Level = 1, FrontContent = "front3", BackContent = "back3", CreationDate = DateTime.Now.Subtract(TimeSpan.FromDays(3)), DueDate = DateTime.Now.Add(TimeSpan.FromDays(3)), LastUpdated = DateTime.Now.Subtract(TimeSpan.FromDays(3)) };

        private static readonly List<Flashcard> FullFlashcardList = new() { TestFlashcard1, TestFlashcard2, TestFlashcard3 };

        private static readonly Deck TestEntity1 = new() { Name = "test deck 1", CreationDate = DateTime.Now.Subtract(TimeSpan.FromDays(1)), LastUpdated = DateTime.Now.Subtract(TimeSpan.FromDays(1)), Owner = TestUser1, Flashcards = FullFlashcardList, Description = "E" };
        private static readonly Deck TestEntity2 = new() { Name = "test deck 2", CreationDate = DateTime.Now.Subtract(TimeSpan.FromDays(2)), LastUpdated = DateTime.Now.Subtract(TimeSpan.FromDays(2)), Owner = TestUser1, Flashcards = new List<Flashcard> { TestFlashcard1 }, Description = "D" };
        private static readonly Deck TestEntity3 = new() { Name = "test deck 3", CreationDate = DateTime.Now.Subtract(TimeSpan.FromDays(3)), LastUpdated = DateTime.Now.Subtract(TimeSpan.FromDays(3)), Owner = TestUser2, Flashcards = FullFlashcardList, Description = "C" };
        private static readonly Deck TestEntity4 = new() { Name = "test deck 4", CreationDate = DateTime.Now.Subtract(TimeSpan.FromDays(4)), LastUpdated = DateTime.Now.Subtract(TimeSpan.FromDays(4)), Owner = TestUser2, Flashcards = new List<Flashcard> { TestFlashcard2 }, Description = "B" };
        private static readonly Deck TestEntity5 = new() { Name = "test deck 5", CreationDate = DateTime.Now.Subtract(TimeSpan.FromDays(5)), LastUpdated = DateTime.Now.Subtract(TimeSpan.FromDays(5)), Owner = TestUser2, Flashcards = new List<Flashcard> { TestFlashcard3 }, Description = "A" };

        private static readonly List<Deck> FullEntityList = new() { TestEntity1, TestEntity2, TestEntity3, TestEntity4, TestEntity5 };

        public static IEnumerable<object[]> GetAllEntityData =>
        new List<object[]>
        {
                new object[] { new List<Deck>(FullEntityList) }, // full list
                new object[] { new List<Deck>() { TestEntity1, TestEntity2, TestEntity3 } }, // only some
                new object[] { new List<Deck>() { } } // nothing
        };

        [Theory, MemberData(nameof(GetAllEntityData))]
        public override void GetAll(List<Deck> decks)
        {
            base.GetAll(decks);
        }

        // I will be explicitly stating the expected results to better visualize the expected behavior of these tests. I could easilly just used LINQ functions for this, but I think that doing this way is more 'conceptually correct'.
        public static IEnumerable<object[]> SearchAndOrderEntityData =>
        new List<object[]>
        {
                new object[] {
                    new SearchAndOrderTestData
                    {
                        entities = new List<Deck>(FullEntityList),
                        expectedEntities = new List<Deck> { TestEntity5, TestEntity4, TestEntity3, TestEntity2, TestEntity1 }, // should order properly
                        predicate = _ => true,
                        sortOptions = new DeckSortOptions(SortType.Descending, "name"),
                        numRecords = 10
                    },
                },
                new object[] {
                    new SearchAndOrderTestData
                    {
                        entities = new List<Deck>(FullEntityList),
                        expectedEntities = new List<Deck> { TestEntity5, TestEntity4, TestEntity3, TestEntity2, TestEntity1 }, // should order properly
                        predicate = _ => true,
                        sortOptions = new DeckSortOptions(SortType.Ascending, "description"),
                        numRecords = 10
                    },
                },
                new object[] {
                    new SearchAndOrderTestData
                    {
                        entities = new List<Deck>(FullEntityList),
                        expectedEntities = new List<Deck> { TestEntity5, TestEntity4, },
                        predicate = _ => true,
                        sortOptions = new DeckSortOptions(SortType.Ascending, "description"),
                        numRecords = 2 // same as previous test, but with limited nubmer of records
                    },
                },
                new object[] {
                    new SearchAndOrderTestData
                    {
                        entities = new List<Deck>(FullEntityList),
                        expectedEntities = new List<Deck> { TestEntity5, TestEntity4 },
                        predicate = e => e.Description == "A" || e.Description == "B", // should filter by property
                        sortOptions = null,
                        numRecords = 10
                    },
                }
        };

        [Theory, MemberData(nameof(SearchAndOrderEntityData))]
        public override void SearchAndOrder(SearchAndOrderTestData data)
        {
            base.SearchAndOrder(data);
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
