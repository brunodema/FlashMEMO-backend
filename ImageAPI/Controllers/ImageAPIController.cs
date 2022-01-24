using API.Tools;
using API.ViewModels;
using Data.Tools.Implementation;
using Data.Tools.Interfaces;
using ImageAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ImageAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageAPIController<TFilterOptions, TSortOptions> : ControllerBase
        where TFilterOptions : IQueryFilterOptions<ImageResult>
        where TSortOptions : GenericSortOptions<ImageResult>

    {
        private readonly ILogger<ImageAPIController<TFilterOptions, TSortOptions>> _logger;

        public ImageAPIController(ILogger<ImageAPIController<TFilterOptions, TSortOptions>> logger)
        {
            _logger = logger;
        }

        //[HttpGet(Name = "search")]
        //public IActionResult Get(int pageSize, int? pageNumber, [FromQuery] TFilterOptions filterOptions, [FromQuery] TSortOptions sortOptions = null)
        //{
        //    return Ok(new PaginatedListResponse<ImageResult> { Status = "Sucess", Data = PaginatedList<ImageResult>.Create(data, pageNumber ?? 1, pageSize) });
        //}
    }
}