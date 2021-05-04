using API.ViewModels;
using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;

        public NewsController(INewsService newsService) : base()
        {
            _newsService = newsService;
        }

        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> List([FromQuery] int pageSize, string filter)
        {
            var news = await _newsService.GetNewsAsync(pageSize, filter);
            return Ok(new ListNewsResponseModel { Status = "Sucess", News = news });
        }
    }
}
