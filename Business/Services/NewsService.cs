using Business.Interfaces;
using Data.Interfaces;
using Data.Models;
using Data.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
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
        public async Task<ICollection<News>> GetNewsAsync(int pageSize, string filter)
        {
            return await _repository.GetAllAsync();
        }
    }
}