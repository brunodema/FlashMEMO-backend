using Data.Interfaces;
using Data.Models;
using System.Collections.Generic;

namespace API.ViewModels
{
    public class ListNewsResponseModel : BaseResponseModel
    {
        public IEnumerable<INews> News { get; set; }
    }
}
