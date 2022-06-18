using Data.Context;
using Data.Models.Implementation;
using Data.Repository.Abstract;
using Data.Repository.Implementation;
using Data.Repository.Interfaces;
using Data.Tools.Sorting;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using Xunit;
using Xunit.Abstractions;
using static Data.Tools.FlashcardTools;
using static Tests.Tools;

namespace Tests.Unit_Tests.Data.Repository
{
    public abstract class GenericRepositoryUnitTests<TEntity, TKey> : IDisposable
        where TEntity : class, IDatabaseItem<TKey>
    {
        protected ITestOutputHelper _output;
        protected GenericRepository<TEntity, TKey, FlashMEMOContext> _repository;
        protected FlashMEMOContext _context;
        protected JsonSerializerSettings _serializerSettings = new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, Formatting = Formatting.Indented };

        public GenericRepositoryUnitTests(ITestOutputHelper output)
        {
            // Terrible design since context and repository are used throughout the class, but only set on the inherited class. Unfortunately, there is no easy/compact way to do this using fixtures(no time to pass to a member of the child class, resulting in the temporary object being disposed) or other methods(can not initialize repository class here since it uses a constructor with parameters).

            // Maybe it would work if the repository classes were declared like Repository<News> or Repository<Deck>, so something like _repository = new Repository<TEntity>(_context) could be used? - The problem from doing this is that this would restrict the child class from running its own tests (i.e., NewsRepository might want to test functions specific to it). 

            // (18/06/2022) It's been so long since I actually checked the comments above that they might as well be completely wrong at this point. FACT: I could use DI just as I use it for controller tests (I didn't implement this here because I didn't know how to at the time), and that would make several things easier.
            _output = output;
            _context = new FlashMEMOContext(new DbContextOptionsBuilder<FlashMEMOContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options, Options.Create(new FlashMEMOContextOptions { SeederPath = "../../../../Data/Seeder", DefaultUserPassword = "Default@Password123" }));
        }

        /// <summary>
        /// Used to copy values between two entity objects while at the same time avoiding that the context starts tracking both of them, which leads to exceptions when running the update method. After setting the property values of <paramref name="destination"/> to be equal to the ones from <paramref name="source"/>, it also makes sure that the Id of the object is not changed during the operation, otherwise EF will assign a third Id value when processing the changes at the database.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        protected virtual void SafeEFEntityValueCopy(TEntity source, TEntity destination)
        {
            source.DbId = destination.DbId; // makes assignment here so the error is not thrown below (i.e., Id gets set to a value different than before)
            _context.Entry(destination).CurrentValues.SetValues(source);
        }

        /// <summary>
        /// Uses context functions to get an entity from the databse (isolate functionality from repository class).
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected T GetEntityViaContext<T, K>(K id) where T : class
        {
            return _context.Set<T>().Find(id);
        }

