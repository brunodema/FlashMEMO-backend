﻿using Data.Context;
using Data.Models.Implementation;
using Data.Repository.Abstract;
using Data.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tests.Integration.Fixtures;
using Xunit;
using System.Net.Http.Json;
using API.ViewModels;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Data.Repository.Implementation;
using Xunit.Abstractions;
using Data.Models.DTOs;
using static Tests.Tools;
using Newtonsoft.Json;
using Business.Services.Implementation;
using static Data.Models.Implementation.StaticModels;
using Business.Tools.Validations;

namespace Tests.Tests.Integration.Abstract
{
    public class GenericControllerTests<T, TKey, TDTO> : IClassFixture<IntegrationTestFixture>
        where T : class, IDatabaseItem<TKey>, new()
        where TDTO : IModelDTO<T, TKey>
    {
        protected IntegrationTestFixture _fixture;
        protected HttpClient _client;
        protected ITestOutputHelper _output;
        protected JsonSerializerSettings _serializerSettings = new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, Formatting = Formatting.Indented };

        protected string _baseEndpoint = $"/api/v1/{typeof(T).Name}";
        protected string _createEndpoint;
        protected string _deleteEndpoint;
        protected string _listEndpoint;
        protected string _searchEndpoint;

        /// <summary>
        /// This disgusting implementation is required because (1) no 'AddOrUpdate' method exists in the EF Core stuff anymore (despite claims of it on the internet), and because (2) the 'Update' method doesn't actually add instead of updating when providing a non-existent object (it should, though) 
        /// </summary>
        /// <param name="entity"></param>
        protected void AddIfNecessary<Type, Key>(Type entity) where Type : class, IDatabaseItem<Key>
        {
            using (var scope = _fixture.Host.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<FlashMEMOContext>();
                if (dbContext.Find<Type>(entity.DbId) == null) dbContext.Add(entity);
                dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Directly adds an object to the DB, bypassing the Repository class and/or any other interfaces (services, controllers, etc).
        /// </summary>
        /// <param name="entity">Object to be added into the DB.</param>
        /// <returns>The Id of the newly created object, which is teh current return value of the associated functions in the Repository classes.</returns>
        private TKey AddToContext(T entity)
        {
            using (var scope = _fixture.Host.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<FlashMEMOContext>();
                var id = dbContext.Add(entity).Entity.DbId;
                dbContext.SaveChanges();
                return id;
            }
        }

        private T GetFromContext(TKey id)
        {
            using (var scope = _fixture.Host.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<FlashMEMOContext>();
                return dbContext.Find<T>(id);
            }
        }

        /// <summary>
        /// Directly removes an object from the DB, bypassing the Repository class and/or any other interfaces (services, controllers, etc).
        /// </summary>
        /// <param name="entity">Object to be removed from the DB.</param>
        private void RemoveFromContext(T entity)
        {
            using (var scope = _fixture.Host.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<FlashMEMOContext>();
                dbContext.Remove(entity);
                dbContext.SaveChanges();
            }
        }

        private void RemoveFromContext(List<T> entities)
        {
            using (var scope = _fixture.Host.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<FlashMEMOContext>();
                dbContext.RemoveRange(entities);
                dbContext.SaveChanges();
            }
        }

        public GenericControllerTests(IntegrationTestFixture fixture, ITestOutputHelper output)
        {
            _fixture = fixture;
            _client = fixture.HttpClient;
            _output = output;

            _createEndpoint = $"{_baseEndpoint}/create";
            _deleteEndpoint = $"{_baseEndpoint}/delete";
            _listEndpoint = $"{_baseEndpoint}/list";
            _searchEndpoint = $"{_baseEndpoint}/search";

            //using (var scope = _fixture.Host.Services.CreateScope())
            //{
            //    var dbContext = scope.ServiceProvider.GetService<FlashMEMOContext>();
            //    dbContext.Database.EnsureCreated(); // must do this to actually seed the data
            //}
        }

        public virtual async Task CreateEntity(TDTO dto)
        {
            // Arrange
            var entity = new T();
            dto.PassValuesToEntity(entity); // in this case, this 'entity' object is used only for comparison later

            // Act
            var response = await _client.PostAsync($"{_createEndpoint}", JsonContent.Create(dto));
            var parsedResponse = await response.Content.ReadFromJsonAsync<DataResponseModel<TKey>>();
            var entityFromContext = GetFromContext(parsedResponse.Data);
            entity.DbId = entityFromContext.DbId; // I assign the DB Id to the original object so I don't get errors such as 'NewsId do not match between objects)'.

            // Assert
            parsedResponse.Status.Should().Be("Success");
            entityFromContext.Should().BeEquivalentTo(entity);

            // Undo
            RemoveFromContext(entityFromContext);
        }

        public virtual async Task GetEntity(TDTO dto)
        {
            // Arrange
            var entity = new T();
            dto.PassValuesToEntity(entity);
            var id = AddToContext(entity);

            // Act
            var response = await _client.GetAsync($"{_baseEndpoint}/{id}");
            var parsedResponse = await response.Content.ReadFromJsonAsync<DataResponseModel<T>>();

            // Assert
            parsedResponse.Status.Should().Be("Success");
            parsedResponse.Data.Should().BeEquivalentTo(entity);

            // Undo
            RemoveFromContext(entity);
        }

        public virtual async Task DeleteEntity(TDTO dto)
        {
            // Arrange
            var entity = new T();
            dto.PassValuesToEntity(entity);
            var id = AddToContext(entity);

            // Act
            var response = await _client.PostAsync($"{_deleteEndpoint}", JsonContent.Create(id));
            var parsedResponse = await response.Content.ReadFromJsonAsync<BaseResponseModel>();
            var entityFromContext = GetFromContext(id);

            // Assert
            parsedResponse.Status.Should().Be("Success");
            entityFromContext.Should().BeNull();
        }

        public virtual async Task UpdateEntity(TDTO dto, TDTO updatedDTO)
        {
            // Arrange
            var entity = new T();
            dto.PassValuesToEntity(entity);
            var id = AddToContext(entity);

            // Act
            var response = await _client.PutAsync($"{_baseEndpoint}/{id}", JsonContent.Create(updatedDTO));
            var parsedResponse = await response.Content.ReadFromJsonAsync<BaseResponseModel>();
            var entityFromContext = GetFromContext(id);

            // Assert
            parsedResponse.Status.Should().Be("Success");
            entityFromContext.Should().BeEquivalentTo(updatedDTO);
            entityFromContext.Should().NotBeEquivalentTo(entity);

            // Undo
            RemoveFromContext(entityFromContext);
        }

        public virtual async Task ListEntity(List<TDTO> dtoList, int pageSize)
        {
            // Arrange
            var entityList = new List<T>();
            for (int i = 0; i < dtoList.Count; i++)
            {
                entityList.Add(new T());
                dtoList[i].PassValuesToEntity(entityList[i]);
                entityList[i].DbId = AddToContext(entityList[i]);
            }
            var retrievedEntities = new List<T>();

            // Act
            var maxIndex = Math.Ceiling((decimal)dtoList.Count / (decimal)pageSize);
            for (int i = 1; i <= maxIndex; i++)
            {
                var response = await _client.GetAsync($"{_listEndpoint}?pageSize={pageSize}&pageNumber={i}");
                var parsedResponse = await response.Content.ReadFromJsonAsync<PaginatedListResponse<T>>();

                // Assert
                parsedResponse.Status.Should().Be("Success");
                parsedResponse.Errors?.Count().Should().Be(0);
                parsedResponse.Data.ResultSize.Should().BeLessThanOrEqualTo(pageSize);
                parsedResponse.Data.TotalAmount.Should().Be((ulong)dtoList.Count);
                parsedResponse.Data.TotalPages.Should().Be((ulong)maxIndex);
                parsedResponse.Data.PageNumber.Should().Be((ulong)i);
                parsedResponse.Data.Results.Count.Should().BeLessThanOrEqualTo(pageSize);
                parsedResponse.Data.HasPreviousPage.Should().Be(i == 1 ? false : true);
                parsedResponse.Data.HasNextPage.Should().Be(i == maxIndex ? false : true);

                parsedResponse.Data.Results.Should().NotIntersectWith(retrievedEntities);
                parsedResponse.Data.Results.Should().IntersectWith(entityList); // EXTREMELY IMPORTANT: THIS METHOD ONLY WORKS IF 'Equal' and (probably) 'GetHashCode' ARE OVERLOADED IN THE ENTITY. OTHERWISE IT FUCKS UP THE COMPARISONS. I discovered that once I start noticing that even two identical collections didn't 'intersect'... amazing how much headache testing in .NET can give

                retrievedEntities.AddRange(parsedResponse.Data.Results);
            }

            // Undo
            RemoveFromContext(entityList);
        }

        public virtual async Task SearchEntity(List<TDTO> dtoList, string queryParams, int pageSize, ValidateFilteringTestData<T> expectedFiltering)
        {
            // Arrange
            var entityList = new List<T>();
            for (int i = 0; i < dtoList.Count; i++)
            {
                entityList.Add(new T());
                dtoList[i].PassValuesToEntity(entityList[i]);
                entityList[i].DbId = AddToContext(entityList[i]);
            }
            var processedEntities = entityList.Where(x => expectedFiltering.predicate.Compile()(x)).ToList();
            if (expectedFiltering.sortType == Data.Tools.Sorting.SortType.Ascending) processedEntities = entityList.OrderBy(expectedFiltering.sortPredicate.Compile()).ToList();
            if (expectedFiltering.sortType == Data.Tools.Sorting.SortType.Descending) processedEntities = entityList.OrderByDescending(expectedFiltering.sortPredicate.Compile()).ToList();

            var retrievedEntities = new List<T>();

            // Act
            // Assert
            var maxIndex = Math.Max(Math.Ceiling((decimal)processedEntities.Count / (decimal)pageSize), 1);
            for (int i = 1; i <= maxIndex; i++)
            {
                var pageOptionsQueryParams = $"{(String.IsNullOrEmpty(queryParams) ? "?" : "&")}pageSize={pageSize}&pageNumber={i}";
                var response = await _client.GetAsync($"{_searchEndpoint}{queryParams}{pageOptionsQueryParams}");
                var parsedResponse = await response.Content.ReadFromJsonAsync<PaginatedListResponse<T>>();

                parsedResponse.Status.Should().Be("Success");
                parsedResponse.Errors?.Count().Should().Be(0);
                parsedResponse.Data.ResultSize.Should().BeLessThanOrEqualTo(pageSize);
                parsedResponse.Data.TotalAmount.Should().BeLessThanOrEqualTo((ulong)dtoList.Count);
                parsedResponse.Data.TotalPages.Should().Be((ulong)maxIndex);
                parsedResponse.Data.PageNumber.Should().Be((ulong)i);
                parsedResponse.Data.Results.Count.Should().BeLessThanOrEqualTo(pageSize);
                parsedResponse.Data.HasPreviousPage.Should().Be(i == 1 ? false : true);
                parsedResponse.Data.HasNextPage.Should().Be(i == maxIndex ? false : true);

                parsedResponse.Data.Results.Should().NotIntersectWith(retrievedEntities);
                if (processedEntities.Count > 0) parsedResponse.Data.Results.Should().IntersectWith(processedEntities);


                retrievedEntities.AddRange(parsedResponse.Data.Results);
            }

            _output.WriteLine($"Retrieved entities throughout the test were: {JsonConvert.SerializeObject(retrievedEntities, _serializerSettings)}");

            // Undo
            RemoveFromContext(entityList);
        }

        public virtual async Task TestCreateAndUpdateValidations(TDTO dto, List<string> expectedValidations)
        {
            // Arrange
            var dummyEntity = new T();
            var id = AddToContext(dummyEntity);

            // Act
            var createResponse = await _client.PostAsync($"{_createEndpoint}", JsonContent.Create(dto));
            var createParsedResponse = await createResponse.Content.ReadFromJsonAsync<BaseResponseModel>();
            var updateResponse = await _client.PutAsync($"{_baseEndpoint}/{id}", JsonContent.Create(dto));
            var updateParsedResponse = await updateResponse.Content.ReadFromJsonAsync<BaseResponseModel>();

            // Assert
            createParsedResponse.Status.Should().Be("Bad Request");
            createParsedResponse.Errors.Should().NotBeNullOrEmpty();
            createParsedResponse.Errors.Should().HaveCount(expectedValidations.Count);
            createParsedResponse.Errors.Should().Contain(expectedValidations);

            updateParsedResponse.Status.Should().Be("Bad Request");
            updateParsedResponse.Errors.Should().NotBeNullOrEmpty();
            updateParsedResponse.Errors.Should().HaveCount(expectedValidations.Count);
            updateParsedResponse.Errors.Should().Contain(expectedValidations);

            // Undo
            RemoveFromContext(dummyEntity);
        }

        /// <summary>
        /// DISCLAIMER: this method shouldn't have to be overwritten in the test classes themselves, since their full behavior could be tested here directly. However, if I declare this method with all of its test attributes ('Theory', etc), the test runner will attempt to instantiate this abstract class, and this of course will piss off C#.
        /// </summary>
        /// <param name="invalidId"></param>
        /// <param name="expectedValidations"></param>
        /// <returns></returns>
        public virtual async Task TestGetAndRemoveInvalidIdValidations(TKey invalidId, List<string> expectedValidations)
        {
            // Arrange

            // Act
            var getResponse = await _client.GetAsync($"{_baseEndpoint}/{invalidId}");
            var getParsedResponse = await getResponse.Content.ReadFromJsonAsync<BaseResponseModel>();
            var deleteResponse = await _client.PostAsync($"{_deleteEndpoint}", JsonContent.Create(invalidId));
            var deleteParsedResponse = await deleteResponse.Content.ReadFromJsonAsync<BaseResponseModel>();

            // Assert
            getParsedResponse.Status.Should().Be("Bad Request");
            getParsedResponse.Errors.Should().NotBeNullOrEmpty();
            getParsedResponse.Errors.Should().HaveCount(expectedValidations.Count);
            getParsedResponse.Errors.Should().Contain(expectedValidations);

            deleteParsedResponse.Status.Should().Be("Bad Request");
            deleteParsedResponse.Errors.Should().NotBeNullOrEmpty();
            deleteParsedResponse.Errors.Should().HaveCount(expectedValidations.Count);
            deleteParsedResponse.Errors.Should().Contain(expectedValidations);

            // Undo
        }
    }

    public class NewsControllerTests : GenericControllerTests<News, Guid, NewsDTO>
    {
        public NewsControllerTests(IntegrationTestFixture fixture, ITestOutputHelper output) : base(fixture, output) { }

        /// <summary>
        /// Data to be used in Create, Read, and Delete tests.
        /// </summary>
        public static IEnumerable<object[]> CRDData
        {
            get
            {
                yield return new object[] { new NewsDTO { Title = "Title", Subtitle = "Subtitle", Content = "Content" } };
                yield return new object[] { new NewsDTO { Title = "Title 2", Subtitle = "Subtitle 2", Content = "Content 2", } };
                yield return new object[] { new NewsDTO { Title = "Title 3", Content = "Content 3" } };
                yield return new object[] { new NewsDTO { Subtitle = "Subtitle 4", CreationDate = DateTime.UtcNow, LastUpdated = DateTime.UtcNow } };
            }
        }

        [Theory, MemberData(nameof(CRDData))]
        public async override Task CreateEntity(NewsDTO dto)
        {
            await base.CreateEntity(dto);
        }

        [Theory, MemberData(nameof(CRDData))]
        public async override Task GetEntity(NewsDTO dto)
        {
            await base.GetEntity(dto);
        }

        [Theory, MemberData(nameof(CRDData))]
        public async override Task DeleteEntity(NewsDTO dto)
        {
            await base.DeleteEntity(dto);
        }

        public static IEnumerable<object[]> UpdateEntityData
        {
            get
            {
                yield return new object[] { new NewsDTO { Title = "Title", Subtitle = "Subtitle", Content = "Content" }, new NewsDTO { Title = "Updated Title", Subtitle = "Updated Subtitle", Content = "Updated Content" } };
                yield return new object[] { new NewsDTO { Title = "Title 2", Subtitle = "Subtitle 2", Content = "Content 2", }, new NewsDTO { ThumbnailPath = "../../DoesntExistFolder/image.img" } };
            }
        }

        [Theory, MemberData(nameof(UpdateEntityData))]
        public async override Task UpdateEntity(NewsDTO dto, NewsDTO updatedDTO)
        {
            await base.UpdateEntity(dto, updatedDTO);
        }

        static List<NewsDTO> dTOs = new List<NewsDTO>()
        {
            new NewsDTO { Title = "Title", Content = "Content", Subtitle = "Subtitle" },
            new NewsDTO { Title = "Spaced Title", Content = "Spaced Content", Subtitle = "Spaced Subtitle" },
            new NewsDTO { Title = "Title", Content = "Content", Subtitle = "Subtitle", CreationDate = DateTime.Parse("2000-01-01+00"), LastUpdated = DateTime.Parse("2000-01-01+00") },
            new NewsDTO { Title = "Title", Content = "Content", Subtitle = "Subtitle", CreationDate = DateTime.Parse("2000-01-01T23:59:59+00"), LastUpdated = DateTime.Parse("2000-01-01T23:59:59+00") },
            new NewsDTO { Title = "Title", Content = "Content", Subtitle = "Subtitle", CreationDate = DateTime.Parse("2000-01-02+00"), LastUpdated = DateTime.Parse("2000-01-02+00") },
            new NewsDTO { Title = "Title", Content = "Content", Subtitle = "Subtitle", CreationDate = DateTime.Parse("2000-01-03+00"), LastUpdated = DateTime.Parse("2000-01-03+00") },
            new NewsDTO { Title = "Title2", Content = "Content2", Subtitle = "Subtitle2" },
            new NewsDTO { Title = "Title3", Content = "Content3", Subtitle = "Subtitle3" },
            new NewsDTO { Title = "Title4", Content = "Content4", Subtitle = "Subtitle4" },
            new NewsDTO { Title = "Title5", Content = "Content5", Subtitle = "Subtitle5" },
            new NewsDTO { Title = "Title6", Content = "Content6", Subtitle = "Subtitle6" },
            new NewsDTO { Title = "Title7", Content = "Content7", Subtitle = "Subtitle7" },
            new NewsDTO { Title = "Title8", Content = "Content8", Subtitle = "Subtitle8" },
            new NewsDTO { Title = "Title9", Content = "Content9", Subtitle = "Subtitle9" },
            new NewsDTO { Title = "Title10", Content = "Content10", Subtitle = "Subtitle10" },
        };

        public static IEnumerable<object[]> ListEntityData
        {
            get
            {
                yield return new object[] { dTOs, 1 };
                yield return new object[] { dTOs, 100 };
                yield return new object[] { dTOs, 5 };
                yield return new object[] { dTOs, 7 };
                yield return new object[] { dTOs, 10 };
            }
        }

        [Theory, MemberData(nameof(ListEntityData))]
        public async override Task ListEntity(List<NewsDTO> dtoList, int pageSize)
        {
            await base.ListEntity(dtoList, pageSize);
        }

        public static IEnumerable<object[]> SearchEntityData
        {
            get
            {
                yield return new object[] { dTOs, "?title=Title2&columnToSort=title&sortType=Descending", 100, new ValidateFilteringTestData<News>() {
                    predicate = n => n.Title.Contains("Title2"),
                    sortPredicate = n => n.Title,
                    sortType = Data.Tools.Sorting.SortType.Descending
                }
            };
                yield return new object[] { dTOs, "?title=Title&orderBy=title&sortType=Ascending&columnToSort=title", 10, new ValidateFilteringTestData<News>() {
                    predicate = n => n.Title.Contains("Title"),
                    sortPredicate = n => n.Title,
                    sortType = Data.Tools.Sorting.SortType.Ascending
                }
            };
                yield return new object[] { dTOs, "?FromDate=2000-01-01&ToDate=2000-01-01&Title=Title&Subtitle=Subtitle&Content=Content", 100, new ValidateFilteringTestData<News>() {
                    predicate = n => n.Title.Contains("Title") &&
                    n.Subtitle.Contains("Subtitle") &&
                    n.Content.Contains("Content") &&
                    n.CreationDate >= DateTime.Parse("2000-01-01T00:00:00+00").ToUniversalTime() &&
                    n.CreationDate <= DateTime.Parse("2000-01-01T23:59:59+00").ToUniversalTime()
                }
            };
                yield return new object[] { dTOs, "?Title=Spaced%20Title", 100, new ValidateFilteringTestData<News>() {
                    predicate = n => n.Title.Contains("Spaced Title")
                }
            };
            }
        }

        [Theory, MemberData(nameof(SearchEntityData))]
        public async override Task SearchEntity(List<NewsDTO> dtoList, string queryParams, int pageSize, ValidateFilteringTestData<News> expectedFiltering)
        {
            await base.SearchEntity(dtoList, queryParams, pageSize, expectedFiltering);
        }

        public static IEnumerable<object[]> TestEntityValidationsData
        {
            get
            {
                yield return new object[] { new NewsDTO { Title = "Title", Content = "Content", Subtitle = "Subtitle", CreationDate = DateTime.Parse("2000-01-02+00"), LastUpdated = DateTime.Parse("2000-01-01+00") }, new List<string>() { ServiceValidationMessages.CreationDateMoreRecentThanLastUpdated }
                };
            }
        }

        [Theory, MemberData(nameof(TestEntityValidationsData))]
        public async override Task TestCreateAndUpdateValidations(NewsDTO dtoList, List<string> expectedValidations)
        {
            await base.TestCreateAndUpdateValidations(dtoList, expectedValidations);
        }

        public static IEnumerable<object[]> TestGetAndRemoveInvalidIdValidationsData
        {
            get
            {
                yield return new object[]
                {
                    null, new List<string>() { API.Controllers.Messages.ErrorMessages.NoObjectAssociatedWithId }
                };
                yield return new object[]
                {
                    Guid.Empty, new List<string>() { API.Controllers.Messages.ErrorMessages.NoObjectAssociatedWithId },
                };
                yield return new object[]
                {
                    Guid.NewGuid(), new List<string>() { API.Controllers.Messages.ErrorMessages.NoObjectAssociatedWithId },
                };
            }
        }

        [Theory, MemberData(nameof(TestGetAndRemoveInvalidIdValidationsData))]
        public async override Task TestGetAndRemoveInvalidIdValidations(Guid invalidId, List<string> expectedValidations)
        {
            await base.TestGetAndRemoveInvalidIdValidations(invalidId, expectedValidations);
        }
    }

    public class DeckControllerTests : GenericControllerTests<Deck, Guid, DeckDTO>
    {
        private static readonly Language TestLanguage1 = new Language { Name = "English", ISOCode = "en" };
        private static readonly Language TestLanguage2 = new Language { Name = "French", ISOCode = "fr" };
        private static readonly Language TestLanguage3 = new Language { Name = "Italian", ISOCode = "it" };

        private static readonly ApplicationUser TestUser1 = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "admin", Email = "admin@flashmemo.edu" };
        private static readonly ApplicationUser TestUser2 = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "user", Email = "user@flashmemo.edu" };
        private static readonly ApplicationUser TestUser3 = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "manager", Email = "manager@flashmemo.edu" };

        public DeckControllerTests(IntegrationTestFixture fixture, ITestOutputHelper output) : base(fixture, output)
        {
            using (var scope = _fixture.Host.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<FlashMEMOContext>();

                // this disgusting implementation is required because (1) no 'AddOrUpdate' method exists in the EF Core stuff anymore (despite claims of it on the internet), and because (2) the 'Update' method doesn't actually add instead of updating when providing a non-existent object (it should, though) 
                foreach (var item in new List<Language>() { TestLanguage1, TestLanguage2, TestLanguage3 })
                {
                    AddIfNecessary<Language, string>(item);
                }
                foreach (var item in new List<ApplicationUser>() { TestUser1, TestUser2, TestUser3 })
                {
                    AddIfNecessary<ApplicationUser, string>(item);
                }

                dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Data to be used in Create, Read, and Delete tests.
        /// </summary>
        public static IEnumerable<object[]> CRDData
        {
            get
            {
                yield return new object[] { new DeckDTO { Name = "Deck", Description = "This is the description", LanguageISOCode = TestLanguage1.ISOCode, OwnerId = TestUser1.Id } };
                yield return new object[] { new DeckDTO { Name = "Deck", Description = "This is the description", LanguageISOCode = TestLanguage2.ISOCode, OwnerId = TestUser2.Id } };
            }
        }

        [Theory, MemberData(nameof(CRDData))]
        public async override Task CreateEntity(DeckDTO dto)
        {
            await base.CreateEntity(dto);
        }

        [Theory, MemberData(nameof(CRDData))]
        public async override Task GetEntity(DeckDTO dto)
        {
            await base.GetEntity(dto);
        }

        [Theory, MemberData(nameof(CRDData))]
        public async override Task DeleteEntity(DeckDTO dto)
        {
            await base.DeleteEntity(dto);
        }

        public static IEnumerable<object[]> UpdateEntityData
        {
            get
            {
                yield return new object[] { new DeckDTO { Name = "Deck", Description = "This is the description", LanguageISOCode = TestLanguage1.ISOCode, OwnerId = TestUser1.Id }, new DeckDTO { Name = "Deck", Description = "This is the updated description", LanguageISOCode = TestLanguage1.ISOCode, OwnerId = TestUser1.Id } };
                yield return new object[] { new DeckDTO { Name = "Deck", Description = "This is the description", LanguageISOCode = TestLanguage1.ISOCode, OwnerId = TestUser1.Id }, new DeckDTO { Name = "Deck", Description = "This is the updated description", LanguageISOCode = TestLanguage2.ISOCode, OwnerId = TestUser2.Id } };
            }
        }

        [Theory, MemberData(nameof(UpdateEntityData))]
        public async override Task UpdateEntity(DeckDTO dto, DeckDTO updatedDTO)
        {
            await base.UpdateEntity(dto, updatedDTO);
        }

        static List<DeckDTO> dTOs = new List<DeckDTO>() {
            new DeckDTO { Name = "Deck", Description = "This is the description", LanguageISOCode = TestLanguage1.ISOCode, OwnerId = TestUser1.Id  },
            new DeckDTO { Name = "Deck 2", Description = "This is the description 2", LanguageISOCode = TestLanguage2.ISOCode, OwnerId = TestUser2.Id  },
            new DeckDTO { Name = "Deck 3", Description = "This is the description 3", LanguageISOCode = TestLanguage3.ISOCode, OwnerId = TestUser3.Id  }
        };

        public static IEnumerable<object[]> ListEntityData
        {
            get
            {
                yield return new object[] { dTOs, 1 };
                yield return new object[] { dTOs, 100 };
                yield return new object[] { dTOs, 5 };
                yield return new object[] { dTOs, 7 };
                yield return new object[] { dTOs, 10 };
            }
        }

        [Theory, MemberData(nameof(ListEntityData))]
        public async override Task ListEntity(List<DeckDTO> dtoList, int pageSize)
        {
            await base.ListEntity(dtoList, pageSize);
        }

        public static IEnumerable<object[]> SearchEntityData
        {
            get
            {
                yield return new object[] {
                    dTOs,
                    "?name=Deck&columnToSort=name&sortType=Descending",
                    100,
                    new ValidateFilteringTestData<Deck>()
                    {
                        predicate = n => n.Name.Contains("Deck"),
                        sortPredicate = n => n.Name,
                        sortType = Data.Tools.Sorting.SortType.Descending
                    }
                };
            }
        }

        [Theory, MemberData(nameof(SearchEntityData))]
        public async override Task SearchEntity(List<DeckDTO> dtoList, string queryParams, int pageSize, ValidateFilteringTestData<Deck> expectedFiltering)
        {
            await base.SearchEntity(dtoList, queryParams, pageSize, expectedFiltering);
        }

        public static IEnumerable<object[]> TestCreateAndUpdateValidationsData
        {
            get
            {
                yield return new object[]
                {
                    new DeckDTO { Name = "Deck", Description = "This is the description", LanguageISOCode = TestLanguage1.ISOCode, OwnerId = TestUser1.Id, CreationDate = DateTime.Parse("2000-01-02"), LastUpdated = DateTime.Parse("2000-01-01")
                    },
                    new List<string>()
                    {
                        ServiceValidationMessages.CreationDateMoreRecentThanLastUpdated
                    }
                };
                yield return new object[]
                {
                    new DeckDTO { Name = "Deck", Description = "This is the description", LanguageISOCode = "invalid", OwnerId = TestUser1.Id,
                    },
                    new List<string>()
                    {
                        ServiceValidationMessages.InvalidLanguageCode
                    }
                };
                yield return new object[]
                {
                    new DeckDTO { Name = "Deck", Description = "This is the description", LanguageISOCode = TestLanguage1.ISOCode, OwnerId = Guid.Empty.ToString()
                    },
                    new List<string>()
                    {
                        ServiceValidationMessages.InvalidUserId
                    }
                };
            }
        }

        [Theory, MemberData(nameof(TestCreateAndUpdateValidationsData))]
        public async override Task TestCreateAndUpdateValidations(DeckDTO dto, List<string> expectedValidations)
        {
            await base.TestCreateAndUpdateValidations(dto, expectedValidations);
        }

        public static IEnumerable<object[]> TestGetAndRemoveInvalidIdValidationsData
        {
            get
            {
                yield return new object[]
                {
                    null, new List<string>() { API.Controllers.Messages.ErrorMessages.NoObjectAssociatedWithId }
                };
                yield return new object[]
                {
                    Guid.Empty, new List<string>() { API.Controllers.Messages.ErrorMessages.NoObjectAssociatedWithId },
                };
                yield return new object[]
                {
                    Guid.NewGuid(), new List<string>() { API.Controllers.Messages.ErrorMessages.NoObjectAssociatedWithId },
                };
            }
        }

        [Theory, MemberData(nameof(TestGetAndRemoveInvalidIdValidationsData))]
        public async override Task TestGetAndRemoveInvalidIdValidations(Guid invalidId, List<string> expectedValidations)
        {
            await base.TestGetAndRemoveInvalidIdValidations(invalidId, expectedValidations);
        }
    }
}


