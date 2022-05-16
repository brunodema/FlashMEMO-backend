using Business.Services.Interfaces;
using Business.Tools;
using Business.Tools.Exceptions;
using Data.Context;
using Data.Repository.Abstract;
using Data.Repository.Interfaces;
using Data.Tools.Sorting;
using Data.Tools.Filtering;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

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
        protected readonly TRepositoryType _baseRepository;
        private readonly GenericRepositoryServiceOptions _serviceOptions;
        public GenericRepositoryService(TRepositoryType baseRepository, GenericRepositoryServiceOptions serviceOptions)
        {
            _baseRepository = baseRepository;
            _serviceOptions = serviceOptions;
        }
        public async virtual Task<TKey> CreateAsync(TEntity entity, object[] auxParams = null) // to cover the 'CreateUserAsync' case (requires password)
        {
            var validationResult = CheckIfEntityIsValid(entity);
            if (!validationResult.IsValid)
            {
                throw new EntityValidationException { ServiceValidationErrors = validationResult.Errors };
            }
            return await _baseRepository.CreateAsync(entity);
        }
        public async virtual Task<TKey> UpdateAsync(TEntity entity)
        {
            return await _baseRepository.UpdateAsync(entity);
        }
        public async virtual Task<TEntity> GetbyIdAsync(TKey id)
        {
            return await _baseRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// A 'List' method in FlashMEMO means that no filtering is applied to the queried, only sorting (i.e., the search predicate returns 'true' for all). Used to fetch all entries for a given entity.
        /// </summary>
        /// <param name="sortOptions">Class containing the sort definitions for the specific entity associated with the service.</param>
        /// <param name="numRecords">Maximum number of records to retrieve.</param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> List(GenericSortOptions<TEntity> sortOptions = null, int numRecords = 1000)
        {
            return _baseRepository.SearchAndOrder(_ => true, sortOptions, numRecords);
        }
        public async virtual Task<TKey> RemoveByIdAsync(TKey guid)
        {
            return await _baseRepository.RemoveByIdAsync(guid);
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