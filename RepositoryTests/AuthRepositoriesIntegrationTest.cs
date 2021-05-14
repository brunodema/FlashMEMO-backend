﻿using Data;
using Data.Interfaces;
using Data.Models;
using Data.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RepositoryTests.Interfaces;
using System;
using Xunit;
using Xunit.Abstractions;

namespace RepositoryTests
{
    public class AuthRepositoriesIntegration
    {
        public static class RoleTestGUID
        {
            public const string GUID1 = "34e9a9fe-85c3-4e47-81eb-904ee907de55";
            public const string GUID2 = "ab0a60dd-606b-42a1-ac78-e5942cf2d425";
            public const string GUID3 = "cfee552f-5f44-4f2e-aa85-c1b2be7632cf";
            public const string GUID4 = "5904095d-dbc5-4363-8044-b444bdebfc79"; // is not used in initialization
            public const string GUID5 = "eec739b0-778d-4af4-b94b-6d242eba2e51"; // is not used in initialization
        }
        public static class UserTestGUID
        {
            public const string GUID1 = "680f82d1-094f-4f9f-b666-4fe910b408d4";
            public const string GUID2 = "0d99f136-87db-461c-943d-bcb76e8d75dd";
            public const string GUID3 = "5f2609e3-323d-4fd7-b497-1b328b161348";
            public const string GUID4 = "0d5c2fb5-a849-4b56-8b6f-23f44ac4be4c"; // is not used in initialization
            public const string GUID5 = "7925ec1c-4c9d-499b-919e-a0447e447e05"; // is not used in initialization
        }
        public class AuthRepositoriesIntegrationTestFixture : IDisposable
        {
            public ApplicationUserRepository _userRepository;
            public RoleRepository _roleRepository;
            public AuthRepositoriesIntegrationTestFixture()
            {
                var options = new DbContextOptionsBuilder<FlashMEMOContext>().UseInMemoryDatabase(databaseName: "FlashMEMOTest").Options;
                var context = new FlashMEMOContext(options);
                var roleManager = new RoleManager<ApplicationRole>(
                    new RoleStore<ApplicationRole>(context),
                    null,
                    null,
                    null,
                    null);

                roleManager.CreateAsync(new ApplicationRole
                {
                    Id = RoleTestGUID.GUID1,
                    Name = "admin"
                }).Wait();
                roleManager.CreateAsync(new ApplicationRole
                {
                    Id = RoleTestGUID.GUID2,
                    Name = "user"
                }).Wait();
                roleManager.CreateAsync(new ApplicationRole
                {
                    Id = RoleTestGUID.GUID3,
                    Name = "visitor"
                }).Wait();

                _roleRepository = new RoleRepository(context, roleManager);

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
                    Id = UserTestGUID.GUID1,
                    Email = "test@email.com",
                    NormalizedEmail = "TEST@EMAIL.COM",
                    UserName = "Test",
                    NormalizedUserName = "TEST"

                }, "Test@123").Wait();
                userManager.CreateAsync(new ApplicationUser
                {
                    Id = UserTestGUID.GUID2,
                    Email = "test2@email.com",
                    NormalizedEmail = "TEST2@EMAIL.COM",
                    UserName = "Test2",
                    NormalizedUserName = "TEST2"

                }, "Test@123").Wait();
                userManager.CreateAsync(new ApplicationUser
                {
                    Id = UserTestGUID.GUID3,
                    Email = "test3@email.com",
                    NormalizedEmail = "TEST3@EMAIL.COM",
                    UserName = "Test3",
                    NormalizedUserName = "TEST3"

                }, "Test@123").Wait();

                _userRepository = new ApplicationUserRepository(context, userManager);
            }

            public void Dispose()
            {
                _roleRepository?.Dispose();
                _userRepository?.Dispose();
            }
        }

        public class AuthRepositoriesIntegrationTest : IClassFixture<AuthRepositoriesIntegrationTestFixture>, IAuthRepositoriesIntegrationTest
        {
            private AuthRepositoriesIntegrationTestFixture _authRepositoriesFixture;
            private readonly ITestOutputHelper _output;

            public AuthRepositoriesIntegrationTest(AuthRepositoriesIntegrationTestFixture authRepositoriesFixture, ITestOutputHelper output)
            {
                _authRepositoriesFixture = authRepositoriesFixture;
                _output = output;
            }
            [Fact]
            public async void AddUserToRole_CheckThatItGetsCorrectlyAdded()
            {
                // Arrange
                var user = await _authRepositoriesFixture._userRepository.GetByIdAsync(Guid.Parse(UserTestGUID.GUID1));

                // Act
                // The repositories do not have shared methods for adding/removing roles - oops! Need to implement it.
                // Assert
            }

            public void RemoveUserFromRole_CheckThatItGetsCorrectlyRemoved()
            {
                throw new NotImplementedException();
            }
        }
    }

}