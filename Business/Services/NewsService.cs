using Business.Tools;
using Data.Models;
using Data.Repository;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace Business.Services
{
    public class NewsService : GenericRepositoryService<NewsRepository, Guid, News>
    {
        public NewsService(NewsRepository baseRepository, IOptions<GenericRepositoryServiceOptions> serviceOptions) : base(baseRepository, serviceOptions.Value) { }
        public override ValidatonResult CheckIfEntityIsValid(News entity)
        {
            bool areDatesValid = entity.CreationDate <= entity.LastUpdated;

            List<string> errors = new();
            if (!areDatesValid)
            {
                errors.Add("The last updated date must be more recent than the creation date.");
            }

            return new ValidatonResult 
            {
                IsValid = areDatesValid,
                Errors = errors
            };
        }
    }
}
