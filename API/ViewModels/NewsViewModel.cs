using Data.Repository.Interfaces;
using System.Collections.Generic;
using Data.Models.Interfaces;

namespace API.ViewModels
{
    public class ListNewsResponseModel : BaseResponseModel
    {
        public IEnumerable<INews> News { get; set; }
    }
}
