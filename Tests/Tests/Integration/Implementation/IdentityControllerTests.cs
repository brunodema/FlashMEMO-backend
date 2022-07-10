using API.Controllers;
using API.ViewModels;
using Business.Services.Implementation;
using Business.Services.Interfaces;
using Data.Models.Implementation;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
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
        // Missing tests for: (1) login with blocked user, (2) login with unactivated user

        // JWT tests
        Task SuccessfulTokenRenewal();
        Task FailedTokenRenewalWithInvalidAT();
        Task FailedTokenRenewalWithNotExpiredAT();
        Task FailedTokenRenewalWithInvalidRT();
        Task FailedTokenRenewalWithExpiredRT();
        Task FailedTokenRenewalWithUnmatchedTokens();
        // Missing tests for: (1) token renewal with blocked user, (2) token renewal with unactivated user

        // Activation tests
        Task SuccessfulActivation();
        Task FailedActivationWithInvalidToken();
        Task FailedActivationWithExpiredToken();
        Task FailedActivationWithAlreadyActivatedUser();

        // Password recovery request tests
        Task SuccessfulRecoveryRequest();
        Task SuccessfulRecoveryRequestWithInvalidEmail();
        Task FailedRecoveryRequestWithUnactivatedAccount();

        // Password recovery tests
        Task SuccessfulPasswordReset();
        Task FailedPasswordResetWithInvalidToken();
        Task FailedPasswordResetWithExpiredToken();
        Task FailedPasswordResetWithUnactivatedAccount();
        Task FailedPasswordResetWithLockedAccount();
    }

    public abstract class IdentityControllerTests : IIdentityControllerTests, IClassFixture<IntegrationTestFixture>
    {
        protected IntegrationTestFixture _fixture;
        protected HttpClient _client;
        protected JWTServiceOptions _jwtOptions;

        protected static string _baseEndpoint = $"/api/v1/auth";
        protected static string _loginEndpoint = $"{_baseEndpoint}/login";
        protected static string _refreshEndpoint = $"{_baseEndpoint}/refresh";
        protected static string _activationEndpoint = $"{_baseEndpoint}/activate";
        protected static string _passwordRecoveryRequestEndpoint = $"{_baseEndpoint}/forgot-password";
        protected static string _passwordResetEndpoint = $"{_baseEndpoint}/reset-password";

        protected readonly static User _dummyUser = new User() { Name = "Test", Surname = "User", UserName = "testuser", NormalizedUserName = "testuser", Email = "testuser@email.com", NormalizedEmail = "testuser@email.com", EmailConfirmed = true };

        protected readonly static User _dummyActivationUser = new User() { Name = "Test2", Surname = "User2", UserName = "testuser2", NormalizedUserName = "testuser2", Email = "testuser2@email.com", NormalizedEmail = "testuser2@email.com", EmailConfirmed = false };

        protected readonly static User _dummyLockedUser = new User() { Name = "Test3", Surname = "User3", UserName = "testuser3", NormalizedUserName = "testuser3", Email = "testuser3@email.com", NormalizedEmail = "testuser3@email.com", EmailConfirmed = true, LockoutEnabled = true, LockoutEnd = DateTimeOffset.Now.AddSeconds(1000) };

        protected readonly static User _dummyPasswordResetUser = new User() { Name = "Test4", Surname = "User4", UserName = "testuser4", NormalizedUserName = "testuser4", Email = "testuser4@email.com", NormalizedEmail = "testuser4@email.com", EmailConfirmed = true };

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

                if (await userManager.FindByIdAsync(_dummyActivationUser.Id) == null)
                {
                    await userManager.CreateAsync(_dummyActivationUser);
                    await userManager.AddPasswordAsync(_dummyActivationUser, _dummyPassword);
                }

                if (await userManager.FindByIdAsync(_dummyLockedUser.Id) == null)
                {
                    await userManager.CreateAsync(_dummyLockedUser);
                    await userManager.AddPasswordAsync(_dummyLockedUser, _dummyPassword);
                }

                if (await userManager.FindByIdAsync(_dummyPasswordResetUser.Id) == null)
                {
                    await userManager.CreateAsync(_dummyPasswordResetUser);
                    await userManager.AddPasswordAsync(_dummyPasswordResetUser, _dummyPassword);
                }
            }
        }

        protected async Task<HttpResponseMessage> LoginWithValidCredentialsAsync()
        {
            return await _client.PostAsync($"{_loginEndpoint}", JsonContent.Create(new LoginRequestModel() { Username = _dummyUser.UserName, Password = _dummyPassword }));
        }

        #region Login tests
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
            parsedResponse.Message.Should().Be(AuthController.ResponseMessages.LOGIN_CREDENTIALS_COULD_NOT_BE_VALIDATED);
        }

        [Fact]
        public virtual async Task FailedLoginWithUnactivatedUser()
        {
            // Arrange
            var request = new LoginRequestModel() { Username = _dummyActivationUser.UserName, Password = _dummyPassword };

            // Act
            var response = await _client.PostAsync($"{_loginEndpoint}", JsonContent.Create(request));

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
            var parsedResponse = await response.Content.ReadFromJsonAsync<LoginResponseModel>();
            parsedResponse.Message.Should().Be(AuthController.ResponseMessages.LOGIN_ACTIVATION_PENDING);
        }

        [Fact]
        public virtual async Task FailedLoginWithLockedUser()
        {
            // Arrange
            var request = new LoginRequestModel() { Username = _dummyLockedUser.UserName, Password = _dummyPassword };

            // Act
            var response = await _client.PostAsync($"{_loginEndpoint}", JsonContent.Create(request));

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
            var parsedResponse = await response.Content.ReadFromJsonAsync<LoginResponseModel>();
            parsedResponse.Message.Should().Be(AuthController.ResponseMessages.LOGIN_ACCOUNT_IS_LOCKED);
        }
        #endregion

        #region Token renewal tests
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
            postRequest.Headers.Add("Cookie", $"RefreshToken={refreshToken}");
            postRequest.Content = JsonContent.Create(new RefreshRequestModel() { ExpiredAccessToken = accessToken });

            var refreshResponse = await _client.SendAsync(postRequest);

            // Assert
            refreshResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var parsedRefreshResponse = await refreshResponse.Content.ReadFromJsonAsync<LoginResponseModel>();
            parsedRefreshResponse.AccessToken.Should().NotBeNullOrEmpty();
            parsedRefreshResponse.RefreshToken.Should().NotBeNullOrEmpty();
            parsedRefreshResponse.Errors.Should().BeNullOrEmpty();
            parsedRefreshResponse.Message.Should().Be(AuthController.ResponseMessages.RENEWAL_ACCESS_TOKEN_RENEWED);
        }

        [Fact]
        public virtual async Task FailedTokenRenewalWithInvalidAT()
        {
            // Arrange
            var riggedJWTService = new JWTService(Options.Create(new JWTServiceOptions()
            {
                ValidIssuer = _jwtOptions.ValidIssuer,
                ValidAudience = _jwtOptions.ValidAudience,
                Secret = _jwtOptions.Secret,
                AccessTokenTTE = _jwtOptions.RefreshTokenTTE,
                RefreshTokenTTE = _jwtOptions.RefreshTokenTTE
            }));

            var accessToken = riggedJWTService.CreateAccessToken(_dummyUser) + "corrupt_ending"; // Add random termination to corrupt the JWT
            var refreshToken = riggedJWTService.CreateRefreshToken(accessToken, _dummyUser);

            // Act]
            var postRequest = new HttpRequestMessage(HttpMethod.Post, _refreshEndpoint);
            postRequest.Headers.Add("Cookie", $"RefreshToken={refreshToken}");
            postRequest.Content = JsonContent.Create(new RefreshRequestModel() { ExpiredAccessToken = accessToken });

            var refreshResponse = await _client.SendAsync(postRequest);

            // Assert
            refreshResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            var parsedRefreshResponse = await refreshResponse.Content.ReadFromJsonAsync<BaseResponseModel>();
            parsedRefreshResponse.Message.Should().Be(AuthController.ResponseMessages.RENEWAL_ACCESS_TOKEN_INVALID);
        }

        [Fact]
        public virtual async Task FailedTokenRenewalWithNotExpiredAT()
        {
            // Arrange
            var riggedJWTService = new JWTService(Options.Create(new JWTServiceOptions()
            {
                ValidIssuer = _jwtOptions.ValidIssuer,
                ValidAudience = _jwtOptions.ValidAudience,
                Secret = _jwtOptions.Secret,
                AccessTokenTTE = 100000, // Very large value to guarantee it won't expire (not like more than a second passes between lines here...)
                RefreshTokenTTE = _jwtOptions.RefreshTokenTTE
            }));

            var accessToken = riggedJWTService.CreateAccessToken(_dummyUser);
            var refreshToken = riggedJWTService.CreateRefreshToken(accessToken, _dummyUser);

            // Act]
            var postRequest = new HttpRequestMessage(HttpMethod.Post, _refreshEndpoint);
            postRequest.Headers.Add("Cookie", $"RefreshToken={refreshToken}");
            postRequest.Content = JsonContent.Create(new RefreshRequestModel() { ExpiredAccessToken = accessToken });

            var refreshResponse = await _client.SendAsync(postRequest);

            // Assert
            refreshResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var parsedRefreshResponse = await refreshResponse.Content.ReadFromJsonAsync<BaseResponseModel>();
            parsedRefreshResponse.Message.Should().Be(AuthController.ResponseMessages.RENEWAL_ACCES_TOKEN_STILL_VALID);
        }

        [Fact]
        public virtual async Task FailedTokenRenewalWithInvalidRT()
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
            var refreshToken = riggedJWTService.CreateRefreshToken(accessToken, _dummyUser) + "corrupt_ending"; // Add random termination to corrupt the JWT;

            // Act]
            var postRequest = new HttpRequestMessage(HttpMethod.Post, _refreshEndpoint);
            postRequest.Headers.Add("Cookie", $"RefreshToken={refreshToken}");
            postRequest.Content = JsonContent.Create(new RefreshRequestModel() { ExpiredAccessToken = accessToken });

            var refreshResponse = await _client.SendAsync(postRequest);

            // Assert
            refreshResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            var parsedRefreshResponse = await refreshResponse.Content.ReadFromJsonAsync<BaseResponseModel>();
            parsedRefreshResponse.Message.Should().Be(AuthController.ResponseMessages.RENEWAL_REFRESH_TOKEN_INVALID);
        }

        [Fact]
        public virtual async Task FailedTokenRenewalWithExpiredRT()
        {
            // Arrange
            var riggedJWTService = new JWTService(Options.Create(new JWTServiceOptions()
            {
                ValidIssuer = _jwtOptions.ValidIssuer,
                ValidAudience = _jwtOptions.ValidAudience,
                Secret = _jwtOptions.Secret,
                AccessTokenTTE = -1,
                RefreshTokenTTE = -1
            }));

            var accessToken = riggedJWTService.CreateAccessToken(_dummyUser);
            var refreshToken = riggedJWTService.CreateRefreshToken(accessToken, _dummyUser);

            // Act]
            var postRequest = new HttpRequestMessage(HttpMethod.Post, _refreshEndpoint);
            postRequest.Headers.Add("Cookie", $"RefreshToken={refreshToken}");
            postRequest.Content = JsonContent.Create(new RefreshRequestModel() { ExpiredAccessToken = accessToken });

            var refreshResponse = await _client.SendAsync(postRequest);

            // Assert
            refreshResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            var parsedRefreshResponse = await refreshResponse.Content.ReadFromJsonAsync<BaseResponseModel>();
            parsedRefreshResponse.Message.Should().Be(AuthController.ResponseMessages.RENEWAL_REFRESH_TOKEN_INVALID);
        }

        [Fact]
        public virtual async Task FailedTokenRenewalWithUnmatchedTokens()
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
            var wrongAccessToken = riggedJWTService.CreateAccessToken(_dummyUser);
            var refreshToken = riggedJWTService.CreateRefreshToken(wrongAccessToken, _dummyUser); // I create the RT using a completely unrelated AT. Then, I send the original AT with this "faulty" RT

            // Act]
            var postRequest = new HttpRequestMessage(HttpMethod.Post, _refreshEndpoint);
            postRequest.Headers.Add("Cookie", $"RefreshToken={refreshToken}");
            postRequest.Content = JsonContent.Create(new RefreshRequestModel() { ExpiredAccessToken = accessToken });

            var refreshResponse = await _client.SendAsync(postRequest);

            // Assert
            refreshResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            var parsedRefreshResponse = await refreshResponse.Content.ReadFromJsonAsync<BaseResponseModel>();
            parsedRefreshResponse.Message.Should().Be(AuthController.ResponseMessages.RENEWAL_UNRELATED_TOKENS);
        }

        [Fact]
        public virtual async Task FailedTokenRenewalWithUnactivatedUser()
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

            var accessToken = riggedJWTService.CreateAccessToken(_dummyActivationUser);
            var refreshToken = riggedJWTService.CreateRefreshToken(accessToken, _dummyActivationUser);

            // Act]
            var postRequest = new HttpRequestMessage(HttpMethod.Post, _refreshEndpoint);
            postRequest.Headers.Add("Cookie", $"RefreshToken={refreshToken}");
            postRequest.Content = JsonContent.Create(new RefreshRequestModel() { ExpiredAccessToken = accessToken });

            var refreshResponse = await _client.SendAsync(postRequest);

            // Assert
            refreshResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            var parsedRefreshResponse = await refreshResponse.Content.ReadFromJsonAsync<BaseResponseModel>();
            parsedRefreshResponse.Message.Should().Be(AuthController.ResponseMessages.GENERAL_ACTIVATION_PENDING);
        }

        [Fact]
        public virtual async Task FailedTokenRenewalWithLockedUser()
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

            var accessToken = riggedJWTService.CreateAccessToken(_dummyLockedUser);
            var refreshToken = riggedJWTService.CreateRefreshToken(accessToken, _dummyLockedUser);

            // Act]
            var postRequest = new HttpRequestMessage(HttpMethod.Post, _refreshEndpoint);
            postRequest.Headers.Add("Cookie", $"RefreshToken={refreshToken}");
            postRequest.Content = JsonContent.Create(new RefreshRequestModel() { ExpiredAccessToken = accessToken });

            var refreshResponse = await _client.SendAsync(postRequest);

            // Assert
            refreshResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            var parsedRefreshResponse = await refreshResponse.Content.ReadFromJsonAsync<BaseResponseModel>();
            parsedRefreshResponse.Message.Should().Be(AuthController.ResponseMessages.GENERAL_ACCOUNT_LOCKED);
        }
        #endregion

        #region Activation tests
        [Fact]
        public async Task SuccessfulActivation()
        {
            // Arrange
            var riggedJWTService = new JWTService(Options.Create(new JWTServiceOptions()
            {
                ValidIssuer = _jwtOptions.ValidIssuer,
                ValidAudience = _jwtOptions.ValidAudience,
                Secret = _jwtOptions.Secret,
                ActivationTokenTTE = 1000
            }));

            var activationToken = riggedJWTService.CreateActivationToken(_dummyActivationUser);

            // Act
            var activationResponse = await _client.PostAsJsonAsync(_activationEndpoint, activationToken);

            // Assert
            activationResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var parsedActivationResponse = await activationResponse.Content.ReadFromJsonAsync<BaseResponseModel>();
            parsedActivationResponse.Errors.Should().BeNullOrEmpty();
            parsedActivationResponse.Message.Should().Be(AuthController.ResponseMessages.EMAIL_ACCOUNT_ACTIVATION_SUCCESSFUL);
        }

        [Fact]
        public async Task FailedActivationWithInvalidToken()
        {
            // Arrange
            var riggedJWTService = new JWTService(Options.Create(new JWTServiceOptions()
            {
                ValidIssuer = _jwtOptions.ValidIssuer,
                ValidAudience = _jwtOptions.ValidAudience,
                Secret = _jwtOptions.Secret,
                ActivationTokenTTE = 1000
            }));

            var activationToken = riggedJWTService.CreateActivationToken(_dummyActivationUser) + "corrupted_ending";

            // Act
            var activationResponse = await _client.PostAsJsonAsync(_activationEndpoint, activationToken);

            // Assert
            activationResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            var parsedActivationResponse = await activationResponse.Content.ReadFromJsonAsync<BaseResponseModel>();
            parsedActivationResponse.Message.Should().Be(AuthController.ResponseMessages.EMAIL_TOKEN_IS_NOT_VALID);
        }

        [Fact]
        public async Task FailedActivationWithExpiredToken()
        {
            // Arrange
            var riggedJWTService = new JWTService(Options.Create(new JWTServiceOptions()
            {
                ValidIssuer = _jwtOptions.ValidIssuer,
                ValidAudience = _jwtOptions.ValidAudience,
                Secret = _jwtOptions.Secret,
                ActivationTokenTTE = -1
            }));

            var activationToken = riggedJWTService.CreateActivationToken(_dummyActivationUser);

            // Act
            var activationResponse = await _client.PostAsJsonAsync(_activationEndpoint, activationToken);

            // Assert
            activationResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            var parsedActivationResponse = await activationResponse.Content.ReadFromJsonAsync<BaseResponseModel>();
            parsedActivationResponse.Message.Should().Be(AuthController.ResponseMessages.EMAIL_TOKEN_IS_NOT_VALID);
        }

        [Fact]
        public async Task FailedActivationWithAlreadyActivatedUser()
        {
            // Arrange
            var riggedJWTService = new JWTService(Options.Create(new JWTServiceOptions()
            {
                ValidIssuer = _jwtOptions.ValidIssuer,
                ValidAudience = _jwtOptions.ValidAudience,
                Secret = _jwtOptions.Secret,
                ActivationTokenTTE = 1000
            }));

            var activationToken = riggedJWTService.CreateActivationToken(_dummyUser);

            // Act
            var activationResponse = await _client.PostAsJsonAsync(_activationEndpoint, activationToken);

            // Assert
            activationResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            var parsedActivationResponse = await activationResponse.Content.ReadFromJsonAsync<BaseResponseModel>();
            parsedActivationResponse.Message.Should().Be(AuthController.ResponseMessages.EMAIL_ACCOUNT_IS_ALREADY_ACTIVATED);
        }
        #endregion

        #region Password recovery/reset tests
        [Fact]
        public async Task SuccessfulRecoveryRequest()
        {
            // Arrange


            // Act
            var recoveryRequestResponse = await _client.PostAsJsonAsync(_passwordRecoveryRequestEndpoint, _dummyUser.Email);

            // Assert
            recoveryRequestResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var parsedRecoveryRequestResponse = await recoveryRequestResponse.Content.ReadFromJsonAsync<BaseResponseModel>();
            parsedRecoveryRequestResponse.Message.Should().Be(AuthController.ResponseMessages.GENERAL_REQUEST_PROCESSED);
        }

        [Fact]
        public async Task SuccessfulRecoveryRequestWithInvalidEmail()
        {
            // Arrange


            // Act
            var recoveryRequestResponse = await _client.PostAsJsonAsync(_passwordRecoveryRequestEndpoint, "bogus@no-domain.com");

            // Assert
            recoveryRequestResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var parsedRecoveryRequestResponse = await recoveryRequestResponse.Content.ReadFromJsonAsync<BaseResponseModel>();
            parsedRecoveryRequestResponse.Message.Should().Be(AuthController.ResponseMessages.GENERAL_REQUEST_PROCESSED);
        }

        [Fact]
        public async Task FailedRecoveryRequestWithUnactivatedAccount()
        {
            // Arrange


            // Act
            var recoveryRequestResponse = await _client.PostAsJsonAsync(_passwordRecoveryRequestEndpoint, _dummyActivationUser.Email);

            // Assert
            recoveryRequestResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            var parsedRecoveryRequestResponse = await recoveryRequestResponse.Content.ReadFromJsonAsync<BaseResponseModel>();
            parsedRecoveryRequestResponse.Message.Should().Be(AuthController.ResponseMessages.GENERAL_ACTIVATION_PENDING);
        }

        [Fact]
        public async Task SuccessfulPasswordReset()
        {
            using (var scope = _fixture.Host.Services.CreateScope())
            {
                // Arrange
                var authService = scope.ServiceProvider.GetService<IAuthService<string>>();
                var token = await authService.GeneratePasswordResetTokenAsync(_dummyPasswordResetUser);

                // Act
                var requestParams = new ResetPasswordRequestModel() { Username = _dummyPasswordResetUser.UserName, Token = token, NewPassword = "NewPassword@101" };
                var resetResponse = await _client.PostAsJsonAsync(_passwordResetEndpoint, requestParams);

                // Assert
                resetResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
                var parsedResetResponse = await resetResponse.Content.ReadFromJsonAsync<BaseResponseModel>();
                parsedResetResponse.Message.Should().Be(AuthController.ResponseMessages.PASSWORD_RESET_SUCCESSFUL);
            }
        }

        [Fact]
        public async Task FailedPasswordResetWithInvalidToken()
        {
            using (var scope = _fixture.Host.Services.CreateScope())
            {
                // Arrange
                var authService = scope.ServiceProvider.GetService<IAuthService<string>>();
                var token = await authService.GeneratePasswordResetTokenAsync(_dummyPasswordResetUser) + "corrupted_token";

                // Act
                var requestParams = new ResetPasswordRequestModel() { Username = _dummyPasswordResetUser.UserName, Token = token, NewPassword = "NewPassword@101" };
                var resetResponse = await _client.PostAsJsonAsync(_passwordResetEndpoint, requestParams);

                // Assert
                resetResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
                var parsedResetResponse = await resetResponse.Content.ReadFromJsonAsync<BaseResponseModel>();
                parsedResetResponse.Message.Should().Be(AuthController.ResponseMessages.PASSWORD_RESET_NOT_SUCCESSFUL);
            }
        }

        public async Task FailedPasswordResetWithExpiredToken()
        {
            using (var scope = _fixture.Host.Services.CreateScope())
            {
                // Arrange
                var authService = scope.ServiceProvider.GetService<IAuthService<string>>();
                var dtpOptions = scope.ServiceProvider.GetService<IOptions<DataProtectionTokenProviderOptions>>(); // Getting options to change them on the fly here
                dtpOptions.Value.TokenLifespan = TimeSpan.FromSeconds(-1);
                var token = await authService.GeneratePasswordResetTokenAsync(_dummyPasswordResetUser);

                // Act
                var requestParams = new ResetPasswordRequestModel() { Username = _dummyPasswordResetUser.UserName, Token = token, NewPassword = "NewPassword@101" };
                var resetResponse = await _client.PostAsJsonAsync(_passwordResetEndpoint, requestParams);

                // Assert
                resetResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
                var parsedResetResponse = await resetResponse.Content.ReadFromJsonAsync<BaseResponseModel>();
                parsedResetResponse.Message.Should().Be(AuthController.ResponseMessages.PASSWORD_RESET_NOT_SUCCESSFUL);
            }
        }

        public async Task FailedPasswordResetWithUnactivatedAccount()
        {
            using (var scope = _fixture.Host.Services.CreateScope())
            {
                // Arrange
                var authService = scope.ServiceProvider.GetService<IAuthService<string>>();
                var token = await authService.GeneratePasswordResetTokenAsync(_dummyActivationUser);

                // Act
                var requestParams = new ResetPasswordRequestModel() { Username = _dummyActivationUser.UserName, Token = token, NewPassword = "NewPassword@101" };
                var resetResponse = await _client.PostAsJsonAsync(_passwordResetEndpoint, requestParams);

                // Assert
                resetResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
                var parsedResetResponse = await resetResponse.Content.ReadFromJsonAsync<BaseResponseModel>();
                parsedResetResponse.Message.Should().Be(AuthController.ResponseMessages.GENERAL_ACTIVATION_PENDING);
            }
        }

        public async Task FailedPasswordResetWithLockedAccount()
        {
            using (var scope = _fixture.Host.Services.CreateScope())
            {
                // Arrange
                var authService = scope.ServiceProvider.GetService<IAuthService<string>>();
                var token = await authService.GeneratePasswordResetTokenAsync(_dummyLockedUser);

                // Act
                var requestParams = new ResetPasswordRequestModel() { Username = _dummyLockedUser.UserName, Token = token, NewPassword = "NewPassword@101" };
                var resetResponse = await _client.PostAsJsonAsync(_passwordResetEndpoint, requestParams);

                // Assert
                resetResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
                var parsedResetResponse = await resetResponse.Content.ReadFromJsonAsync<BaseResponseModel>();
                parsedResetResponse.Message.Should().Be(AuthController.ResponseMessages.GENERAL_ACCOUNT_LOCKED);
            }
        }
        #endregion

    }

    public class AuthControllerTests : IdentityControllerTests
    {
        public AuthControllerTests(IntegrationTestFixture fixture) : base(fixture) { }
    }
}
