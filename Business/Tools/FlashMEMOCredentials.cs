using Business.Tools.Interfaces;

namespace Business.Tools
{
    public class FlashMEMOCredentials : IFlashMEMOCredentials
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
}
