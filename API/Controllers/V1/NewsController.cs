using API.Controllers.Implementations;
using API.Tools;
using API.ViewModels;
using Business.Services;
using Data.Models;
using Data.Tools;
using Microsoft.AspNetCore.Mvc;
using System;
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
        protected override SortOptions<News, object> SetColumnSorting(string columnToSort, SortType sortType)
        {
            SortOptions<News, object> sortOptions = new SortOptions<News, object>();
            switch (columnToSort)
            {
                case "subtitle":
                    sortOptions.ColumnToSort = news => news.Subtitle;
                    break;
                case "date":
                    sortOptions.ColumnToSort = news => news.CreationDate;
                    break;
                default: // default will be title
                    sortOptions.ColumnToSort = news => news.Title;
                    break;
            }
            sortOptions.SortType = sortType;
            return sortOptions;
        }

        protected override Expression<Func<News, bool>> SetFiltering(string searchString)
        {
            return searchString == null ? _ => true : news => news.Content.Contains(searchString) || news.Title.Contains(searchString) || news.Subtitle.Contains(searchString);
        }
    }
}
