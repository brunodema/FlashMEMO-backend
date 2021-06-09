using Business.Tools;
using Data.Context;
using Data.Repository.Interfaces;
using Data.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Interfaces
{
    public interface IBaseRepositoryService<TEntity>
        where TEntity : class
    {
        public Task CreateAsync(TEntity entity, object[] auxParams = null); // to cover the 'CreateUserAsync' case (requires password)
        public Task UpdateAsync(TEntity entity);
        public Task<TEntity> GetbyIdAsync(Guid id);
        public Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate, SortOptions<TEntity, object> sortOptions, int numRecords = 1000);
        public Task RemoveByIdAsync(Guid guid);
        public ValidatonResult CheckIfEntityIsValid(TEntity entity);
    }
}
