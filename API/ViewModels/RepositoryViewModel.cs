using API.Tools;

namespace API.ViewModels
{
    public class PaginatedListResponse<TEntity> : BaseResponseModel
        where TEntity : class
    {
        public PaginatedList<TEntity> Data { get; set; }
    }
}
