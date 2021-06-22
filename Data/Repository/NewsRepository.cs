using Data.Models;
using Data.Tools;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Data.Context;

namespace Data.Repository
{
    public class NewsRepository : BaseRepository<News, Guid, FlashMEMOContext>
    {
        public NewsRepository(FlashMEMOContext context) : base(context) { }

        public async Task<IEnumerable<News>> SearchAndOrderByCreationDateAsync(Expression<Func<News, bool>> predicate, SortType sortType, int numRecords)
        {
            return await base.SearchAndOrderAsync(predicate, new NewsSortOptions(sortType, NewsSortOptions.ColumnOptions.DATE), numRecords);
        }
    }
}
