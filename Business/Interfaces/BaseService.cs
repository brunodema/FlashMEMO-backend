using Data.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface BaseService<TEntity, TResultClass> 
        where TEntity : class
        where TResultClass : class // in case you need to use an additional type for the return type of certain operations (eX: 'IdentityResult')
    {
        public Task<TResultClass> CreateAsync(TEntity entity, object[] auxParams = null); // to cover the 'CreateUserAsync' case (requires password)
        public Task<TResultClass> UpdateAsync(TEntity entity);
        public Task<TEntity> GetbyIdAsync(Guid id);
        public Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate = null, SortOptions<TEntity, IKey> sortOptions = null, int numRecords = 1000);
        public Task<TResultClass> DeleteAsync(TEntity entity);
    }
}