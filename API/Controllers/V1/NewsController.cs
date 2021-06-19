﻿using API.Controllers.Implementations;
using Business.Services;
using Data.Models;
using Data.Tools;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq.Expressions;

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
            GenericSortOptions<News> sortOptions = new NewsSortOptions(sortType, columnToSort);
            return sortOptions;
        }

        protected override Expression<Func<News, bool>> SetFiltering(string searchString)
        {
            return searchString == null ? _ => true : news => news.Content.Contains(searchString) || news.Title.Contains(searchString) || news.Subtitle.Contains(searchString);
        }
    }
}
