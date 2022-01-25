using API.Tools;
using API.ViewModels;
using Business.Services.Interfaces;
using Google.Apis.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Google.Apis.CustomSearchAPI.v1;
using static Google.Apis.CustomSearchAPI.v1.CseResource;
using Google.Apis.CustomSearchAPI.v1.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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

        private List<string> IsInputValid(string searchText, int pageSize, int pageNumber)
        {
            var errorMessages = new List<string>();

            if (!IsSearchTextValid(searchText))
            {
                errorMessages.Add("The search text used is not valid.");
            }
            if (pageSize <= 0 || pageSize > 10) // the GT part is a restriction of the Google API itself
            {
                errorMessages.Add("The page size has an invalid number (less or equal than 0, or greater than 10).");
            }
            if (pageNumber <= 0)
            {
                errorMessages.Add("The page size has an invalid number (less or equal than 0).");
            }

            return errorMessages;
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

                    var validationsMessages = IsInputValid(searchText, pageSize, pageNumber);
                    if (validationsMessages.Count > 0)
                    {
                        return BadRequest(new BaseResponseModel() { Status = "Bad Request", Message = "Query params have validation problems.", Errors = validationsMessages });
                    } 

                    listRequest.Q = searchText;
                    listRequest.SearchType = ListRequest.SearchTypeEnum.Image;
                    listRequest.Cx = _configuration.EngineID;
                    listRequest.Start = (pageSize * pageNumber) - pageSize;
                    listRequest.Num = pageSize;
                    var results = await listRequest.ExecuteAsync();

                    var totalAmount = Convert.ToUInt64(results?.SearchInformation.TotalResults ?? "0");
                    var response = new PaginatedListResponse<ImageAPIResponseViewModel>
                    {
                        Status = "Success",
                        Data = new PaginatedList<ImageAPIResponseViewModel>()
                        {
                            Results = results.Items.Select(i => new ImageAPIResponseViewModel(i.Title, i.Image, i.Link)).ToList(),
                            ResultSize = results?.Items?.Count ?? 0,
                            PageIndex = Convert.ToUInt64(pageNumber),
                            TotalAmount = totalAmount,
                            TotalPages = Convert.ToUInt64(Math.Ceiling(totalAmount / (double)pageSize))

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