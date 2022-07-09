using Data.Tools.Sorting;
using Data.Tools.Filtering;
using Tests.Integration.Interfaces;
using Data.Models.Implementation;
using Tests.Integration.Fixtures;
using Data.Context;
using Microsoft.Extensions.DependencyInjection;
using Business.Services.Implementation;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

namespace Tests.Integration.Auxiliary
{
    #region ControllerAuthTokenInjector
    /// <summary>
    /// This class is used to handle authentication for integration testing. Since many endpoints are protected with JWT authentication, the appropriate headers need to be injected into the requests made to them. This class is supposed to take care of this.
    /// </summary>
    public interface IControllerTestingAuthTokenInjector
    {
        void AddAuthHeadersToClient();
    }

    public class ControllerTestingAuthTokenInjector : IControllerTestingAuthTokenInjector
    {
        protected readonly IntegrationTestFixture _fixture;
        protected readonly User _dummyUser = new User()
        {
            Email = "loggeduser@email.com",
            NormalizedEmail = "loggeduser@email.com",
            UserName = "loggeduser",
            NormalizedUserName = "loggeduser",
            EmailConfirmed = true,
        };

        public ControllerTestingAuthTokenInjector(IntegrationTestFixture fixture) 
        {
            _fixture = fixture;
        }

        /// <summary>
        /// Creates a dummy user with the goal of using it to retrieve a valid access token, so the HttpClient is authorized to use the protected endpoints of FlashMEMO.
        /// </summary>
        public void AddAuthHeadersToClient()
        {
            using (var scope = _fixture.Host.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<FlashMEMOContext>();
                if (dbContext.Find<User>(_dummyUser.Id) == null) dbContext.Add(_dummyUser);
                dbContext.SaveChanges();
            }

            // Declares a dummy JWTService and creates a token using it
            var jwtService = new JWTService(_fixture.Host.Services.GetService<IOptions<JWTServiceOptions>>()); // For some fucking reason, I can't simply retrieve the JWTService from the fixture...
            var accessToken = jwtService.CreateAccessToken(_dummyUser);

            _fixture.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", accessToken);
        }
    }
    #endregion

}
