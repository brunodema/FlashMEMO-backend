using Data.Repository.Interfaces;
using System.Collections.Generic;
using Data.Models.Interfaces;
using Data.Models;
using API.Tools;

namespace API.ViewModels
{
    public class ListNewsResponseModel : BaseResponseModel
    {
        public PaginatedList<News> News { get; set; }
    }
}
