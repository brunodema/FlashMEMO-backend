﻿using Data;
using Data.Interfaces;
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
    public class AuthRepositoryFixture : IDisposable
    {
        public AuthRepository _repository;
        public AuthRepositoryFixture()
        {
            var options = new DbContextOptionsBuilder<FlashMEMOContext>().UseInMemoryDatabase(databaseName: "FlashMEMOTest").Options;
            var context = new FlashMEMOContext(options);
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context),
                null,
                new PasswordHasher<ApplicationUser>(),
                null,
                null,
                null,
                null,
                null,
                null);

            userManager.CreateAsync(new ApplicationUser
            {
                Id = "858b3287-5972-4069-bf75-a650453dfef7",
                Email = "test@email.com",
                NormalizedEmail = "TEST@EMAIL.COM",
                UserName = "Test",
                NormalizedUserName = "TEST"

            }, "Test@123").Wait();
            userManager.CreateAsync(new ApplicationUser
            {
                Id = "8dcbd335-0f2e-46ae-bebb-d9cdad0e7487",
                Email = "test2@email.com",
                NormalizedEmail = "TEST2@EMAIL.COM",
                UserName = "Test2",
                NormalizedUserName = "TEST2"

            }, "Test@123").Wait();
            userManager.CreateAsync(new ApplicationUser
            {
                Id = "e7edd329-b0bd-4820-9df4-c13c8aab3577",
                Email = "test3@email.com",
                NormalizedEmail = "TEST3@EMAIL.COM",
                UserName = "Test3",
                NormalizedUserName = "TEST3"

            }, "Test@123").Wait();

            _repository = new AuthRepository(userManager);
        }

        public void Dispose()
        {
            _repository?.Dispose();
        }
    }

    public class AuthRepositoryTests : IClassFixture<AuthRepositoryFixture>, IAuthRepositoryTest
    {
        private AuthRepositoryFixture _repositoryFixture;
        private readonly ITestOutputHelper _output;

        public AuthRepositoryTests(AuthRepositoryFixture repositoryFixture, ITestOutputHelper output)
        {
            _repositoryFixture = repositoryFixture;
            _output = output;
        }

        [Fact]
        public async void CreateUserAsync_AssertThatItGetsProperlyCreated()
        {
            // Arrange
            var numRows = this._repositoryFixture._repository.GetAllUserAsync().Result.Count;
            var dummyUser = new ApplicationUser
            {
                Id = "424e9d3e-686d-4163-be89-e7bae263a1a5",
                Email = "dummy@domain.com",
                NormalizedEmail = "DUMMY@DOMAIN.COM",
                UserName = "Dummy",
                NormalizedUserName = "DUMMY"
            };

            // Act
            await this._repositoryFixture._repository.CreateUserAsync(dummyUser, "Dummy@123");

            // Assert
            var newNumRows = this._repositoryFixture._repository.GetAllUserAsync().Result.Count;
            Assert.True((await this._repositoryFixture._repository.GetAllUserAsync()).Contains(dummyUser), "Table does not contain the new item");
            Assert.True(newNumRows == numRows + 1, $"Number of rows did not increase with the new item added ({newNumRows} != {numRows + 1})");
        }
        [Fact]
        public async void UpdateUserAsync_AssertThatItGetsProperlyUpdated()
        {
            // Arrange
            var numRows = this._repositoryFixture._repository.GetAllUserAsync().Result.Count;
            var dummyUser = await this._repositoryFixture._repository.GetUserByIdAsync(Guid.Parse("e7edd329-b0bd-4820-9df4-c13c8aab3577"));

            dummyUser.Email = "newemail@email.com";
            dummyUser.UserName = "newdummy";

            // Act
            await this._repositoryFixture._repository.UpdateUserAsync(dummyUser);

            // Assert
            var newNumRows = this._repositoryFixture._repository.GetAllUserAsync().Result.Count;
            var queryResult = await this._repositoryFixture._repository.GetUserByIdAsync(Guid.Parse("e7edd329-b0bd-4820-9df4-c13c8aab3577"));
            Assert.NotNull(queryResult);
            Assert.True(queryResult.Email == "newemail@email.com", "Object property does not match the new updated value");
            Assert.True(queryResult.UserName == "newdummy", "Object property does not match the new updated value");
            Assert.True(newNumRows == numRows, $"Number of rows did not stay the same with the update ({newNumRows} != {numRows})");
        }
        [Fact]
        public async void RemoveUserAsync_AssertThatItGetsProperlyRemoved()
        {
            // Arrange
            var numRows = this._repositoryFixture._repository.GetAllUserAsync().Result.Count;
            var dummyUser = await this._repositoryFixture._repository.GetUserByIdAsync(Guid.Parse("858b3287-5972-4069-bf75-a650453dfef7"));

            // Act
            await this._repositoryFixture._repository.RemoveUserAsync(dummyUser);

            // Assert
            var newNumRows = this._repositoryFixture._repository.GetAllUserAsync().Result.Count;
            Assert.False((await this._repositoryFixture._repository.GetAllUserAsync()).Contains(dummyUser), "Table still contains the item");
            Assert.True(newNumRows == numRows - 1, $"Number of rows did not decrease with the item removed ({ newNumRows} != { numRows - 1})");
        }

        public async void GetUserByIdAsync_AssertThatItGetsProperlyRemoved()
        {
            // Arrange
            // Act
            // Assert
        }
    }
}