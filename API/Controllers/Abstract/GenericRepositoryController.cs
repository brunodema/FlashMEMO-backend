using API.Tools;
using API.ViewModels;
using Business.Services.Interfaces;
using Data.Repository.Interfaces;
using Data.Tools.Implementation;
using Data.Tools.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers.Abstract
{
    public abstract class GenericRepositoryController<TEntity, TKey, TFilterOptions, TSortOptions> : ControllerBase
        where TEntity : class, IDatabaseItem<TKey>
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
        public virtual IActionResult List(int pageSize, int? pageNumber, [FromQuery] TSortOptions sortOptions = null)
        {
            var data = _repositoryService.GetAsync(_ => true, sortOptions);
            return Ok(new PaginatedListResponse<TEntity> { Status = "Sucess", Data = PaginatedList<TEntity>.Create(data, pageNumber ?? 1, pageSize) });
        }

        [HttpGet]
        [Route("{id}")]
        public async virtual Task<IActionResult> Get(TKey id)
        {
            var data = await _repositoryService.GetbyIdAsync(id);
            return Ok(new PaginatedListResponse<TEntity> { Status = "Sucess", Data = PaginatedList<TEntity>.Create(new List<TEntity> { data }, 1, 1) });
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

        [HttpGet]
        [Route("search")]
        public virtual IActionResult Search(int pageSize, int? pageNumber, [FromQuery] TFilterOptions filterOptions, [FromQuery] TSortOptions sortOptions = null)
        {
            var data = _repositoryService.SearchAndOrder(filterOptions, sortOptions);
            data = filterOptions.GetFilteredResults(data.AsQueryable());

            return Ok(new PaginatedListResponse<TEntity> { Status = "Sucess", Data = PaginatedList<TEntity>.Create(data, pageNumber ?? 1, pageSize) });
        }
    }
}
