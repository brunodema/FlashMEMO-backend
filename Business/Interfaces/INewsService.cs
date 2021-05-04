﻿using Data.Interfaces;
using Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface INewsServiceOptions
    {
        public int MaxPageSize { get; set; }
        public int PageSize { get; set; }
        public string Filter { get; set; }
    }
    public interface INewsService
    {
        public Task<ICollection<News>> GetNewsAsync(int pageSize, string filter);
    }
}
