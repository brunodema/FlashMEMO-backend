namespace Business.Tools
{
    public class ValidatonResult
    {
        public bool IsValid { get; set; } = false;
        public string[] Errors { get; set; } = { };
    }
}