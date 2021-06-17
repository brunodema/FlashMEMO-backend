using Business.Tools;
using Data.Models;
using Data.Repository;
using Microsoft.Extensions.Options;
using System;

namespace Business.Services
{
    public class NewsService : BaseRepositoryService<NewsRepository, Guid, News>
    {
        public NewsService(NewsRepository baseRepository, IOptions<BaseRepositoryServiceOptions> serviceOptions) : base(baseRepository, serviceOptions.Value) { }
        public override ValidatonResult CheckIfEntityIsValid(News entity)
        {
            var areDatesValid = entity.CreationDate <= entity.LastUpdated;
            var errors = areDatesValid ? null : new string[] { "The last updated date must be more recent than the creation date." };
            return new ValidatonResult 
            {
                IsValid = areDatesValid,
                Errors = errors
            };
        }
    }
}
