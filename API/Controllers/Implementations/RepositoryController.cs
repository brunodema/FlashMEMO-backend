using API.Tools;
using API.ViewModels;
using Business.Services.Interfaces;
using Data.Repository.Interfaces;
using Data.Tools;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace API.Controllers.Implementations
{
    public abstract class RepositoryController<TEntity, TKey> : ControllerBase
        where TEntity : class, IDatabaseItem<TKey>
    {
        private readonly IBaseRepositoryService<TEntity, TKey> _repositoryService;

        protected RepositoryController(IBaseRepositoryService<TEntity, TKey> repositoryService)
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
        [Route("{id}")]
        public async virtual Task<IActionResult> Get(TKey id)
        {
            var data = await _repositoryService.GetbyIdAsync(id);
            return Ok(new PaginatedListResponse<TEntity> { Status = "Sucess", Data = PaginatedList<TEntity>.CreateAsync(new List<TEntity> { data }, 1, 1) });
        }

        [HttpPost]
        [Route("create")]
        public async virtual Task<IActionResult> Create(TEntity entity)
        {
            var validationResult = _repositoryService.CheckIfEntityIsValid(entity);
            bool idAlreadyExists = await _repositoryService.IdAlreadyExists(entity.GetId());
            if (validationResult.IsValid && !idAlreadyExists)
            {
                await _repositoryService.CreateAsync(entity);
                return Ok(new BaseResponseModel { Status = "Success", Message = $"{entity.GetType().Name} created successfully." });
            }
            var errors = validationResult.Errors;
            if (idAlreadyExists)
            {
                errors.Add("The provided ID points to an already existing object.");
            }
            return BadRequest(new BaseResponseModel { Status = "Error", Message = $"Validation errors occured when creating {entity.GetType().Name}.", Errors = errors });
        }

        [HttpPut]
        [Route("update")]
        public async virtual Task<IActionResult> Update(TEntity entity)
        {
            var validationResult = _repositoryService.CheckIfEntityIsValid(entity);
            if (validationResult.IsValid)
            {
                await _repositoryService.UpdateAsync(entity);
                return Ok(new BaseResponseModel { Status = "Success", Message = $"{entity.GetType().Name} updated successfully." });
            }
            return BadRequest(new BaseResponseModel { Status = "Error", Message = $"Validation errors occured when updating {entity.GetType().Name}.", Errors = validationResult.Errors });
        }

        [HttpPost]
        [Route("delete")]
        public async virtual Task<IActionResult> Delete([FromBody] TKey id)
        {
            await _repositoryService.RemoveByIdAsync(id);
            return Ok(new BaseResponseModel { Status = "Success", Message = $"Object deleted successfully." });
        }
    }
}
