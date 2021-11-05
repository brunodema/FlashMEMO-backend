using Data.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RepositoryTests.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;
using Xunit.Abstractions;
using Data.Tools;
using FluentAssertions;
using Data.Tools.Implementation;
using Data.Models.Implementation;
using Data.Repository.Implementation;

namespace RepositoryTests.Implementation
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
        public class AuthRepositoryFixture : IDisposable
        {
            public ApplicationUserRepository _applicationUserRepository;
            public RoleRepository _roleRepository;
            public AuthRepositoryFixture()
            {
                var options = new DbContextOptionsBuilder<FlashMEMOContext>().UseInMemoryDatabase(databaseName: "AuthRepositoryFixture").Options;
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
                    Name = "admin",
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

                var roleRepository = new RoleRepository(context, roleManager);
                _roleRepository = roleRepository;

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

                var userRepository = new ApplicationUserRepository(context, userManager);

                _applicationUserRepository = userRepository;
            }

            public void Dispose()
            {
                _applicationUserRepository?.Dispose();
                _roleRepository?.Dispose();
            }
        }

        public class AuthRepositoriesIntegrationTest : IClassFixture<AuthRepositoryFixture>, IAuthRepositoriesIntegrationTest
        {
            private readonly AuthRepositoryFixture _authRepositoryFixture;
            private readonly ITestOutputHelper _output;

            public AuthRepositoriesIntegrationTest(AuthRepositoryFixture authRepositoryFixture, ITestOutputHelper output)
            {
                _authRepositoryFixture = authRepositoryFixture;
                _output = output;
            }
            [Fact]
            public async void User_CreateAsync_AssertThatItGetsProperlyCreated()
            {
                // Arrange
                var numRows = await _authRepositoryFixture._applicationUserRepository.GetAll().CountAsync();
                var dummyUser = new ApplicationUser
                {
                    Id = UserTestGUID.GUID4,
                    Email = "dummy@domain.com",
                    NormalizedEmail = "DUMMY@DOMAIN.COM",
                    UserName = "Dummy",
                    NormalizedUserName = "DUMMY",
                };

                // Act
                await _authRepositoryFixture._applicationUserRepository.CreateAsync(dummyUser);

                // Assert
                var newNumRows = await _authRepositoryFixture._applicationUserRepository.GetAll().CountAsync();
                Assert.True((await _authRepositoryFixture._applicationUserRepository.GetAll().ToListAsync()).Contains(dummyUser), "Table does not contain the new item");
                Assert.True(newNumRows == numRows + 1, $"Number of rows did not increase with the new item added ({newNumRows} != {numRows + 1})");
            }
            [Fact]
            public async void User_UpdateAsync_AssertThatItGetsProperlyUpdated()
            {
                // Arrange
                var numRows = await _authRepositoryFixture._applicationUserRepository.GetAll().CountAsync();
                var dummyUser = await _authRepositoryFixture._applicationUserRepository.GetByIdAsync(UserTestGUID.GUID2);

                dummyUser.UserName = "newdummy";

                // Act
                await  _authRepositoryFixture._applicationUserRepository.UpdateAsync(dummyUser);

                // Assert
                var newNumRows = await _authRepositoryFixture._applicationUserRepository.GetAll().CountAsync();
                var queryResult = await _authRepositoryFixture._applicationUserRepository.GetByIdAsync(UserTestGUID.GUID2);
                Assert.NotNull(queryResult);
                Assert.True(queryResult.UserName == "newdummy", "Object property does not match the new updated value");
                Assert.True(newNumRows == numRows, $"Number of rows did not stay the same with the update ({newNumRows} != {numRows})");
            }
            [Fact]
            public async void User_RemoveAsync_AssertThatItGetsProperlyRemoved()
            {
                // Arrange
                var numRows = await _authRepositoryFixture._applicationUserRepository.GetAll().CountAsync();
                var dummyUser = await _authRepositoryFixture._applicationUserRepository.GetByIdAsync(UserTestGUID.GUID1);

                // Act
                await _authRepositoryFixture._applicationUserRepository.RemoveByIdAsync(dummyUser.Id);

                // Assert
                var newNumRows = await _authRepositoryFixture._applicationUserRepository.GetAll().CountAsync();
                Assert.False((await _authRepositoryFixture._applicationUserRepository.GetAll().ToListAsync()).Contains(dummyUser), "Table still contains the item");
                Assert.True(newNumRows == numRows - 1, $"Number of rows did not decrease with the item removed ({ newNumRows} != { numRows - 1})");
            }
            [Fact]
            public async void User_GetByIdAsync_AssertThatItGetsProperlyRemoved()
            {
                // Arrange
                // Act
                var dummyUser1 = await _authRepositoryFixture._applicationUserRepository.GetByIdAsync(UserTestGUID.GUID4);
                var dummyUser2 = await _authRepositoryFixture._applicationUserRepository.GetByIdAsync(UserTestGUID.GUID5); // invalid GUID

                // Assert
                Assert.NotNull(dummyUser1);
                Assert.Null(dummyUser2);
            }
            [Theory]
            [InlineData(50, SortType.Ascending)]
            [InlineData(1, SortType.Ascending)]
            [InlineData(0, SortType.Ascending)]
            [InlineData(4, SortType.Ascending)]
            [InlineData(-1, SortType.Ascending)]
            [InlineData(50, SortType.Descending)]
            [InlineData(1, SortType.Descending)]
            [InlineData(0, SortType.Descending)]
            [InlineData(4, SortType.Descending)]
            [InlineData(-1, SortType.Descending)]
            public void User_SearchAndOrderAsync_AssertThatItGetsProperlySorted(int numRecords, SortType sortType)
            {
                /// Arrange
                var response = _authRepositoryFixture._applicationUserRepository.SearchAndOrderAsync(_ => true, new ApplicationUserSortOptions(sortType), numRecords);

                Assert.True(response.Count() <= (numRecords < 0 ? 0 : numRecords));
                if (sortType == SortType.Ascending)
                {
                    response.Should().BeEquivalentTo(response.OrderBy(user => user.UserName));
                }
                else
                {
                    response.Should().BeEquivalentTo(response.OrderByDescending(user => user.UserName));
                }
            }
            [Theory]
            //[InlineData("test@email.com", false)] // gets removed
            [InlineData("test2@email.com", false)] // gets updated
            [InlineData("test3@email.com", false)]
            [InlineData("fake@email.com", true)]
            public async void User_SearchFirstAsync_AssertThatItWorksProperly(string email, bool expectNull)
            {
                // Arrange
                // Act
                var response = await _authRepositoryFixture._applicationUserRepository.SearchFirstAsync(u => u.Email == email);

                // Assert
                bool isResponseNull = response == null;
                Assert.True(isResponseNull == expectNull);
            }
            [Fact]
            public async void Role_CreateAsync_AssertThatItGetsProperlyCreated()
            {
                // Arrange
                var numRows = await _authRepositoryFixture._roleRepository.GetAll().CountAsync();
                var dummyRole = new ApplicationRole
                {
                    Id = RoleTestGUID.GUID4,
                    Name = "new_admin"
                };

                // Act
                await _authRepositoryFixture._roleRepository.CreateAsync(dummyRole);

                // Assert
                var newNumRows = await _authRepositoryFixture._roleRepository.GetAll().CountAsync();
                Assert.True((await _authRepositoryFixture._roleRepository.GetAll().ToListAsync()).Contains(dummyRole), "Table does not contain the new item");
                Assert.True(newNumRows == numRows + 1, $"Number of rows did not increase with the new item added ({newNumRows} != {numRows + 1})");
            }
            [Fact]
            public async void Role_UpdateAsync_AssertThatItGetsProperlyUpdated()
            {
                // Arrange
                var numRows = await _authRepositoryFixture._roleRepository.GetAll().CountAsync();
                var dummyRole = await _authRepositoryFixture._roleRepository.GetByIdAsync(RoleTestGUID.GUID1);

                dummyRole.Name = "altered_name";

                // Act
                await _authRepositoryFixture._roleRepository.UpdateAsync(dummyRole);

                // Assert
                var newNumRows = await _authRepositoryFixture._roleRepository.GetAll().CountAsync();
                var queryResult = await _authRepositoryFixture._roleRepository.GetByIdAsync(RoleTestGUID.GUID1);
                Assert.NotNull(queryResult);
                Assert.True(queryResult.Name == "altered_name", "Object property does not match the new updated value");
                Assert.True(newNumRows == numRows, $"Number of rows did not stay the same with the update ({newNumRows} != {numRows})");
            }
            [Fact]
            public async void Role_RemoveAsync_AssertThatItGetsProperlyRemoved()
            {
                // Arrange
                var numRows = await _authRepositoryFixture._roleRepository.GetAll().CountAsync();
                var dummyRole = await _authRepositoryFixture._roleRepository.GetByIdAsync(RoleTestGUID.GUID2);

                // Act
                await _authRepositoryFixture._roleRepository.RemoveByIdAsync(dummyRole.Id);

                // Assert
                var newNumRows = await _authRepositoryFixture._roleRepository.GetAll().CountAsync();
                Assert.False((await _authRepositoryFixture._roleRepository.GetAll().ToListAsync()).Contains(dummyRole), "Table still contains the item");
                Assert.True(newNumRows == numRows - 1, $"Number of rows did not decrease with the item removed ({ newNumRows} != { numRows - 1})");
            }
            [Fact]
            public async void Role_GetByIdAsync_AssertThatItGetsProperlyRemoved()
            {
                // Arrange
                // Act
                var dummyRole1 = await _authRepositoryFixture._roleRepository.GetByIdAsync(RoleTestGUID.GUID1);
                var dummyRole2 = await _authRepositoryFixture._roleRepository.GetByIdAsync(RoleTestGUID.GUID5); // invalid GUID

                // Assert
                Assert.NotNull(dummyRole1);
                Assert.Null(dummyRole2);
            }
            [Theory]
            [InlineData(50, SortType.Ascending)]
            [InlineData(1, SortType.Ascending)]
            [InlineData(0, SortType.Ascending)]
            [InlineData(4, SortType.Ascending)]
            [InlineData(-1, SortType.Ascending)]
            [InlineData(50, SortType.Descending)]
            [InlineData(1, SortType.Descending)]
            [InlineData(0, SortType.Descending)]
            [InlineData(4, SortType.Descending)]
            [InlineData(-1, SortType.Descending)]
            public void Role_SearchAndOrderAsync_AssertThatItWorksProperly(int numRecords, SortType sortType)
            {
                /// Arrange
                var response = _authRepositoryFixture._roleRepository.SearchAndOrderAsync(_ => true, new RoleSortOptions(sortType), numRecords);

                Assert.True(response.Count() <= (numRecords < 0 ? 0 : numRecords));
                if (sortType == SortType.Ascending)
                {
                    response.Should().BeEquivalentTo(response.OrderBy(role => role.Name));
                }
                else
                {
                    response.Should().BeEquivalentTo(response.OrderByDescending(role => role.Name));
                }
            }
            [Theory]
            [InlineData("admin", false)]
            [InlineData("user", false)]
            [InlineData("visitor", false)] // gets deleted during testing
            [InlineData("new_one", true)]
            public async void Role_SearchFirstAsync_AssertThatItWorksProperly(string roleName, bool expectNull)
            {
                // Arrange
                // Act
                var response = await _authRepositoryFixture._roleRepository.SearchFirstAsync(role => role.Name == roleName);

                // Assert
                bool isResponseNull = response == null;
                Assert.True(isResponseNull == expectNull);
            }
            public class User_SearchAndOrderAsync_AssertThatPredicateIsConsidered_TestConfig
            {
                public static IEnumerable<object[]> TestCases
                {
                    get
                    {
                        //yield return new object[] { (Expression<Func<ApplicationUser, bool>>)((u) => u.Email == "test@email.com"), 10, SortType.Ascending, 1 };
                        yield return new object[] { (Expression<Func<ApplicationUser, bool>>)((u) => u.Email == "test2@email.com"), 10, SortType.Ascending, 1 };
                        yield return new object[] { (Expression<Func<ApplicationUser, bool>>)((u) => u.Email == "test3@email.com"), 10, SortType.Ascending, 1 };
                        yield return new object[] { (Expression<Func<ApplicationUser, bool>>)((u) => u.Email == "inexistant@email.com"), 10, SortType.Ascending, 0 };
                        //yield return new object[] { (Expression<Func<ApplicationUser, bool>>)((u) => u.Email.Contains("email")), 10, SortType.Ascending, 3 };
                    }
                }
            }
            [Theory]
            [MemberData(nameof(User_SearchAndOrderAsync_AssertThatPredicateIsConsidered_TestConfig.TestCases), MemberType = typeof(User_SearchAndOrderAsync_AssertThatPredicateIsConsidered_TestConfig))]
            public void User_SearchAndOrderAsync_AssertThatPredicateIsConsidered(Expression<Func<ApplicationUser, bool>> predicate, int numRecords, SortType sortType, int expectedNumberOfRecordsReturned)
            {
                /// Arrange
                var response = _authRepositoryFixture._applicationUserRepository.SearchAndOrderAsync(predicate, new ApplicationUserSortOptions(sortType, ApplicationUserSortOptions.ColumnOptions.EMAIL), numRecords);

                Assert.True(response.Count() <= (numRecords < 0 ? 0 : numRecords));
                if (sortType == SortType.Ascending)
                {
                    response.Should().BeEquivalentTo(response.OrderBy(user => user.Email));
                }
                else
                {
                    response.Should().BeEquivalentTo(response.OrderByDescending(user => user.Email));
                }
                Assert.True(response.Count() == expectedNumberOfRecordsReturned);
            }

            public class Role_SearchAndOrderAsync_AssertThatPredicateIsConsidered_TestConfig
            {
                public static IEnumerable<object[]> TestCases
                {
                    get
                    {
                        //yield return new object[] { (Expression<Func<ApplicationRole, bool>>)((r) => r.Name == "altered_name"), 10, SortType.Ascending, 1 };
                        //yield return new object[] { (Expression<Func<ApplicationRole, bool>>)((r) => r.Name == "user"), 10, SortType.Ascending, 0 }; // this needs to be refactored to avoid "test concurrence" issues
                        yield return new object[] { (Expression<Func<ApplicationRole, bool>>)((r) => r.Name == "visitor"), 10, SortType.Ascending, 1 };
                        yield return new object[] { (Expression<Func<ApplicationRole, bool>>)((r) => r.Name == "inexistant"), 10, SortType.Ascending, 0 };
                    }
                }
            }
            [Theory]
            [MemberData(nameof(Role_SearchAndOrderAsync_AssertThatPredicateIsConsidered_TestConfig.TestCases), MemberType = typeof(Role_SearchAndOrderAsync_AssertThatPredicateIsConsidered_TestConfig))]
            public void Role_SearchAndOrderAsync_AssertThatPredicateIsConsidered(Expression<Func<ApplicationRole, bool>> predicate, int numRecords, SortType sortType, int expectedNumberOfRecordsReturned)
            {
                /// Arrange
                var response = _authRepositoryFixture._roleRepository.SearchAndOrderAsync(predicate, new RoleSortOptions(sortType), numRecords);

                Assert.True(response.Count() <= (numRecords < 0 ? 0 : numRecords));
                if (sortType == SortType.Ascending)
                {
                    response.Should().BeEquivalentTo(response.OrderBy(role => role.Name));
                }
                else
                {
                    response.Should().BeEquivalentTo(response.OrderByDescending(role => role.Name));
                }
                Assert.True(response.Count() == expectedNumberOfRecordsReturned);
            }
        }
    }

}