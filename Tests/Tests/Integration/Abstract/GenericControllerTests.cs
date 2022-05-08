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

namespace Tests.Tests.Integration.Abstract
{
    public class GenericControllerTests<T, TKey, TDTO> : IClassFixture<IntegrationTestFixture>
        where T : class, IDatabaseItem<TKey>, new()
        where TDTO : IModelDTO<T, TKey>
    {
        protected IntegrationTestFixture _fixture;
        protected HttpClient _client;
        protected ITestOutputHelper _output;

        protected string _baseEndpoint = $"/api/v1/{typeof(T).Name}";
        protected string _createEndpoint;
        protected string _deleteEndpoint;
        protected string _listEndpoint;

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
            for (int i = 1; i <= Math.Ceiling((decimal)dtoList.Count / (decimal)pageSize); i++)
            {
                var response = await _client.GetAsync($"{_listEndpoint}?pageSize={pageSize}&pageNumber={i}");
                var parsedResponse = await response.Content.ReadFromJsonAsync<PaginatedListResponse<T>>();

                // Assert
                parsedResponse.Status.Should().Be("Success");
                parsedResponse.Data.Results.Count.Should().BeLessThanOrEqualTo(pageSize);
                parsedResponse.Data.Results.Should().NotIntersectWith(retrievedEntities);
                parsedResponse.Data.Results.Should().IntersectWith(entityList); // EXTREMELY IMPORTANT: THIS METHOD ONLY WORKS IF 'Equal' and (probably) 'GetHashCode' ARE OVERLOADED IN THE ENTITY. OTHERWISE IT FUCKS UP THE COMPARISONS. I discovered that once I start noticing that even two identical collections didn't 'intersect'... amazing how much headache testing in .NET can give

                retrievedEntities.AddRange(parsedResponse.Data.Results);
            }

            // Undo
            RemoveFromContext(retrievedEntities);
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
                yield return new object[] { new NewsDTO { Title = "Title 2", Subtitle = "Subtitle 2", Content = "Content 2",  } };
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
                yield return new object[] { dTOs, 100};
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
    }
}
