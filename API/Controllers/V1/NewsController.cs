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
        public async Task<IActionResult> List()
        {
            var news = await _newsService.GetNewsAsync(1, "");
            return Ok(new ListNewsResponseModel { Status = "Sucess", News = news });
        }
    }
}
