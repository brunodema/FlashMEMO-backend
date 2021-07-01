using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Business.Tools;
using Business.Services.Interfaces;
using Data.Repository.Interfaces;
using Data.Tools.Interfaces;
using Data.Tools.Implementation;
using Data.Repository.Abstract;

namespace Business.Services.Abstract
{
    public class GenericRepositoryServiceOptions
    {
        public int MaxPageSize { get; set; } = 50;
        public int PageSize { get; set; } = 10;
    }

    public abstract class GenericRepositoryService<TRepositoryType, TKey, TEntity> : IRepositoryService<TEntity, TKey>
        where TRepositoryType : GenericRepository<TEntity, TKey, FlashMEMOContext>
        where TEntity : class, IDatabaseItem<TKey>
    {
        private readonly TRepositoryType _baseRepository;
        private readonly GenericRepositoryServiceOptions _serviceOptions;
        public GenericRepositoryService(TRepositoryType baseRepository, GenericRepositoryServiceOptions serviceOptions)
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
        public virtual IEnumerable<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate = null, GenericSortOptions<TEntity> sortOptions = null, int numRecords = 1000)
        {
            return _baseRepository.SearchAndOrderAsync(predicate, sortOptions, numRecords);
        }
        public async virtual Task RemoveByIdAsync(TKey guid)
        {
            await _baseRepository.RemoveByIdAsync(guid);
        }
        public abstract ValidatonResult CheckIfEntityIsValid(TEntity entity);

        public async virtual Task<bool> IdAlreadyExists(TKey id)
        {
            return await GetbyIdAsync(id) != null;
        }

        public IEnumerable<TEntity> SearchAndOrder(IQueryFilterOptions<TEntity> filterOptions, GenericSortOptions<TEntity> sortOptions)
        {
            return _baseRepository.SearchAndOrder(filterOptions, sortOptions);
        }
    }
}