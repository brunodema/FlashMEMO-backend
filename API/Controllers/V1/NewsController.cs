﻿using API.Tools;
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
    public class NewsController : ControllerBase
    {
        private readonly NewsService _newsService;

        public NewsController(NewsService newsService) : base()
        {
            _newsService = newsService;
        }

        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> List([FromQuery] string columnToSort, SortType sortType, string currentFilter, string searchString, int pageSize, int? pageNumber)
        {
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

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
            Expression <Func<News,bool>> predicate = searchString == null ? _ => true : news => news.Content.Contains(searchString) || news.Title.Contains(searchString) || news.Subtitle.Contains(searchString);

            var news = await _newsService.GetAsync(predicate, sortOptions, 1000);
            return Ok(new ListNewsResponseModel { Status = "Sucess", News = PaginatedList<News>.CreateAsync(news, pageNumber ?? 1, pageSize)});
        }
    }
}