        /// <summary>
        /// Uses context functions to add an entity to the database (isolate functionality from repository class).
        /// </summary>
        /// <param name="entity"></param>
        protected void AddEntityViaContext<T, K>(T entity) where T : class, IDatabaseItem<K>
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
            _context.Set<T>().Find(entity.DbId).Should().BeEquivalentTo(entity);
        }

        public async virtual void CreateEntity(TEntity entity)
        {
            // Arrange
            // Act
            await _repository.CreateAsync(entity);
            var entityFromRepository = GetEntityViaContext<TEntity, TKey>(entity.DbId);

            // Assert
            entity.Should().BeEquivalentTo(entityFromRepository);
        }

        public async virtual void ReadEntity(TEntity entity)
        {
            // Arrange
            AddEntityViaContext<TEntity, TKey>(entity);

            // Act
            var entityFromRepository = await _repository.GetByIdAsync(entity.DbId);

            // Assert
            entity.Should().BeEquivalentTo(entityFromRepository);
        }
        public async virtual void UpdateEntity(TEntity previousEntity, TEntity updatedEntity)
        {
            // Arrange
            AddEntityViaContext<TEntity, TKey>(previousEntity);

            // Act
            var entityFromRepository = GetEntityViaContext<TEntity, TKey>(previousEntity.DbId);
            SafeEFEntityValueCopy(updatedEntity, entityFromRepository);
            await _repository.UpdateAsync(entityFromRepository);

            // Assert
            entityFromRepository = GetEntityViaContext<TEntity, TKey>(entityFromRepository.DbId);
            entityFromRepository.Should().BeEquivalentTo(updatedEntity);
        }

        public async virtual void DeleteEntity(TEntity entity)
        {
            // Arrange
            AddEntityViaContext<TEntity, TKey>(entity);

            // Act
            await _repository.RemoveByIdAsync(entity.DbId);

            // Assert
            GetEntityViaContext<TEntity, TKey>(entity.DbId).Should().BeNull();
        }

        public virtual void GetAll(TEntity[] entities)
        {
            _output.WriteLine($"Input data has length of {entities.Length} is: {JsonConvert.SerializeObject(entities, _serializerSettings)}");

            // Arrange
            entities.ToList().ForEach(e => AddEntityViaContext<TEntity, TKey>(e));

            // Act
            var entitiesFromRepository = _repository.GetAll();

            // Assert
            entitiesFromRepository.Should().BeEquivalentTo(entities);
            entitiesFromRepository.Should().HaveCount(entities.Length);

            _output.WriteLine($"Data returned by the test method has length of {entitiesFromRepository.Count()} and is: {JsonConvert.SerializeObject(entitiesFromRepository, _serializerSettings)}");
        }

        /// <summary>
        /// Tests the 'SearchAndOrder' endpoint, but only the ordering aspect (easier to show intent of tests).
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="sortOptions"></param>
        public virtual void SearchAndOrder_ValidateOrdering(List<TEntity> entities, GenericSortOptions<TEntity> sortOptions)
        {
            _output.WriteLine($"Sorting requested is: {JsonConvert.SerializeObject(sortOptions)}");
            _output.WriteLine($"Input data has length of {entities.Count} is: {JsonConvert.SerializeObject(entities, _serializerSettings)}");

            // Arrange
            entities.ForEach(e => AddEntityViaContext<TEntity, TKey>(e));

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

            _output.WriteLine($"Data returned by the test method has length of {entitiesFromRepository.Count()} and is: {JsonConvert.SerializeObject(entitiesFromRepository, _serializerSettings)}");
        }

        /// <summary>
        /// Tests the 'SearchAndOrder' endpoint, but only the filtering aspect (easier to show intent of tests).
        /// </summary>
        /// <param name="testData"></param>
        public virtual void SearchAndOrder_ValidateFiltering(ValidateFilteringTestData<TEntity> testData)
        {
            _output.WriteLine($"Filtering requested is: {JsonConvert.SerializeObject(testData.predicate.ToString())}");
            _output.WriteLine($"Input data has length of {testData.entities.Count} is: {JsonConvert.SerializeObject(testData.entities, _serializerSettings)}");

            // Arrange
            testData.entities.ForEach(e => AddEntityViaContext<TEntity, TKey>(e));

            // Act
            var entitiesFromRepository = _repository.SearchAndOrder(testData.predicate, null, -1);

            // Assert
            entitiesFromRepository.Should().BeEquivalentTo(testData.entities.AsQueryable().Where(testData.predicate));

            _output.WriteLine($"Data returned by the test method has length of {entitiesFromRepository.Count()} and is: {JsonConvert.SerializeObject(entitiesFromRepository, _serializerSettings)}");
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

        public static IEnumerable<object[]> CreateEntityData
        {
            get
            {
                yield return new object[] { new Deck { Name = "test", Description = "this is a test deck" } };
                yield return new object[] { new Deck { Name = "test number two", Description = "this is another test deck" } };
            }
        }

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
        private static readonly User TestUser1 = new User() { Email = "testuser1@email.com", UserName = "testuser1" };
        private static readonly User TestUser2 = new User() { Email = "testuser2@email.com", UserName = "testuser2" };

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

        private static readonly Deck TestEntity1 = new() { Name = "test deck 1", CreationDate = DateTime.Now.Subtract(TimeSpan.FromDays(1)), LastUpdated = DateTime.Now.Subtract(TimeSpan.FromDays(1)), OwnerId = TestUser1.Id, Flashcards = new List<Flashcard> { TestFlashcard1, TestFlashcard2, TestFlashcard3, TestFlashcard4 }, Description = "E" };
        private static readonly Deck TestEntity2 = new() { Name = "test deck 2", CreationDate = DateTime.Now.Subtract(TimeSpan.FromDays(2)), LastUpdated = DateTime.Now.Subtract(TimeSpan.FromDays(2)), OwnerId = TestUser1.Id, Flashcards = new List<Flashcard> { TestFlashcard5, TestFlashcard6, TestFlashcard7 }, Description = "D" };
        private static readonly Deck TestEntity3 = new() { Name = "test deck 3", CreationDate = DateTime.Now.Subtract(TimeSpan.FromDays(3)), LastUpdated = DateTime.Now.Subtract(TimeSpan.FromDays(3)), OwnerId = TestUser2.Id, Flashcards = new List<Flashcard> { TestFlashcard8, TestFlashcard9 }, Description = "C" };
        private static readonly Deck TestEntity4 = new() { Name = "test deck 4", CreationDate = DateTime.Now.Subtract(TimeSpan.FromDays(4)), LastUpdated = DateTime.Now.Subtract(TimeSpan.FromDays(4)), OwnerId = TestUser2.Id, Flashcards = new List<Flashcard> { TestFlashcard10 }, Description = "B" };
        private static readonly Deck TestEntity5 = new() { Name = "test deck 5", CreationDate = DateTime.Now.Subtract(TimeSpan.FromDays(5)), LastUpdated = DateTime.Now.Subtract(TimeSpan.FromDays(5)), OwnerId = TestUser2.Id, Flashcards = new List<Flashcard>(), Description = "A" };

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
                //new object[] { new List<Deck>(FullEntityList), new DeckSortOptions(SortType.Ascending, "owner") },
                //new object[] { new List<Deck>(FullEntityList), new DeckSortOptions(SortType.Descending, "owner") },
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
            new object[] { new ValidateFilteringTestData<Deck> { entities = FullEntityList, predicate = _ => true } },
            new object[] { new ValidateFilteringTestData<Deck> { entities = FullEntityList, predicate = e => e.Name == "test deck 1" } },
            //new object[] { new ValidateFilteringTestData<Deck> { entities = FullEntityList, predicate = e => e.Owner.UserName.Contains("user2") } },
            new object[] { new ValidateFilteringTestData<Deck> { entities = FullEntityList, predicate = e => e.Flashcards.Count > 1 } },
            new object[] { new ValidateFilteringTestData<Deck> { entities = FullEntityList, predicate = e => e.Description == "A" || e.Description == "B" } },
            new object[] { new ValidateFilteringTestData<Deck> { entities = FullEntityList, predicate = e => e.CreationDate < DateTime.Now.AddDays(-2) } },
            new object[] { new ValidateFilteringTestData<Deck> { entities = FullEntityList, predicate = e => e.LastUpdated > DateTime.Now } }
        };

        [Theory, MemberData(nameof(SearchAndOrder_ValidateFilteringData))]
        public override void SearchAndOrder_ValidateFiltering(ValidateFilteringTestData<Deck> testData)
        {
            base.SearchAndOrder_ValidateFiltering(testData);
        }
    }

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

        // specific test data
        // using static DateTimes here to try to improve readability (no implicit calculations based on days/whatever)

        private static readonly News TestEntity1 = new News() { Content = "content1", Subtitle = "subtitle1", Title = "title1", LastUpdated = DateTime.Parse("01-01-2001"), CreationDate = DateTime.Parse("01-01-2001") };
        private static readonly News TestEntity2 = new News() { Content = "content2", Subtitle = "subtitle2", Title = "title2", LastUpdated = DateTime.Parse("01-01-2002"), CreationDate = DateTime.Parse("01-01-2002") };
        private static readonly News TestEntity3 = new News() { Content = "content3", Subtitle = "subtitle3", Title = "title3", LastUpdated = DateTime.Parse("01-01-2003"), CreationDate = DateTime.Parse("01-01-2003") };
        private static readonly News TestEntity4 = new News() { Content = "content4", Subtitle = "subtitle4", Title = "title4", LastUpdated = DateTime.Parse("01-01-2004"), CreationDate = DateTime.Parse("01-01-2004") };

        private static readonly List<News> FullEntityList = new() { TestEntity1, TestEntity2, TestEntity3, TestEntity4 };

        public static IEnumerable<object[]> GetAllEntityData =>
        new List<object[]>
        {
                new object[] { FullEntityList.ToArray() }, // full list
                new object[] { new News[] { TestEntity1, TestEntity2, TestEntity3 } }, // only some
                new object[] { new News[] { } } // nothing
        };

        [Theory, MemberData(nameof(GetAllEntityData))]
        public override void GetAll(News[] decks)
        {
            base.GetAll(decks);
        }

        public static IEnumerable<object[]> SearchAndOrder_ValidateOrderingData =>
        new List<object[]>
        {
                new object[] { new List<News>(FullEntityList), new NewsSortOptions(SortType.Ascending, "title") },
                new object[] { new List<News>(FullEntityList), new NewsSortOptions(SortType.Ascending, "subtitle") },
                new object[] { new List<News>(FullEntityList), new NewsSortOptions(SortType.Ascending, "date") },
                new object[] { new List<News>(FullEntityList), new NewsSortOptions(SortType.Descending, "title") },
                new object[] { new List<News>(FullEntityList), new NewsSortOptions(SortType.Descending, "subtitle") },
                new object[] { new List<News>(FullEntityList), new NewsSortOptions(SortType.Descending, "date") },

                // these seem to be missing, should revert to 'title' at the moment
                new object[] { new List<News>(FullEntityList), new NewsSortOptions(SortType.Ascending, "creationdate") },
                new object[] { new List<News>(FullEntityList), new NewsSortOptions(SortType.Ascending, "lastupdated") },
                new object[] { new List<News>(FullEntityList), new NewsSortOptions(SortType.Ascending, "content") },
        };

        [Theory, MemberData(nameof(SearchAndOrder_ValidateOrderingData))]
        public override void SearchAndOrder_ValidateOrdering(List<News> entities, GenericSortOptions<News> sortOptions)
        {
            base.SearchAndOrder_ValidateOrdering(entities, sortOptions);
        }

        public static IEnumerable<object[]> SearchAndOrder_ValidateFilteringData =>
        new List<object[]>
        {
            new object[] { new ValidateFilteringTestData<News> { entities = FullEntityList, predicate = _ => true } },
            new object[] { new ValidateFilteringTestData<News> { entities = FullEntityList, predicate = e => e.Title == "title1" } },
            new object[] { new ValidateFilteringTestData<News> { entities = FullEntityList, predicate = e => e.Title == "title2" } },
            new object[] { new ValidateFilteringTestData<News> { entities = FullEntityList, predicate = e => e.Subtitle == "subtitle3" } },
            new object[] { new ValidateFilteringTestData<News> { entities = FullEntityList, predicate = e => e.Content == "content4" } },
            new object[] { new ValidateFilteringTestData<News> { entities = FullEntityList, predicate = e => e.CreationDate == DateTime.Parse("01-01-2001") } },
            new object[] { new ValidateFilteringTestData<News> { entities = FullEntityList, predicate = e => e.LastUpdated == DateTime.Parse("01-01-2002") } },
            new object[] { new ValidateFilteringTestData<News> { entities = FullEntityList, predicate = e => e.Title == "should return nothing" },
            }
        };

        [Theory, MemberData(nameof(SearchAndOrder_ValidateFilteringData))]
        public override void SearchAndOrder_ValidateFiltering(ValidateFilteringTestData<News> testData)
        {
            base.SearchAndOrder_ValidateFiltering(testData);
        }
    }

    #region Flashcard
    // this class is here just to prove if the concept of the generic class works or not for multiple types
    public class FlashcardRepositoryUnitTests : GenericRepositoryUnitTests<Flashcard, Guid>
    {
        public FlashcardRepositoryUnitTests(ITestOutputHelper output) : base(output)
        {
            _repository = new FlashcardRepository(_context);
        }

        public static IEnumerable<object[]> CreateEntityData =>
            new List<object[]>
            { 
                new object[] { new Flashcard { Level = 0, Answer = "Answer #1", FrontContentLayout = FlashcardContentLayout.SINGLE_BLOCK, Content1 = "<p>Here is some content!</p>", CreationDate = DateTime.Parse("01-01-2001"), BackContentLayout = FlashcardContentLayout.SINGLE_BLOCK, Content4 = "<p>Here is some content!</p>" } },
                new object[] { new Flashcard { Level = 0, FrontContentLayout = FlashcardContentLayout.TRIPLE_BLOCK, Content1 = "<p>Here is some content!</p>", Content2 = "<p>Here is more content!</p>", Content3 = "<p>Here is even more content!</p>", CreationDate = DateTime.Parse("01-01-2001"), BackContentLayout = FlashcardContentLayout.SINGLE_BLOCK, Content4 = "<p>Here is some content!</p>" } },
            };

        [Theory, MemberData(nameof(CreateEntityData))]
        public override void CreateEntity(Flashcard entity)
        {
            base.CreateEntity(entity);
        }

        public static IEnumerable<object[]> ReadEntityData =>
            new List<object[]>
            {
                new object[] { new Flashcard { Level = 0, FrontContentLayout = FlashcardContentLayout.SINGLE_BLOCK, Content1 = "<p>Here is some content!</p>", BackContentLayout = FlashcardContentLayout.SINGLE_BLOCK, Content4 = "<p>Here is some content!</p>" } },
            };

        [Theory, MemberData(nameof(ReadEntityData))]
        public override void ReadEntity(Flashcard entity)
        {
            base.ReadEntity(entity);
        }

        public static IEnumerable<object[]> UpdateEntityData =>
        new List<object[]>
        {
                new object[] { new Flashcard { Level = 0, FrontContentLayout = FlashcardContentLayout.SINGLE_BLOCK, Content1 = "<p>Here is some content!</p>" }, new Flashcard { Level = 1, FrontContentLayout = FlashcardContentLayout.SINGLE_BLOCK, Content1 = "<p>Updated content!</p>", BackContentLayout = FlashcardContentLayout.SINGLE_BLOCK, Content4 = "<p>Here is some content!</p>" } },
                new object[] { new Flashcard { Level = 0, FrontContentLayout = FlashcardContentLayout.SINGLE_BLOCK, Content1 = "<p>Here is some content!</p>" }, new Flashcard { Level = 3, FrontContentLayout = FlashcardContentLayout.TRIPLE_BLOCK, Content1 = "<p>Updated content!</p>", Content2 = "<p>More updated content!</p>", Content3 = "<p>Even more updated content!</p>", BackContentLayout = FlashcardContentLayout.SINGLE_BLOCK, Content4 = "<p>Here is some content!</p>" } },
        };

        [Theory, MemberData(nameof(UpdateEntityData))]
        public override void UpdateEntity(Flashcard previousEntity, Flashcard updatedEntity)
        {
            base.UpdateEntity(previousEntity, updatedEntity);
        }

        public static IEnumerable<object[]> DeleteEntityData =>
        new List<object[]>
        {
                new object[] { new Flashcard { Level = 0, FrontContentLayout = FlashcardContentLayout.SINGLE_BLOCK, Content1 = "<p>Here is some content!</p>" } },
        };

        [Theory, MemberData(nameof(DeleteEntityData))]
        public override void DeleteEntity(Flashcard entity)
        {
            base.DeleteEntity(entity);
        }

        // specific test data
        // using static DateTimes here to try to improve readability (no implicit calculations based on days/whatever)

        private static readonly DateTime ReferenceDate = DateTime.Parse("01-01-2002");

        private static readonly Flashcard TestEntity1 = new Flashcard { Level = 0, FrontContentLayout = FlashcardContentLayout.SINGLE_BLOCK, Content1 = "<p>Here is some content!</p>", LastUpdated = ReferenceDate, CreationDate = ReferenceDate, DueDate = ReferenceDate, BackContentLayout = FlashcardContentLayout.SINGLE_BLOCK, Content4 = "<p>Here is some content!</p>" };
        private static readonly Flashcard TestEntity2 = new Flashcard { Level = 1, FrontContentLayout = FlashcardContentLayout.VERTICAL_SPLIT, Content1 = "<p>Here is some content!</p>", Content2 = "<p>Here is some content!2</p>", LastUpdated = ReferenceDate.AddDays(1), CreationDate = ReferenceDate, DueDate = ReferenceDate.AddDays(2), BackContentLayout = FlashcardContentLayout.SINGLE_BLOCK, Content4 = "<p>Here is some content!</p>" };
        private static readonly Flashcard TestEntity3 = new Flashcard { Level = 2, Answer = "Answer #1", FrontContentLayout = FlashcardContentLayout.TRIPLE_BLOCK, Content1 = "<p>Here is some content!</p>", Content2 = "<p>Here is some content!2</p>", Content3 = "<p>Here is some content!3</p>", LastUpdated = ReferenceDate.AddDays(2), CreationDate = ReferenceDate, DueDate = ReferenceDate.AddDays(4), BackContentLayout = FlashcardContentLayout.SINGLE_BLOCK, Content4 = "<p>Here is some content!</p>" };
        private static readonly Flashcard TestEntity4 = new Flashcard { Level = 3, Answer = "Answer #2", FrontContentLayout = FlashcardContentLayout.FULL_CARD, Content1 = "<p>Here is some content!</p>", Content2 = "<p>Here is some content!2</p>", Content3 = "<p>Here is some content!3</p>", LastUpdated = ReferenceDate.AddDays(2), CreationDate = ReferenceDate.AddDays(2), DueDate = ReferenceDate.AddDays(5), BackContentLayout = FlashcardContentLayout.SINGLE_BLOCK, Content4 = "<p>Here is some content!</p>" };
        private static readonly Flashcard TestEntity5 = new Flashcard { Level = 4, Answer = "Answer #3", FrontContentLayout = FlashcardContentLayout.SINGLE_BLOCK, Content1 = "<p>Here is some content!</p>", LastUpdated = ReferenceDate.AddDays(4), CreationDate = ReferenceDate.AddDays(4), DueDate = ReferenceDate.AddDays(19), BackContentLayout = FlashcardContentLayout.HORIZONTAL_SPLIT, Content4 = "<p>Here is some content!</p>", Content5 = "<p>Here is some content!</p>" };
        private static readonly Flashcard TestEntity6 = new Flashcard { Level = 5, FrontContentLayout = FlashcardContentLayout.FULL_CARD, Content1 = "<p>Here is some content!</p>", Content2 = "<p>Here is some content!2</p>", Content3 = "<p>Here is some content!3</p>", LastUpdated = ReferenceDate.AddDays(2), CreationDate = ReferenceDate.AddDays(2), DueDate = ReferenceDate.AddDays(5), BackContentLayout = FlashcardContentLayout.FULL_CARD, Content4 = "<p>Here is some content!</p>", Content5 = "<p>Here is some content!</p>", Content6 = "<p>Here is some content!</p>" };

        private static readonly List<Flashcard> FullEntityList = new() { TestEntity1, TestEntity2, TestEntity3, TestEntity4, TestEntity5, TestEntity6 };

        public static IEnumerable<object[]> GetAllEntityData =>
        new List<object[]>
        {
                new object[] { FullEntityList.ToArray() }, // full list
                new object[] { new Flashcard[] { TestEntity1, TestEntity2, TestEntity3, TestEntity4 } }, // only some
                new object[] { new Flashcard[] { } } // nothing
        };

        [Theory, MemberData(nameof(GetAllEntityData))]
        public override void GetAll(Flashcard[] entities)
        {
            base.GetAll(entities);
        }

        public static IEnumerable<object[]> SearchAndOrder_ValidateOrderingData =>
        new List<object[]>
        {
                new object[] { new List<Flashcard>(FullEntityList), new FlashcardSortOptions(SortType.Ascending, "duedate") },
                new object[] { new List<Flashcard>(FullEntityList), new FlashcardSortOptions(SortType.Descending, "duedate") },
                new object[] { new List<Flashcard>(FullEntityList), new FlashcardSortOptions(SortType.Ascending, "level") },
                new object[] { new List<Flashcard>(FullEntityList), new FlashcardSortOptions(SortType.Descending, "level") },
                new object[] { new List<Flashcard>(FullEntityList), new FlashcardSortOptions(SortType.Ascending, "answer") },
                new object[] { new List<Flashcard>(FullEntityList), new FlashcardSortOptions(SortType.Descending, "answer") },
                new object[] { new List<Flashcard>(FullEntityList), new FlashcardSortOptions(SortType.Ascending, "creationdate") },
                new object[] { new List<Flashcard>(FullEntityList), new FlashcardSortOptions(SortType.Descending, "creationdate") },
                new object[] { new List<Flashcard>(FullEntityList), new FlashcardSortOptions(SortType.Ascending, "lastupdated") },
                new object[] { new List<Flashcard>(FullEntityList), new FlashcardSortOptions(SortType.Descending, "lastupdated") },
                new object[] { new List<Flashcard>(FullEntityList), new FlashcardSortOptions(SortType.Ascending, "invalid") },
                new object[] { new List<Flashcard>(FullEntityList), new FlashcardSortOptions(SortType.Descending, "invalid") },

        };

        [Theory, MemberData(nameof(SearchAndOrder_ValidateOrderingData))]
        public override void SearchAndOrder_ValidateOrdering(List<Flashcard> entities, GenericSortOptions<Flashcard> sortOptions)
        {
            base.SearchAndOrder_ValidateOrdering(entities, sortOptions);
        }

        public static IEnumerable<object[]> SearchAndOrder_ValidateFilteringData =>
        new List<object[]>
        {
            new object[] { new ValidateFilteringTestData<Flashcard> { entities = FullEntityList, predicate = _ => true } },
            new object[] { new ValidateFilteringTestData<Flashcard> { entities = FullEntityList, predicate = e => e.Level == 1 } },
            new object[] { new ValidateFilteringTestData<Flashcard> { entities = FullEntityList, predicate = e => e.Answer == "Answer #1" } },
            new object[] { new ValidateFilteringTestData<Flashcard> { entities = FullEntityList, predicate = e => e.Content1 == "<p>Here is some content!</p>" } },
            new object[] { new ValidateFilteringTestData<Flashcard> { entities = FullEntityList, predicate = e => e.Content2 == "<p>Here is some content!2</p>" } },
            new object[] { new ValidateFilteringTestData<Flashcard> { entities = FullEntityList, predicate = e => e.Content1 == "<p>Here is some content!</p>" && e.Content6 == "<p>Here is some content!</p>" } },
            new object[] { new ValidateFilteringTestData<Flashcard> { entities = FullEntityList, predicate = e => e.CreationDate > ReferenceDate.AddDays(-360) } },
            new object[] { new ValidateFilteringTestData<Flashcard> { entities = FullEntityList, predicate = e => e.LastUpdated == ReferenceDate } },
            new object[] { new ValidateFilteringTestData<Flashcard> { entities = FullEntityList, predicate = e => e.DueDate < ReferenceDate.AddDays(2) } },
        };


        [Theory, MemberData(nameof(SearchAndOrder_ValidateFilteringData))]
        public override void SearchAndOrder_ValidateFiltering(ValidateFilteringTestData<Flashcard> testData)
        {
            base.SearchAndOrder_ValidateFiltering(testData);
        }
    }
    #endregion

    public class UserRepositoryUnitTests : GenericRepositoryUnitTests<User, string>
    {
        public UserRepositoryUnitTests(ITestOutputHelper output) : base(output)
        {
            // stole this implementation from what was being used in the now deprecated UserRepository tests, which seems to be taken from here: https://stackoverflow.com/questions/38253607/creating-a-usermanager-outside-of-built-in-dependency-injection-system
            var userManager = new UserManager<User>(
                new UserStore<User>(_context),
                null,
                new PasswordHasher<User>(),
                null,
                null,
                null,
                null,
                null,
                null);
            _repository = new UserRepository(_context, userManager);

            AssertionOptions.AssertEquivalencyUsing(o => o.Excluding(p => p.Name.ToLower().Contains("concurrencystamp"))); // This is necessary so 'ConcurrencyStamp' properties don't throw useless comparison exceptions during testing the Identity classes.
        }

         // I changed the implementation a bit here to reduce code duplication

        public static IEnumerable<object[]> CreateAndReadEntityData =>
            new List<object[]>
            {
                new object[] { TestUser1 },
                new object[] { TestUser2 },
            };

        [Theory, MemberData(nameof(CreateAndReadEntityData))]
        public override void CreateEntity(User entity)
        {
            base.CreateEntity(entity);
        }

        [Theory, MemberData(nameof(CreateAndReadEntityData))]
        public override void ReadEntity(User entity)
        {
            base.ReadEntity(entity);
        }

        public static IEnumerable<object[]> UpdateEntityData =>
        new List<object[]>
        {
                new object[] {TestUser1, new User() { Name = "newName", Surname = "newSurname" } },
        };

        /// <summary>
        /// I override this method so I can alter the way stamp properties are handled during the value copy, since mismatched values end up throwing concurrency exceptions by EF Core.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        protected override void SafeEFEntityValueCopy(User source, User destination)
        {
            source.DbId = destination.DbId;
            source.ConcurrencyStamp = destination.ConcurrencyStamp;

            _context.Entry(destination).CurrentValues.SetValues(source);
        }

        [Theory, MemberData(nameof(UpdateEntityData))]
        public override void UpdateEntity(User previousEntity, User updatedEntity)
        {
            base.UpdateEntity(previousEntity, updatedEntity);
        }

        public static IEnumerable<object[]> DeleteEntityData =>
        new List<object[]>
        {
                new object[] { TestUser1 },
                new object[] { TestUser2 },
        };

        [Theory, MemberData(nameof(DeleteEntityData))]
        public override void DeleteEntity(User entity)
        {
            base.DeleteEntity(entity);
        }

        // specific test data
        private static User TestUser1 = new User { Email = "test@email.com", NormalizedEmail = "test@email.com", Name = "test", Surname = "user", UserName = "testuser", NormalizedUserName = "testuser" };
        private static User TestUser2 = new User { Email = "test2@email.com", NormalizedEmail = "test2@email.com", Name = "test2", Surname = "user", UserName = "testuser2", NormalizedUserName = "testuser2" };
        private static User TestUser3 = new User { Email = "test3@email.com", NormalizedEmail = "test3@email.com", Name = "test3", Surname = "user", UserName = "testuser3", NormalizedUserName = "testuser3" };
        private static User TestUser4 = new User { Email = "test4@email.com", NormalizedEmail = "test4@email.com", Name = "test4", Surname = "user", UserName = "testuser4", NormalizedUserName = "testuser4" };
        private static User TestUser5 = new User { Email = "test5@email.com", NormalizedEmail = "test5@email.com", Name = "test5", Surname = "user", UserName = "testuser5", NormalizedUserName = "testuser5" };

        private static readonly List<User> FullEntityList = new() { TestUser1, TestUser2, TestUser3, TestUser4, TestUser5 };

        public static IEnumerable<object[]> GetAllEntityData =>
        new List<object[]>
        {
                new object[] { FullEntityList.ToArray() }, // full list
                new object[] { new User[] { TestUser1, TestUser2, TestUser3 } }, // only some
                new object[] { new User[] { } } // nothing
        };

        [Theory, MemberData(nameof(GetAllEntityData))]
        public override void GetAll(User[] users)
        {
            base.GetAll(users);
        }

        public static IEnumerable<object[]> SearchAndOrder_ValidateOrderingData =>
        new List<object[]>
        {
                new object[] { new List<User>(FullEntityList), new UserSortOptions(SortType.Ascending, "email") },
                new object[] { new List<User>(FullEntityList), new UserSortOptions(SortType.Descending, "email") },
                new object[] { new List<User>(FullEntityList), new UserSortOptions(SortType.Ascending, "username") },
                new object[] { new List<User>(FullEntityList), new UserSortOptions(SortType.Descending, "username") },
                new object[] { new List<User>(FullEntityList), new UserSortOptions(SortType.Ascending, "default") }, // should go to 'email'
                new object[] { new List<User>(FullEntityList), new UserSortOptions(SortType.Descending, "default") }, // should go to 'email'
        };

        [Theory, MemberData(nameof(SearchAndOrder_ValidateOrderingData))]
        public override void SearchAndOrder_ValidateOrdering(List<User> entities, GenericSortOptions<User> sortOptions)
        {
            base.SearchAndOrder_ValidateOrdering(entities, sortOptions);
        }

        public static IEnumerable<object[]> SearchAndOrder_ValidateFilteringData =>
        new List<object[]>
        {
            new object[] { new ValidateFilteringTestData<User> { entities = FullEntityList, predicate = _ => true } },
            new object[] { new ValidateFilteringTestData<User> { entities = FullEntityList, predicate = e => e.Email == "test@email.com" } },
            new object[] { new ValidateFilteringTestData<User> { entities = FullEntityList, predicate = e => e.UserName == "testuser2" } },
            new object[] { new ValidateFilteringTestData<User> { entities = FullEntityList, predicate = e => e.Email.Contains("email.com") } },
        };

        [Theory, MemberData(nameof(SearchAndOrder_ValidateFilteringData))]
        public override void SearchAndOrder_ValidateFiltering(ValidateFilteringTestData<User> testData)
        {
            base.SearchAndOrder_ValidateFiltering(testData);
        }

        public static Deck TestDeck1 = new Deck() { Name = "Test Deck", Description = "Test Description", LanguageISOCode = "en", OwnerId = TestUser1.Id, };

        public static Flashcard TestFlashcard1 = new Flashcard() { Content1 = "Front 1", Content4 = "Back 1", FrontContentLayout = FlashcardContentLayout.SINGLE_BLOCK, BackContentLayout = FlashcardContentLayout.SINGLE_BLOCK, Answer = "Answer 1", Level = 1, DeckId = TestDeck1.DeckId };
        public static Flashcard TestFlashcard2 = new Flashcard() { Content1 = "Front 2", Content4 = "Back 2", FrontContentLayout = FlashcardContentLayout.SINGLE_BLOCK, BackContentLayout = FlashcardContentLayout.SINGLE_BLOCK, Answer = "Answer 2", Level = 1, DeckId = TestDeck1.DeckId };
        public static Flashcard TestFlashcard3 = new Flashcard() { Content1 = "Front 3", Content4 = "Back 3", FrontContentLayout = FlashcardContentLayout.SINGLE_BLOCK, BackContentLayout = FlashcardContentLayout.SINGLE_BLOCK, Answer = "Answer 3", Level = 1, DeckId = TestDeck1.DeckId };

        public static IEnumerable<object[]> CascadeDeleteData =>
        new List<object[]>
        {
            new object[] { TestUser1, TestDeck1, new List<Flashcard>() { TestFlashcard1, TestFlashcard2, TestFlashcard3 } }
        };

        [Theory, MemberData(nameof(CascadeDeleteData))]
        public async void CascadeDelete(User user, Deck deck, List<Flashcard> flashcards)
        {
            // Arrange
            AddEntityViaContext<User, string>(user);
            deck.OwnerId = user.Id; // Doing this just for good measure
            AddEntityViaContext<Deck, Guid>(deck);
            flashcards.ForEach(flashcard =>
            {
                flashcard.DeckId = deck.DeckId; // Doing this also for good measure
                AddEntityViaContext<Flashcard, Guid>(flashcard);
            });

            // Act
            await _repository.RemoveByIdAsync(user.Id);

            // Assert
            GetEntityViaContext<User, string>(user.Id).Should().BeNull();
            GetEntityViaContext<Deck, Guid>(deck.DeckId).Should().BeNull();
            flashcards.ForEach(flashcard =>
            {
                GetEntityViaContext<Flashcard, Guid>(flashcard.FlashcardId).Should().BeNull();
            });
        }
    }
}
