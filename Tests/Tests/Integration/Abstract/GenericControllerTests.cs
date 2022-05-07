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

namespace Tests.Tests.Integration.Abstract
{
    public class GenericControllerTests<T, TKey> : IClassFixture<IntegrationTestFixture>
        where T : class, IDatabaseItem<TKey>
    {
        protected IntegrationTestFixture _fixture;
        protected HttpClient _client;
        protected FlashMEMOContext _context;
        protected ITestOutputHelper _output;

        protected string _baseEndpoint = $"/api/v1/{typeof(T).Name}";

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
        }

        public virtual async Task CreateEntity(T entity)
        {
            // Arrange
            var id = AddToContext(entity);

            // Act
            var response = await _client.GetAsync($"{_baseEndpoint}/{id}");
            var parsedResponse = await response.Content.ReadFromJsonAsync<DataResponseModel<T>>();
            var lol = await response.Content.ReadAsStringAsync();
            var dflfdl = await (await _client.GetAsync($"{_baseEndpoint}/list?pageSize=100")).Content.ReadAsStringAsync();

            // Assert
            parsedResponse.Status.Should().Be("Success");
            parsedResponse.Data.Should().BeEquivalentTo(entity);

            // Undo
            RemoveFromContext(entity);
        }
    }

    public class NewsControllerTests : GenericControllerTests<News, Guid>
    {
        public NewsControllerTests(IntegrationTestFixture fixture, ITestOutputHelper output) : base(fixture, output) { }

        public static IEnumerable<object[]> CreateEntityData
        {
            get
            {
                yield return new object[] { new News { Title = "Title", Subtitle = "Subtitle", Content = "Content" } };
                yield return new object[] { new News { Title = "Title 2", Subtitle = "Subtitle 2", Content = "Content 2",  } };
            }
        }

        [Theory, MemberData(nameof(CreateEntityData))]
        public async override Task CreateEntity(News entity)
        {
            await base.CreateEntity(entity);
        }
    }
}
