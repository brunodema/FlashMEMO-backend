using API.ViewModels;

namespace Tests.Tests.Integration.Implementation
{
    public interface IIdentityControllerTests
    {
        // Login tests
        void SuccessfulLogin(LoginRequestModel request);
        void FailedLoginWithWrongCredentials(LoginRequestModel request);
        // JWT tests
        void SuccessfulTokenRenewal(string expiredAccesstoken, string refreshToken);
        void FailedTokenRenewalWithInvalidAT(string expiredAccesstoken, string refreshToken);
        void FailedTokenRenewalWithNotExpiredAT(string expiredAccesstoken, string refreshToken);
        void FailedTokenRenewalWithInvalidRT(string expiredAccesstoken, string refreshToken);
        void FailedTokenRenewalWithExpiredRT(string expiredAccesstoken, string refreshToken);
        void FailedTokenRenewalWithUnmatchedTokens(string expiredAccesstoken, string refreshToken);
    }

    public abstract class IdentityControllerTests : IIdentityControllerTests
    {
        // Login tests
        public void SuccessfulLogin(LoginRequestModel request)
        {
            throw new System.NotImplementedException();
        }

        public void FailedLoginWithWrongCredentials(LoginRequestModel request)
        {
            throw new System.NotImplementedException();
        }

        // JWT tests
        public void SuccessfulTokenRenewal(string expiredAccesstoken, string refreshToken)
        {
            throw new System.NotImplementedException();
        }

        public void FailedTokenRenewalWithInvalidAT(string expiredAccesstoken, string refreshToken)
        {
            throw new System.NotImplementedException();
        }

        public void FailedTokenRenewalWithNotExpiredAT(string expiredAccesstoken, string refreshToken)
        {
            throw new System.NotImplementedException();
        }

        public void FailedTokenRenewalWithInvalidRT(string expiredAccesstoken, string refreshToken)
        {
            throw new System.NotImplementedException();
        }

        public void FailedTokenRenewalWithExpiredRT(string expiredAccesstoken, string refreshToken)
        {
            throw new System.NotImplementedException();
        }

        public void FailedTokenRenewalWithUnmatchedTokens(string expiredAccesstoken, string refreshToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
