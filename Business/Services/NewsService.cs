using Business.Interfaces;
using Data.Interfaces;
using Data.Models;
using Data.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<IEnumerable<News>> GetNewsAsync(int pageSize = 0, string filter = null, SortType sortType = SortType.Ascending)
        {
            IEnumerable<News> news;
            if (filter == null) // empty
            {
                news = await _repository.GetAllAndOrderByCreationDateAsync(sortType);
            }
            else
            {
                news = await _repository.FindAllAndOrderByCreationDateAsync(o => o.Title.Contains(filter) || o.Subtitle.Contains(filter) || o.Content.Contains(filter), sortType); // must make this case insensitive
            }
            if (pageSize > _options.MaxPageSize || pageSize <= 0) pageSize = _options.MaxPageSize;

            return news.Take(pageSize);
        }
    }
}