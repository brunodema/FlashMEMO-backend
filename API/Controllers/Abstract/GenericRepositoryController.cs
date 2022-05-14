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
using Business.Tools.Exceptions;
using API.Controllers.Messages;

namespace API.Controllers.Messages
{
    public static class ErrorMessages
    {
        public static readonly string NoObjectAssociatedWithId = "The provided Id does not correspond to a valid object within the FlashMEMO database.";
    }
}

namespace API.Controllers.Abstract
{
    public class GenericRepositoryControllerDefaults
    {
        public const int DefaultPageSize = 10;
        public const int DefaultPageNumber = 1;
    }

    public abstract class GenericRepositoryController<TEntity, TKey, TDTO, TFilterOptions, TSortOptions> : ControllerBase
        where TEntity : class, IDatabaseItem<TKey>, new()
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
        public virtual IActionResult List(int pageSize = GenericRepositoryControllerDefaults.DefaultPageSize, int pageNumber = GenericRepositoryControllerDefaults.DefaultPageNumber, [FromQuery] TSortOptions sortOptions = null)
        {
            var data = _repositoryService.ListAsync(sortOptions);
            return Ok(new PaginatedListResponse<TEntity> { Status = "Success", Data = PaginatedList<TEntity>.Create(data, pageNumber, pageSize) });
        }

        [HttpGet]
        [Route("{id}")]
        public async virtual Task<IActionResult> Get(TKey id)
        {
            try
            {
                var ret = await _repositoryService.GetbyIdAsync(id);
                if (ret == null) return BadRequest(new BaseResponseModel { Status = "Bad Request", Message = $"Object was not able to be retrieved from the database.", Errors = new List<string>() { ErrorMessages.NoObjectAssociatedWithId } });

                return Ok(new DataResponseModel<TEntity> { Status = "Success", Message = $"Object retrieved successfully.", Data = ret });
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected async Task<TKey> AttemptEntityCreation(TDTO entityDTO)
        {
            var entity = new TEntity();
            entityDTO.PassValuesToEntity(entity);

            var validationResult = _repositoryService.CheckIfEntityIsValid(entity);
            bool idAlreadyExists = await _repositoryService.IdAlreadyExists(entity.DbId);

            if (validationResult.IsValid && !idAlreadyExists)
            {
                return await _repositoryService.CreateAsync(entity);
            }
            var errors = validationResult.Errors;
            if (idAlreadyExists)
            {
                errors.Add("The provided ID points to an already existing object.");
            }

            throw new EntityValidationException() { ServiceValidationErrors = errors };
        }

        [HttpPost]
        [Route("create")]
        public async virtual Task<IActionResult> Create(TDTO entityDTO)
        {
            try
            {
                var brandNewId = await AttemptEntityCreation(entityDTO);
                if (brandNewId != null) return Ok(new DataResponseModel<TKey> { Status = "Success", Message = $"object created successfully.", Data = brandNewId });
                return BadRequest(new BaseResponseModel { Status = "Bad Request", Message = $"Object was not able to be created within the database." });
            }
            catch (EntityValidationException ex)
            {
                return BadRequest(new BaseResponseModel { Status = "Bad Request", Message = $"Validation errors occured when creating object.", Errors = ex.ServiceValidationErrors });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut]
        [Route("{id}")]
        public async virtual Task<IActionResult> Update(TKey id, TDTO entityDTO)
        {
            var entity = new TEntity();
            entityDTO.PassValuesToEntity(entity);

            var validationResult = _repositoryService.CheckIfEntityIsValid(entity);
            if (validationResult.IsValid)
            {
                // apparently I have to do this, otherwise errors such as 'Database operation expected to affect 1 row(s) but actually affected 0 row(s)' end up happening. Plus, this seems to be the standard used by EF Core (1. get object, 2. change it, 3. update at database)
                var entityFromDb = await _repositoryService.GetbyIdAsync(id);

                if (entityFromDb == null) return BadRequest(new BaseResponseModel { Status = "Bad Request", Message = $"No valid object was retrieved from the database with the provided Id. Please check if it is correct." });

                entityDTO.PassValuesToEntity(entityFromDb);

                var changedEntityId = await _repositoryService.UpdateAsync(entityFromDb);
                return Ok(new DataResponseModel<TKey> { Status = "Success", Message = $"Object updated successfully.", Data = changedEntityId });
            }
            return BadRequest(new BaseResponseModel { Status = "Bad Request", Message = $"Validation errors occured when updating object.", Errors = validationResult.Errors });
        }

        [HttpPost]
        [Route("delete")]
        public async virtual Task<IActionResult> Delete([FromBody]TKey id)
        {
            try
            {
                var ret = await _repositoryService.RemoveByIdAsync(id);

                // This comparison is JANKY AS FUCK, but it works. Why do I use it? Because C# is overly complicated and doesn't allow me to set 'TKey' as either 'class' or 'struct' easily. 
                if (ret.ToString() == default(TKey).ToString()) return BadRequest(new BaseResponseModel { Status = "Bad Request", Message = $"Object was not able to be removed from the database.", Errors = new List<string>() { ErrorMessages.NoObjectAssociatedWithId } });

                return Ok(new BaseResponseModel { Status = "Success", Message = $"Object deleted successfully." });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("search")]
        public virtual IActionResult Search([FromQuery] TFilterOptions filterOptions, [FromQuery] TSortOptions sortOptions = null, int pageSize = GenericRepositoryControllerDefaults.DefaultPageSize, int pageNumber = GenericRepositoryControllerDefaults.DefaultPageNumber)
        {
            var data = _repositoryService.SearchAndOrder(filterOptions, sortOptions);
            data = filterOptions.GetFilteredResults(data.AsQueryable());

            return Ok(new PaginatedListResponse<TEntity> { Status = "Success", Data = PaginatedList<TEntity>.Create(data, pageNumber, pageSize) });
        }
    }
}
