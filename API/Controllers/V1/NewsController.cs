using API.Controllers.Implementations;
using API.Tools;
using API.ViewModels;
using Business.Services;
using Data.Models;
using Data.Tools;
using Data.Tools.Implementations;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class NewsController : GenericRepositoryController<News, Guid>
    {
        private readonly NewsService _newsService;

        public NewsController(NewsService newsService) : base(newsService)
        {
            _newsService = newsService;
        }
        protected override GenericSortOptions<News> SetColumnSorting(SortType sortType, string columnToSort)
        {
            return new NewsSortOptions(sortType, columnToSort);
        }

        [HttpGet]
        [Route("search")]
        public virtual IActionResult Search([FromQuery] string columnToSort, SortType sortType, int pageSize, int? pageNumber, [FromQuery] NewsFilterOptions filterOptions)
        {
            var sortOptions = SetColumnSorting(sortType, columnToSort);

            var data = _newsService.SearchAndOrder(filterOptions, sortOptions);
            data = filterOptions.GetFilteredResults(data.AsQueryable());

            return Ok(new PaginatedListResponse<News> { Status = "Sucess", Data = PaginatedList<News>.Create(data, pageNumber ?? 1, pageSize) });
        }
    }
}
