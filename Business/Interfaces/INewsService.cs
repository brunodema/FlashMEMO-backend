using Data.Tools;
using Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface INewsServiceOptions
    {
        public int MaxPageSize { get; set; }
        public int PageSize { get; set; }
        public string Filter { get; set; }
    }
    public interface INewsService
    {
        public Task<IEnumerable<News>> GetNewsAsync(int pageSize = 0, string filter = null, SortType sortType = SortType.Ascending);
    }
}
