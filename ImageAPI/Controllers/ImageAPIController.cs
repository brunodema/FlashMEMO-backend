using API.Tools;
using API.ViewModels;
using Data.Tools.Implementation;
using Data.Tools.Interfaces;
using ImageAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ImageAPI.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ImageAPIController<TFilterOptions, TSortOptions> : ControllerBase
        where TFilterOptions : IQueryFilterOptions<ImageResult>
        where TSortOptions : GenericSortOptions<ImageResult>

    {
        private readonly ILogger<ImageAPIController<TFilterOptions, TSortOptions>> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly string _baseURL;
        private readonly string _engineID;
        private readonly string _token;
        private readonly string _fullEndpoint;

        public ImageAPIController(ILogger<ImageAPIController<TFilterOptions, TSortOptions>> logger, IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;

            _baseURL = configuration.GetValue<string>("BaseURL");
            _engineID = configuration.GetValue<string>("EngineID");
            _token = configuration.GetValue<string>("Token");

            _fullEndpoint = $"{_baseURL}?key={_token}&cx={_engineID}&q=";
        }

        [HttpGet(Name = "search")]
        public async Task<IActionResult> Get(string searchText, int? pageSize, int? pageNumber)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var httpResponseMessage = await httpClient.GetAsync($"{_fullEndpoint}&q={searchText}");

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                using var contentStream =
                    await httpResponseMessage.Content.ReadAsStreamAsync();
            }

            return Ok(new PaginatedListResponse<ImageResult> { Status = "Sucess", Data = PaginatedList<ImageResult>.Create(null, pageNumber ?? 1, pageSize ?? 10) });
        }
    }
}