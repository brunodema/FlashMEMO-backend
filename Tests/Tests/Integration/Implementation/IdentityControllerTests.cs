﻿using API.Controllers;
using API.ViewModels;
using Business.Services.Implementation;
using Data.Context;
using Data.Models.Implementation;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
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
        Task SuccessfulTokenRenewal();
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
        protected JWTServiceOptions _jwtOptions;

        protected static string _baseEndpoint = $"/api/v1/auth";
        protected static string _loginEndpoint = $"{_baseEndpoint}/login";
        protected static string _refreshEndpoint = $"{_baseEndpoint}/refresh";
        protected readonly static User _dummyUser = new User() { Name = "Test", Surname = "User", UserName = "testuser", NormalizedUserName = "testuser", Email = "testuser@email.com", NormalizedEmail = "testuser@email.com" };
        protected readonly static string _dummyPassword = "Test@123";

        public IdentityControllerTests(IntegrationTestFixture fixture)
        {
            _fixture = fixture;
            _client = fixture.HttpClient;
            _jwtOptions = fixture.Host.Services.GetService<IOptions<JWTServiceOptions>>().Value;

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
                    await userManager.AddPasswordAsync(_dummyUser, _dummyPassword);
                }
            }
        }

        protected async Task<HttpResponseMessage> LoginWithValidCredentialsAsync()
        {
            return await _client.PostAsync($"{_loginEndpoint}", JsonContent.Create(new LoginRequestModel() { Username = _dummyUser.UserName, Password = _dummyPassword }));
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

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var parsedResponse = await response.Content.ReadFromJsonAsync<LoginResponseModel>();
            parsedResponse.AccessToken.Should().NotBeNullOrEmpty();
            parsedResponse.RefreshToken.Should().NotBeNullOrEmpty();
            parsedResponse.Errors.Should().BeNullOrEmpty();
        }

        // Login tests
        public static IEnumerable<object[]> FailedLoginWithWrongCredentialsData
        {
            get
            {
                yield return new object[] { new LoginRequestModel() { Username = _dummyUser.UserName, Password = "Wrong@Password123" } };
            }
        }

        [Theory, MemberData(nameof(FailedLoginWithWrongCredentialsData))]
        public virtual async Task FailedLoginWithWrongCredentials(LoginRequestModel request)
        {
            // Arrange

            // Act
            var response = await _client.PostAsync($"{_loginEndpoint}", JsonContent.Create(request));

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
            var parsedResponse = await response.Content.ReadFromJsonAsync<LoginResponseModel>();
            parsedResponse.Message.Should().Be(AuthController.ResponseMessages.CREDENTIALS_COULD_NOT_BE_VALIDATED);
        }

        // JWT tests
        [Fact]
        public virtual async Task SuccessfulTokenRenewal()
        {
            // Arrange
            var riggedJWTService = new JWTService(Options.Create(new JWTServiceOptions()
            {
                ValidIssuer = _jwtOptions.ValidIssuer,
                ValidAudience = _jwtOptions.ValidAudience,
                Secret = _jwtOptions.Secret,
                AccessTokenTTE = -1,
                RefreshTokenTTE = _jwtOptions.RefreshTokenTTE
            }));

            var accessToken = riggedJWTService.CreateAccessToken(_dummyUser);
            var refreshToken = riggedJWTService.CreateRefreshToken(accessToken, _dummyUser);

            // Act]
            var postRequest = new HttpRequestMessage(HttpMethod.Post, _refreshEndpoint);
            postRequest.Headers.Add("Cookie", $"refreshToken={refreshToken}");
            postRequest.Content = JsonContent.Create(new RefreshRequestModel() { ExpiredAccessToken = accessToken, RefreshToken = refreshToken });

            var refreshResponse = await _client.SendAsync(postRequest);

            // Assert
            refreshResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var parsedRefreshResponse = await refreshResponse.Content.ReadFromJsonAsync<LoginResponseModel>();
            parsedRefreshResponse.AccessToken.Should().NotBeNullOrEmpty();
            parsedRefreshResponse.RefreshToken.Should().NotBeNullOrEmpty();
            parsedRefreshResponse.Errors.Should().BeNullOrEmpty();
            parsedRefreshResponse.Message.Should().Be(AuthController.ResponseMessages.ACCESS_TOKEN_RENEWED);
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