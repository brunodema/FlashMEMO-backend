using Data.Context;
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
        protected FlashMEMOContext _context;
        protected ITestOutputHelper _output;

        protected string _baseEndpoint = $"/api/v1/{typeof(T).Name}";
        protected string _createEndpoint;

        /// <summary>
        /// Directly adds an object to the DB, bypassing the Repository class and/or any other interfaces (services, controllers, etc).
        /// </summary>
        /// <param name="entity">Object to be added into the DB.</param>
        /// <returns>The Id of the newly created object, which is teh current return value of the associated functions in the Repository classes.</returns>
        private TKey AddToContext(T entity)
        {
            var id = _context.Add(entity).Entity.DbId;
            _context.SaveChanges();
            return id;
        }

        /// <summary>
        /// Directly removes an object from the DB, bypassing the Repository class and/or any other interfaces (services, controllers, etc).
        /// </summary>
        /// <param name="entity">Object to be removed from the DB.</param>
        private void RemoveFromContext(T entity)
        {
            _context.Remove(entity);
            _context.SaveChanges();
        }

        public GenericControllerTests(IntegrationTestFixture fixture, ITestOutputHelper output)
        {
            _fixture = fixture;
            _client = fixture.HttpClient;
            _context = fixture.Host.Services.GetService<FlashMEMOContext>(); // spaghetti taken from here (which apparently is the correct approach): https://stackoverflow.com/questions/32459670/resolving-instances-with-asp-net-core-di-from-within-configureservices.
            _context.Database.EnsureCreated(); // required to actullay seed the data into the virtual DB
            _output = output;

            _createEndpoint = $"{_baseEndpoint}/create";
        }

        public virtual async Task CreateEntity(TDTO dto)
        {
            // Arrange
            var entity = new T();
            dto.PassValuesToEntity(entity); // in this case, this 'entity' object is used only for comparison later

            // Act
            var response = await _client.PostAsync($"{_createEndpoint}", JsonContent.Create(dto));
            var parsedResponse = await response.Content.ReadFromJsonAsync<DataResponseModel<TKey>>();
            var entityFromContext = _context.Find<T>(parsedResponse.Data);
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
    }

    public class NewsControllerTests : GenericControllerTests<News, Guid, NewsDTO>
    {
        public NewsControllerTests(IntegrationTestFixture fixture, ITestOutputHelper output) : base(fixture, output) { }

        public static IEnumerable<object[]> CreateEntityData
        {
            get
            {
                yield return new object[] { new NewsDTO { Title = "Title", Subtitle = "Subtitle", Content = "Content" } };
                yield return new object[] { new NewsDTO { Title = "Title 2", Subtitle = "Subtitle 2", Content = "Content 2",  } };
            }
        }

        [Theory, MemberData(nameof(CreateEntityData))]
        public async override Task CreateEntity(NewsDTO dto)
        {
            await base.CreateEntity(dto);
        }

        public static IEnumerable<object[]> GetEntityData
        {
            get
            {
                yield return new object[] { new NewsDTO { Title = "Title", Subtitle = "Subtitle", Content = "Content" } };
                yield return new object[] { new NewsDTO { Title = "Title 2", Subtitle = "Subtitle 2", Content = "Content 2", } };
            }
        }

        [Theory, MemberData(nameof(CreateEntityData))]
        public async override Task GetEntity(NewsDTO dto)
        {
            await base.GetEntity(dto);
        }
    }
}
