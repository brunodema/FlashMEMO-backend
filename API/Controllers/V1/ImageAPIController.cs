using API.Tools;
using API.ViewModels;
using Business.Services.Interfaces;
using Google.Apis.Services;
using ImageAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading.Tasks;

using Google.Apis.CustomSearchAPI.v1;
using static Google.Apis.CustomSearchAPI.v1.CseResource;
using Google.Apis.CustomSearchAPI.v1.Data;

namespace API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ImageAPIController : ControllerBase

    {
        private readonly ILogger<ImageAPIController> _logger;
        private readonly IImageAPIServiceOptions _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly string _fullEndpoint;

        public ImageAPIController(ILogger<ImageAPIController> logger, IOptions<ImageAPIServiceOptions> configuration, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _configuration = configuration.Value;
            _httpClientFactory = httpClientFactory;

            _fullEndpoint = $"{_configuration.BaseURL}?key={_configuration.Token}&cx={_configuration.EngineID}";
        }

        [HttpGet]
        [Route("search")]
        public async Task<IActionResult> Search(string searchText, int? pageSize, int? pageNumber)
        {
            var service = new CustomSearchAPIService(new BaseClientService.Initializer
            {
                ApplicationName = "Discovery Sample",
                ApiKey = _configuration.Token,
            });
            ListRequest listRequest = service.Cse.List();
            listRequest.Q = searchText;
            listRequest.SearchType = ListRequest.SearchTypeEnum.Image;
            listRequest.Cx = _configuration.EngineID;

            var results = await listRequest.ExecuteAsync();

            return Ok(new PaginatedListResponse<Result> { Status = "Sucess", Data = PaginatedList<Result>.Create(results.Items, pageNumber ?? 1, pageSize ?? 10) });

        }
    }
}