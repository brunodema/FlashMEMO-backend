namespace Business.Tools.Interfaces
{
    public interface IFlashMEMOCredentials
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
}
