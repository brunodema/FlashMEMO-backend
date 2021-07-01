using System.Collections.Generic;

namespace Business.Tools
{
    public class ValidatonResult
    {
        public bool IsValid { get; set; } = false;
        public List<string> Errors { get; set; } = null;
    }
}