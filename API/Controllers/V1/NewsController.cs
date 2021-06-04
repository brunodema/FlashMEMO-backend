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
        public async Task<IActionResult> List([FromQuery] int pageSize, string filter, SortType sortType)
        {
            Expression<Func<News,bool>> predicate = filter == null ? _ => true : news => news.Content.Contains(filter) || news.Title.Contains(filter) || news.Subtitle.Contains(filter);
            var news = await _newsService.GetAsync<int>(predicate, null, pageSize);;
            return Ok(new ListNewsResponseModel { Status = "Sucess", News = news });
        }
    }
}
