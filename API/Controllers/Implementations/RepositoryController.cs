using API.Tools;
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
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace API.Controllers.Implementations
{
    public abstract class RepositoryController<TEntity> : ControllerBase
        where TEntity : class
    {
        private readonly IBaseRepositoryService<TEntity> _repositoryService;

        protected RepositoryController(IBaseRepositoryService<TEntity> repositoryService)
        {
            _repositoryService = repositoryService;
        }
        protected abstract SortOptions<TEntity, object> SetColumnSorting(string columnToSort, SortType sortType);
        protected abstract Expression<Func<TEntity, bool>> SetFiltering(string searchString);

        [HttpGet]
        [Route("list")]
        public async virtual Task<IActionResult> List([FromQuery] string columnToSort, SortType sortType, string searchString, int pageSize, int? pageNumber)
        {
            var sortOptions = SetColumnSorting(columnToSort, sortType);
            var predicate = SetFiltering(searchString);

            var data = await _repositoryService.GetAsync(predicate, sortOptions);
            return Ok(new PaginatedListResponse<TEntity> { Status = "Sucess", Data = PaginatedList<TEntity>.CreateAsync(data, pageNumber ?? 1, pageSize) });
        }

        [HttpGet]
        [Route("get")]
        public async virtual Task<IActionResult> Get(Guid guid)
        {
            var data = await _repositoryService.GetbyIdAsync(guid);
            return Ok(new PaginatedListResponse<TEntity> { Status = "Sucess", Data = PaginatedList<TEntity>.CreateAsync(new List<TEntity> { data }, 1, 1) });
        }

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
            return BadRequest(new BaseResponseModel { Status = "Error", Message = $"Validation errors occured when creating {entity.ToString()}.", Errors = validationResult.Errors });
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
            return BadRequest(new BaseResponseModel { Status = "Error", Message = $"Validation errors occured when updating {entity.ToString()}.", Errors = validationResult.Errors });
        }

        [HttpPost]
        [Route("delete")]
        public async virtual Task<IActionResult> Delete(TEntity entity)
        {
            await _repositoryService.RemoveAsync(entity);
            return Ok(new BaseResponseModel { Status = "Success", Message = $"{entity.ToString()} deleted successfully." });
        }
    }
}
