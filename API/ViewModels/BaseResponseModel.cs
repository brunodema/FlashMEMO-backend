using System.Collections.Generic;

namespace API.ViewModels
{
    public class BaseResponseModel
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
