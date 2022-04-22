using API.Tools;
using API.ViewModels;
using Business.Services.Interfaces;
using Data.Repository.Interfaces;
using Data.Tools.Sorting;
using Data.Tools.Filtering;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Models.DTOs;
using System;
using Data.Tools.Exceptions.Repository;

namespace API.Controllers.Abstract
{
    public class GenericRepositoryControllerDefaults
    {
        public const int DefaultPageSize = 10;
        public const int DefaultPageIndex = 1;
    }

    public abstract class GenericRepositoryController<TEntity, TKey, TDTO, TFilterOptions, TSortOptions> : ControllerBase
        where TEntity : class, IDatabaseItem<TKey>
        where TDTO : IModelDTO<TEntity, TKey>
        where TFilterOptions : IQueryFilterOptions<TEntity>
        where TSortOptions : GenericSortOptions<TEntity>
    {
        private readonly IRepositoryService<TEntity, TKey> _repositoryService;

        protected GenericRepositoryController(IRepositoryService<TEntity, TKey> repositoryService)
        {
            _repositoryService = repositoryService;
        }

        [HttpGet]
        [Route("list")]
        public virtual IActionResult List(int pageSize = GenericRepositoryControllerDefaults.DefaultPageSize, int pageNumber = GenericRepositoryControllerDefaults.DefaultPageIndex, [FromQuery] TSortOptions sortOptions = null)
        {
            var data = _repositoryService.ListAsync(sortOptions);
            return Ok(new PaginatedListResponse<TEntity> { Status = "Success", Data = PaginatedList<TEntity>.Create(data, pageNumber, pageSize) });
        }

        [HttpGet]
        [Route("{id}")]
        public async virtual Task<IActionResult> Get(TKey id)
        {
            var data = await _repositoryService.GetbyIdAsync(id);
            return Ok(new DataResponseModel<TEntity> { Status = "Success", Data = data });
        }

        [HttpPost]
        [Route("create")]
        public async virtual Task<IActionResult> Create(TDTO entityDTO)
        {
            var entity = entityDTO.CreateFromDTO();

            var validationResult = _repositoryService.CheckIfEntityIsValid(entity);
            bool idAlreadyExists = await _repositoryService.IdAlreadyExists(entity.DbId);

            if (validationResult.IsValid && !idAlreadyExists)
            {
                var brandNewId = await _repositoryService.CreateAsync(entity);
                return Ok(new DataResponseModel<TKey> { Status = "Success", Message = $"{entity.GetType().Name} created successfully.", Data = brandNewId });
            }
            var errors = validationResult.Errors;
            if (idAlreadyExists)
            {
                errors.Add("The provided ID points to an already existing object.");
            }
            return BadRequest(new BaseResponseModel { Status = "Error", Message = $"Validation errors occured when creating {entity.GetType().Name}.", Errors = errors });
        }

        [HttpPut]
        [Route("update/{id}")]
        public async virtual Task<IActionResult> Update(TKey id, TDTO entityDTO)
        {
            var entity = entityDTO.CreateFromDTO();
            entity.DbId = id;

            var validationResult = _repositoryService.CheckIfEntityIsValid(entity);
            if (validationResult.IsValid)
            {
                var changedEntityId = await _repositoryService.UpdateAsync(entity);
                return Ok(new DataResponseModel<TKey> { Status = "Success", Message = $"{entity.GetType().Name} updated successfully.", Data = changedEntityId });
            }
            return BadRequest(new BaseResponseModel { Status = "Error", Message = $"Validation errors occured when updating {entity.GetType().Name}.", Errors = validationResult.Errors });
        }

        [HttpPost]
        [Route("delete")]
        public async virtual Task<IActionResult> Delete([FromBody] TKey id)
        {
            try
            {
                var ret = await _repositoryService.RemoveByIdAsync(id);
                return Ok(new BaseResponseModel { Status = "Success", Message = $"Object deleted successfully." });
            }
            catch (ObjectNotFoundWithId<TKey> ex)
            {
                return NotFound(new BaseResponseModel { Status = "Not Found", Message = "Object was not deleted. Please make sure that the Id provided is valid.", Errors = new List<string>() { ex.Message } });
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("search")]
        public virtual IActionResult Search([FromQuery] TFilterOptions filterOptions, [FromQuery] TSortOptions sortOptions = null, int pageSize = GenericRepositoryControllerDefaults.DefaultPageSize, int pageNumber = GenericRepositoryControllerDefaults.DefaultPageIndex)
        {
            var data = _repositoryService.SearchAndOrder(filterOptions, sortOptions);
            data = filterOptions.GetFilteredResults(data.AsQueryable());

            return Ok(new PaginatedListResponse<TEntity> { Status = "Success", Data = PaginatedList<TEntity>.Create(data, pageNumber, pageSize) });
        }
    }
}
