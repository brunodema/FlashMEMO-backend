using Data.Context;
using Data.Tools;
using Data.Models;
using Data.Repository;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public class ValidatonResult
    {
        public bool IsValid { get; set; } = false;
        public string[] Errors { get; set; } = { };
    }
    public class ServiceValidationException : Exception
    {
        public ServiceValidationException() : base("The service could successfully validate the entity.") { }
        public string[] Errors { get; set; } = { };
    }
    public abstract class BaseRepositoryService<TRepositoryType, TEntity>
        where TRepositoryType : BaseRepository<TEntity, FlashMEMOContext>
        where TEntity : class
    {
        private readonly TRepositoryType _baseRepository;
        private readonly BaseRepositoryServiceOptions _serviceOptions;
        public BaseRepositoryService(TRepositoryType baseRepository, BaseRepositoryServiceOptions serviceOptions)
        {
            _baseRepository = baseRepository;
            _serviceOptions = serviceOptions;
        }
        public async virtual Task CreateAsync(TEntity entity, object[] auxParams = null) // to cover the 'CreateUserAsync' case (requires password)
        {
            var validationResult = CheckIfEntityIsValid(entity);
            if (validationResult.IsValid)
            {
                throw new ServiceValidationException { Errors = validationResult.Errors };
            }
            await _baseRepository.CreateAsync(entity);
        }
        public async virtual Task UpdateAsync(TEntity entity)
        {
            await _baseRepository.UpdateAsync(entity);
        }
        public async virtual Task<TEntity> GetbyIdAsync(Guid id)
        {
            return await _baseRepository.GetByIdAsync(id);
        }
        public async virtual Task<IEnumerable<TEntity>> GetAsync<TKey>(Expression<Func<TEntity, bool>> predicate = null, SortOptions<TEntity, TKey> sortOptions = null, int numRecords = 1000)
        {
            return await _baseRepository.SearchAndOrderAsync<TKey>(predicate, sortOptions, numRecords);
        }
        public async virtual Task RemoveAsync(TEntity entity)
        {
            await _baseRepository.RemoveAsync(entity);
        }
        public abstract ValidatonResult CheckIfEntityIsValid(TEntity entity);
    }
    public class BaseRepositoryServiceOptions
    {
        public int MaxPageSize { get; set; } = 50;
        public int PageSize { get; set; } = 10;
    }

    public class DummyService : BaseRepositoryService<NewsRepository, News>
    {
        public DummyService(NewsRepository baseRepository, IOptions<BaseRepositoryServiceOptions> serviceOptions) : base(baseRepository, serviceOptions.Value) { }
        public override ValidatonResult CheckIfEntityIsValid(News entity)
        {
            return new ValidatonResult { IsValid = true }; // temporary implementation
        }
    }
}