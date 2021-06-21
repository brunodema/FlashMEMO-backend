using API.Controllers.Implementations;
using API.Tools;
using API.Tools.Interfaces;
using API.ViewModels;
using Business.Services;
using Data.Models;
using Data.Tools;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class NewsController : RepositoryController<News, Guid>
    {
        private readonly NewsService _newsService;

        public NewsController(NewsService newsService) : base(newsService)
        {
            _newsService = newsService;
        }
        protected override GenericSortOptions<News> SetColumnSorting(string columnToSort, SortType sortType)
        {
            return new NewsSortOptions(sortType, columnToSort);
        }

        [HttpGet]
        [Route("search")]
        public async virtual Task<IActionResult> Search([FromQuery] string columnToSort, SortType sortType, int pageSize, int? pageNumber, [FromQuery] NewsFilterOptions filterOptions)
        {
            var sortOptions = new NewsSortOptions(sortType, columnToSort);

            var data = await _newsService.GetAsync(_ => true, sortOptions);
            data = filterOptions.GetFilteredResults(data.AsQueryable());

            return Ok(new PaginatedListResponse<News> { Status = "Sucess", Data = PaginatedList<News>.CreateAsync(data, pageNumber ?? 1, pageSize) });
        }
    }
}
