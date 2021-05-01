using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IAuthServiceOptions
    {
    }
    public interface IAuthService
    {
        public Task<bool> AreCredentialsValidAsync(ICredentials credentials);
    }
}
