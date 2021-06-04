using Data.Context;
using Data.Repository.Interfaces;
using Data.Models;
using Data.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;
using Xunit.Abstractions;

namespace RepositoryTests
{
    public static class TestGUID
    {
        public const string GUID1 = "34e9a9fe-85c3-4e47-81eb-904ee907de55";
        public const string GUID2 = "ab0a60dd-606b-42a1-ac78-e5942cf2d425";
        public const string GUID3 = "cfee552f-5f44-4f2e-aa85-c1b2be7632cf";
        public const string GUID4 = "5904095d-dbc5-4363-8044-b444bdebfc79"; // is not used in initialization
        public const string GUID5 = "eec739b0-778d-4af4-b94b-6d242eba2e51"; // is not used in initialization
    }
    public class RoleRepositoryFixture : IDisposable
    {
        public RoleRepository _repository;
        public RoleRepositoryFixture()
        {
            var options = new DbContextOptionsBuilder<FlashMEMOContext>().UseInMemoryDatabase(databaseName: "RoleRepositoryFixture").Options;
            var context = new FlashMEMOContext(options);
            var roleManager = new RoleManager<ApplicationRole>(
                new RoleStore<ApplicationRole>(context),
                null,
                null,
                null,
                null);

            roleManager.CreateAsync(new ApplicationRole
            {
                Id = TestGUID.GUID1,
                Name = "admin"
            }).Wait();
            roleManager.CreateAsync(new ApplicationRole
            {
                Id = TestGUID.GUID2,
                Name = "admin"
            }).Wait();
            roleManager.CreateAsync(new ApplicationRole
            {
                Id = TestGUID.GUID3,
                Name = "admin"
            }).Wait();

            _repository = new RoleRepository(context, roleManager);
        }

        public void Dispose()
        {
            _repository?.Dispose();
        }
    }

    public class RoleRepositoryTests : IClassFixture<RoleRepositoryFixture>, IBaseRepositoryTests<ApplicationRole>
    {
        private RoleRepositoryFixture _repositoryFixture;
        private readonly ITestOutputHelper _output;

        public RoleRepositoryTests(RoleRepositoryFixture repositoryFixture, ITestOutputHelper output)
        {
            _repositoryFixture = repositoryFixture;
            _output = output;
        }

        [Fact]
        public async void CreateAsync_AssertThatItGetsProperlyCreated()
        {
            // Arrange
            var numRows = this._repositoryFixture._repository.GetAllAsync().Result.Count;
            var dummyRole = new ApplicationRole
            {
                Id = TestGUID.GUID4,
                Name = "new_admin"
            };

            // Act
            await this._repositoryFixture._repository.CreateAsync(dummyRole);

            // Assert
            var newNumRows = this._repositoryFixture._repository.GetAllAsync().Result.Count;
            Assert.True((await this._repositoryFixture._repository.GetAllAsync()).Contains(dummyRole), "Table does not contain the new item");
            Assert.True(newNumRows == numRows + 1, $"Number of rows did not increase with the new item added ({newNumRows} != {numRows + 1})");
        }
        [Fact]
        public async void UpdateAsync_AssertThatItGetsProperlyUpdated()
        {
            // Arrange
            var numRows = this._repositoryFixture._repository.GetAllAsync().Result.Count;
            var dummyRole = await this._repositoryFixture._repository.GetByIdAsync(Guid.Parse(TestGUID.GUID1));

            dummyRole.Name = "altered_name";

            // Act
            await this._repositoryFixture._repository.UpdateAsync(dummyRole);

            // Assert
            var newNumRows = this._repositoryFixture._repository.GetAllAsync().Result.Count;
            var queryResult = await this._repositoryFixture._repository.GetByIdAsync(Guid.Parse(TestGUID.GUID1));
            Assert.NotNull(queryResult);
            Assert.True(queryResult.Name == "altered_name", "Object property does not match the new updated value");
            Assert.True(newNumRows == numRows, $"Number of rows did not stay the same with the update ({newNumRows} != {numRows})");
        }
        [Fact]
        public async void RemoveAsync_AssertThatItGetsProperlyRemoved()
        {
            // Arrange
            var numRows = this._repositoryFixture._repository.GetAllAsync().Result.Count;
            var dummyRole = await this._repositoryFixture._repository.GetByIdAsync(Guid.Parse(TestGUID.GUID2));

            // Act
            await this._repositoryFixture._repository.RemoveAsync(dummyRole);

            // Assert
            var newNumRows = this._repositoryFixture._repository.GetAllAsync().Result.Count;
            Assert.False((await this._repositoryFixture._repository.GetAllAsync()).Contains(dummyRole), "Table still contains the item");
            Assert.True(newNumRows == numRows - 1, $"Number of rows did not decrease with the item removed ({ newNumRows} != { numRows - 1})");
        }
        [Fact]
        public async void GetByIdAsync_AssertThatItGetsProperlyRemoved()
        {
            // Arrange
            // Act
            var dummyRole1 = await this._repositoryFixture._repository.GetByIdAsync(Guid.Parse(TestGUID.GUID1));
            var dummyRole2 = await this._repositoryFixture._repository.GetByIdAsync(Guid.Parse(TestGUID.GUID5)); // invalid GUID

            // Assert
            Assert.NotNull(dummyRole1);
            Assert.Null(dummyRole2);
        }
    }
}