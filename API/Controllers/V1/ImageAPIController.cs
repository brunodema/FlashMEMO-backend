using API.Tools;
using API.ViewModels;
using Business.Services.Interfaces;
using Data.Tools.Implementation;
using Data.Tools.Interfaces;
using ImageAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading.Tasks;

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
            var httpClient = _httpClientFactory.CreateClient();
            var httpResponseMessage = await httpClient.GetAsync($"{_fullEndpoint}&q={searchText}");

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                using var contentStream =
                    await httpResponseMessage.Content.ReadAsStreamAsync();

                return Ok(new PaginatedListResponse<ImageResult> { Status = "Sucess", Data = PaginatedList<ImageResult>.Create(, pageNumber ?? 1, pageSize ?? 10) });
            }
        }
    }
}