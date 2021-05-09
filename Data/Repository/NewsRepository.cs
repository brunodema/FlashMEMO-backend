﻿using Data.Interfaces;
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

        public async Task<IEnumerable<News>> SearchAndOrderByCreationDateAsync(Expression<Func<News, bool>> predicate, SortType sortType, int numRecords)
        {
            return await base.SearchAndOrderAsync(predicate, sortType, news => news.CreationDate, numRecords);
        }
        public async Task<IEnumerable<News>> GetAndOrderByCreationDateAsync(SortType sortType, int numRecords)
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
