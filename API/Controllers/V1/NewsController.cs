using API.Controllers.Implementations;
using Business.Services;
using Data.Models;
using Data.Tools;
using Data.Tools.Implementations;
using Microsoft.AspNetCore.Mvc;
using System;

namespace API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class NewsController : GenericRepositoryController<News, Guid, NewsFilterOptions>
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
    }
}
