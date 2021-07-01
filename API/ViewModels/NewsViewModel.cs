using API.Tools;
using Data.Models.Implementation;

namespace API.ViewModels
{
    public class ListNewsResponseModel : BaseResponseModel
    {
        public PaginatedList<News> News { get; set; }
    }
}
