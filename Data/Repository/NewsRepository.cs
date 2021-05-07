using Data.Interfaces;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class NewsRepository : BaseRepository<News, FlashMEMOContext>
    {
        public NewsRepository(FlashMEMOContext context) : base(context) { }

        public async Task<IEnumerable<News>> FindAllAndOrderByCreationDateAsync(Expression<Func<News, bool>> predicate, SortType sortType)
        {
            var response = await base.FindAllAsync(predicate);
            if (sortType == SortType.Ascending)
            {
                return response.OrderBy(news => news.CreationDate);
            }
            return response.OrderByDescending(news => news.CreationDate);
        }
        public async Task<IEnumerable<News>> GetAllAndOrderByCreationDateAsync(SortType sortType)
        {
            var response = await base.GetAllAsync();
            if (sortType == SortType.Ascending)
            {
                return response.OrderBy(news => news.CreationDate);
            }
            return response.OrderByDescending(news => news.CreationDate);
        }
    }
}
