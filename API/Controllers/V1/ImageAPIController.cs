using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Business.Services.Implementation;
using Business.Tools;
using API.ViewModels;
using System;
using API.Tools;
using System.Linq;

namespace API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ImageAPIController : ControllerBase

    {
        private readonly CustomSearchAPIService _service;

        public ImageAPIController(CustomSearchAPIService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("search")]
        public async Task<IActionResult> Search(string searchText, int pageSize = 10, int pageNumber = 1)
        {
            try
            {
                var results = await _service.Search(searchText, pageSize, pageNumber);

                return Ok(new PaginatedListResponse<CustomSearchAPIImageResult>
                {
                    Status = "Success",
                    Message = "API results successfully retrieved.",
                    Data = new PaginatedList<CustomSearchAPIImageResult>()
                    {
                        Results = results.Results.ToList(),
                        ResultSize = results.ResultSize,
                        PageIndex = results.PageIndex,
                        TotalAmount = results.TotalAmount,
                        TotalPages = results.TotalPages,
                    }
                });
            }
            catch (InputValidationException e)
            {
                return BadRequest(new BaseResponseModel { Status = "Bad Request", Message = e.Message, Errors = e.InputValidationErrors});
                throw;
            }
            catch (Exception)
            {
                return StatusCode(500);
                throw;
            }
        }
    }
}