using Business.Interfaces;
using Data.Interfaces;
using Data.Models;
using Data.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Business.Services
{
    public class NewsServiceOptions : INewsServiceOptions
    {
        public int MaxPageSize { get; set; } = 50;
        public int PageSize { get; set; } = 10;
        public string Filter { get; set; } = "";
    }
    public class NewsService : INewsService
    {
        private readonly INewsServiceOptions _options;
        private readonly NewsRepository _repository;

        public NewsService(IOptions<NewsServiceOptions> options, NewsRepository repository)
        {
            _options = options.Value;
            _repository = repository;
        }
        public async Task<IEnumerable<News>> GetNewsAsync(int numRecords = 0, string filter = null, SortType sortType = SortType.Ascending)
        {
            Expression<Func<News, bool>> func;
            if (filter == null) func = _ => true;
            else func = (news) => news.Title.Contains(filter) || news.Subtitle.Contains(filter) || news.Content.Contains(filter);

            if (numRecords > _options.MaxPageSize || numRecords <= 0) numRecords = _options.MaxPageSize;

            return await _repository.FindAndOrderByCreationDateAsync(func, numRecords, sortType); // must make this case insensitive
        }
    }
}