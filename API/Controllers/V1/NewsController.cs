using API.Controllers.Implementations;
using API.Tools;
using API.ViewModels;
using Business.Services;
using Data.Models;
using Data.Tools;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
    public interface IQueryFilterOptions<TEntiy> where TEntiy : class
    {
        public IQueryable<TEntiy> GetFilteredResults(IQueryable<News> elements);
    }
    public class NewsFilterOptions : IQueryFilterOptions<News>
    {
        public DateTime? FromDate { get; set; } = null;
        public DateTime? ToDate { get; set; } = null;
        public string Title { get; set; } = null;
        public string Subtitle { get; set; } = null;
        public string Content { get; set; } = null;

        public IQueryable<News> GetFilteredResults(IQueryable<News> elements)
        {
            var processedFromDate = FromDate ?? DateTime.MinValue;
            var processedToDate = ToDate ?? DateTime.MaxValue;
            elements = elements.Where(x => x.CreationDate >= processedFromDate && x.CreationDate <= processedToDate);

            if (Title != null)
            {
                elements = elements.Where(x => x.Title.Contains(Title));
            }
            if (Subtitle != null)
            {
                elements = elements.Where(x => x.Subtitle.Contains(Subtitle));
            }
            if (Content != null)
            {
                elements = elements.Where(x => x.Content.Contains(Content));
            }

            return elements;
        }
    }
}
