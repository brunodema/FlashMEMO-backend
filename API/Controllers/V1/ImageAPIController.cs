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
using System;
using System.Collections.Generic;

namespace API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ImageAPIController : ControllerBase

    {
        private readonly IImageAPIServiceOptions _configuration;

        private bool IsSearchTextValid(string searchText)
        {
            return !String.IsNullOrEmpty(searchText);
        }

        public ImageAPIController(IOptions<ImageAPIServiceOptions> configuration)
        {
            _configuration = configuration.Value;
        }

        [HttpGet]
        [Route("search")]
        public async Task<IActionResult> Search(string searchText, int pageSize = 10, int pageNumber = 1)
        {
            try
            {
                using (var service = new CustomSearchAPIService(new BaseClientService.Initializer { ApplicationName = "FlashMEMO Image Search", ApiKey = _configuration.Token }))
                {
                    ListRequest listRequest = service.Cse.List();

                    if(!IsSearchTextValid(searchText))
                    {
                        return BadRequest(new BaseResponseModel { Status = "Bad Request", Errors = new List<string>() { "The search text used is not valid." }, Message = "The search text used is not valid." });
                    }

                    listRequest.Q = searchText;
                    listRequest.SearchType = ListRequest.SearchTypeEnum.Image;
                    listRequest.Cx = _configuration.EngineID;

                    if (pageSize < 0)
                    {
                        return BadRequest(new BaseResponseModel { Status = "Bad Request", Errors = new List<string>() { "The page size has an invalid number (less than 0)." }, Message = "The page size has an invalid number (less than 0)." });
                    }
                    if (pageNumber < 0)
                    {
                        return BadRequest(new BaseResponseModel { Status = "Bad Request", Errors = new List<string>() { "The page size has an invalid number (less than 0)." }, Message = "The page size has an invalid number (less than 0)." });
                    }
                    listRequest.Start = (pageSize * pageNumber) - pageSize;
                    listRequest.Num = pageSize;

                    var results = await listRequest.ExecuteAsync();

                    var response = new PaginatedListResponse<Result>
                    {
                        Status = "Success",
                        Data = new PaginatedList<Result>()
                        {
                            Results = results.Items ?? new List<Result>() { },
                            ResultSize = results?.Items?.Count ?? 0,
                            PageIndex = pageNumber,
                            TotalAmount = Convert.ToUInt32(results?.SearchInformation.TotalResults ?? "0"),
                            TotalPages = (int)Math.Ceiling(Convert.ToUInt32(results?.SearchInformation.TotalResults ?? "0") / (double)pageSize)

                        }
                    };

                    return Ok(response);
                }
            }
            catch (Exception)
            {
                return StatusCode(500);
                throw;
            }
        }
    }
}