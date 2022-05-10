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
    public abstract class GenericControllerTests<T, TKey, TDTO> : IClassFixture<IntegrationTestFixture>
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
        public async Task TestGetAndRemoveInvalidIdValidations(TKey invalidId, List<string> expectedValidations)
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
}


