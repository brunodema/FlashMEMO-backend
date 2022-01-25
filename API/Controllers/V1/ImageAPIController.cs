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
        private readonly IImageAPIServiceOptions _configuration;

        public ImageAPIController(IOptions<ImageAPIServiceOptions> configuration)
        {
            _configuration = configuration.Value;
        }

        [HttpGet]
        [Route("search")]
        public async Task<IActionResult> Search(string searchText, int? pageSize, int? pageNumber)
        {
            var service = new CustomSearchAPIService(new BaseClientService.Initializer
            {
                ApplicationName = "FlashMEMO Image Search",
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