using API.ViewModels;
using Data.Context;
using Data.Models.Implementation;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Tests.Integration.Fixtures;
using Xunit;

namespace Tests.Tests.Integration.Implementation
{
    public interface IIdentityControllerTests
    {
        // Login tests
        Task SuccessfulLogin(LoginRequestModel request);
        Task FailedLoginWithWrongCredentials(LoginRequestModel request);
        // JWT tests
        Task SuccessfulTokenRenewal(string expiredAccesstoken, string refreshToken);
        Task FailedTokenRenewalWithInvalidAT(string expiredAccesstoken, string refreshToken);
        Task FailedTokenRenewalWithNotExpiredAT(string expiredAccesstoken, string refreshToken);
        Task FailedTokenRenewalWithInvalidRT(string expiredAccesstoken, string refreshToken);
        Task FailedTokenRenewalWithExpiredRT(string expiredAccesstoken, string refreshToken);
        Task FailedTokenRenewalWithUnmatchedTokens(string expiredAccesstoken, string refreshToken);
    }

    public abstract class IdentityControllerTests : IIdentityControllerTests, IClassFixture<IntegrationTestFixture>
    {
        protected IntegrationTestFixture _fixture;
        protected HttpClient _client;

        protected static string _baseEndpoint = $"/api/v1/auth";
        protected static string _loginEndpoint = $"{_baseEndpoint}/login";
        protected static string _renewEndpoint = $"{_baseEndpoint}/refresh";
        protected readonly static User _dummyUser = new User() { Name = "Test", Surname = "User", UserName = "testuser", NormalizedUserName = "testuser", Email = "testuser@email.com", NormalizedEmail = "testuser@email.com" };

        public IdentityControllerTests(IntegrationTestFixture fixture)
        {
            _fixture = fixture;
            _client = fixture.HttpClient;

            SetupDummyUsers().Wait();
        }

        protected async Task SetupDummyUsers()
        {
            using (var scope = _fixture.Host.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetService<UserManager<User>>();
                if (await userManager.FindByIdAsync(_dummyUser.Id) == null)
                {
                    await userManager.CreateAsync(_dummyUser);
                    await userManager.AddPasswordAsync(_dummyUser, "Test@123");
                }
            }
        }

        // Login tests
        public static IEnumerable<object[]> SuccessfulLoginData
        {
            get
            {
                yield return new object[] { new LoginRequestModel() { Username = _dummyUser.UserName, Password = "Test@123" } };
            }
        }

        [Theory, MemberData(nameof(SuccessfulLoginData))]
        public virtual async Task SuccessfulLogin(LoginRequestModel request)
        {
            // Arrange

            // Act
            var response = await _client.PostAsync($"{_loginEndpoint}", JsonContent.Create(request));
            var parsedResponse = await response.Content.ReadFromJsonAsync<LoginResponseModel>();

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            parsedResponse.AccessToken.Should().NotBeNullOrEmpty();
            parsedResponse.RefreshToken.Should().NotBeNullOrEmpty();
            parsedResponse.Errors.Should().BeNullOrEmpty();
        }

        public virtual Task FailedLoginWithWrongCredentials(LoginRequestModel request)
        {
            throw new System.NotImplementedException();
        }

        // JWT tests
        public virtual Task SuccessfulTokenRenewal(string expiredAccesstoken, string refreshToken)
        {
            throw new System.NotImplementedException();
        }

        public virtual Task FailedTokenRenewalWithInvalidAT(string expiredAccesstoken, string refreshToken)
        {
            throw new System.NotImplementedException();
        }

        public virtual Task FailedTokenRenewalWithNotExpiredAT(string expiredAccesstoken, string refreshToken)
        {
            throw new System.NotImplementedException();
        }

        public virtual Task FailedTokenRenewalWithInvalidRT(string expiredAccesstoken, string refreshToken)
        {
            throw new System.NotImplementedException();
        }

        public virtual Task FailedTokenRenewalWithExpiredRT(string expiredAccesstoken, string refreshToken)
        {
            throw new System.NotImplementedException();
        }

        public virtual Task FailedTokenRenewalWithUnmatchedTokens(string expiredAccesstoken, string refreshToken)
        {
            throw new System.NotImplementedException();
        }
    }

    public class AuthControllerTests : IdentityControllerTests
    {
        public AuthControllerTests(IntegrationTestFixture fixture) : base(fixture) { }
    }
}
