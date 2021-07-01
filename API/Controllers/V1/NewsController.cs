using API.Controllers.Abstract;
using Business.Services.Implementation;
using Data.Models.Implementation;
using Data.Tools;
using Microsoft.AspNetCore.Mvc;
using System;

namespace API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class NewsController : GenericRepositoryController<News, Guid, NewsFilterOptions, NewsSortOptions>
    {
        private readonly NewsService _newsService;

        public NewsController(NewsService newsService) : base(newsService)
        {
            _newsService = newsService;
        }
    }
}
