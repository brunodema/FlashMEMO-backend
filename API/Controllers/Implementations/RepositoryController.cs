using API.ViewModels;
using Business.Services;
using Business.Services.Interfaces;
using Business.Tools;
using Data.Context;
using Data.Models;
using Data.Repository;
using Data.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers.Implementations
{
    public abstract class RepositoryController<TEntity> : ControllerBase
        where TEntity : class
    {
        private readonly IBaseRepositoryService<TEntity> _repositoryService;
        private readonly IDictionary<string, object> _columnSort;

        protected RepositoryController(IBaseRepositoryService<TEntity> repositoryService)
        {
            _repositoryService = repositoryService;
        }
        [HttpGet]
        [Route("list")]
        public abstract Task<IActionResult> Get([FromQuery] string columnToSort, SortType sortType, string searchString, int pageSize, int? pageNumber);

        [HttpPost]
        [Route("create")]
        public async virtual Task<IActionResult> Create(TEntity entity)
        {
            var validationResult = _repositoryService.CheckIfEntityIsValid(entity);
            if (validationResult.IsValid)
            {
                await _repositoryService.CreateAsync(entity);
                return Ok(new BaseResponseModel { Status = "Success", Message = $"{entity.ToString()} created successfully." });
            }
            return BadRequest(new BaseResponseModel { Status = "Error", Message = $"Validation occured when creating {entity.ToString()}.", Errors = validationResult.Errors });
        }

        [HttpPut]
        [Route("update")]
        public async virtual Task<IActionResult> Update(TEntity entity)
        {
            var validationResult = _repositoryService.CheckIfEntityIsValid(entity);
            if (validationResult.IsValid)
            {
                await _repositoryService.UpdateAsync(entity);
                return Ok(new BaseResponseModel { Status = "Success", Message = $"{entity.ToString()} updated successfully." });
            }
            return BadRequest(new BaseResponseModel { Status = "Error", Message = $"Validation occured when updating {entity.ToString()}.", Errors = validationResult.Errors });
        }

        [HttpPost]
        [Route("delete")]
        public async virtual Task<IActionResult> Delete(TEntity entity)
        {
            await _repositoryService.UpdateAsync(entity);
            return Ok(new BaseResponseModel { Status = "Success", Message = $"{entity.ToString()} deleted successfully." });
        }
    }
}
