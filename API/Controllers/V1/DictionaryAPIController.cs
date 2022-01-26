using Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;

namespace API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class DictionaryAPIController : ControllerBase

    {
        private readonly DictionaryAPIService _service;

        public DictionaryAPIController(DictionaryAPIService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("search")]
        public IActionResult Search(string searchText)
        {
            try
            {
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500);
                throw;
            }
        }
    }
}