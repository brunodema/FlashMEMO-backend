using Data.Models;
using System.Collections.Generic;

namespace API.ViewModels
{
    public class ListNewsResponseModel : BaseResponseModel
    {
        public ICollection<News> News { get; set; }
    }
}
