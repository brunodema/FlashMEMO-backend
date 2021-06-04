using Business.Tools.Interfaces;

namespace Business.Tools
{
    public class Credentials : ICredentials
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
}
