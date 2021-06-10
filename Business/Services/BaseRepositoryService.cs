using Data.Context;
using Data.Tools;
using Data.Models;
using Data.Repository;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Business.Tools;
using Business.Services.Interfaces;

namespace Business.Services
{
    public abstract class BaseRepositoryService<TRepositoryType, TKey, TEntity> : IBaseRepositoryService<TEntity, TKey>
        where TRepositoryType : BaseRepository<TEntity, TKey, FlashMEMOContext>
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
            if (!validationResult.IsValid)
            {
                throw new ServiceValidationException { Errors = validationResult.Errors };
            }
            await _baseRepository.CreateAsync(entity);
        }
        public async virtual Task UpdateAsync(TEntity entity)
        {
            await _baseRepository.UpdateAsync(entity);
        }
        public async virtual Task<TEntity> GetbyIdAsync(TKey id)
        {
            return await _baseRepository.GetByIdAsync(id);
        }
        public async virtual Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate = null, SortOptions<TEntity, object> sortOptions = null, int numRecords = 1000)
        {
            return await _baseRepository.SearchAndOrderAsync(predicate, sortOptions, numRecords);
        }
        public async virtual Task RemoveByIdAsync(TKey guid)
        {
            await _baseRepository.RemoveByIdAsync(guid);
        }
        public abstract ValidatonResult CheckIfEntityIsValid(TEntity entity);
    }
    public class BaseRepositoryServiceOptions
    {
        public int MaxPageSize { get; set; } = 50;
        public int PageSize { get; set; } = 10;
    }

    public class NewsService : BaseRepositoryService<NewsRepository, Guid, News>
    {
        public NewsService(NewsRepository baseRepository, IOptions<BaseRepositoryServiceOptions> serviceOptions) : base(baseRepository, serviceOptions.Value) { }
        public override ValidatonResult CheckIfEntityIsValid(News entity)
        {
            // return new ValidatonResult { IsValid = entity.CreationDate <= entity.LastUpdated }; // dummy database needs to be updated in order to take this into account... let's just return true for now
            return new ValidatonResult { IsValid = true };
        }
    }
}