using Data.Tools;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Data.Context;
using Data.Tools.Implementation;
using Data.Repository.Abstract;
using Data.Models.Implementation;

namespace Data.Repository.Implementation
{
    public class NewsRepository : GenericRepository<News, Guid, FlashMEMOContext>
    {
        public NewsRepository(FlashMEMOContext context) : base(context) { }

        public IEnumerable<News> SearchAndOrderByCreationDateAsync(Expression<Func<News, bool>> predicate, SortType sortType, int numRecords)
        {
            return base.SearchAndOrderAsync(predicate, new NewsSortOptions(sortType, NewsSortOptions.ColumnOptions.DATE), numRecords);
        }
    }
}
