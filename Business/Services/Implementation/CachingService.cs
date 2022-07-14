using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Implementation
{
    public interface ICachingService
    {
        Task<byte[]> GetAsync(string key);
        Task SetAsync(string key, byte[] value);
        Task RefreshAsync(string key);
        Task Remove(string key);
    }

    public class CachingOptions // Leave this class for now, but it might be useless depending on what can be configured on 'Startup'.
    {
        public string CachingURL { get; set; }
    }

    public class CachingService
    {
        private readonly IDistributedCache _cache;
        private readonly CachingOptions _options;

        public CachingService(IDistributedCache cache, IOptions<CachingOptions> options)
        {
            _cache = cache;
            _options = options.Value;
        }

        public Task<byte[]> GetAsync(string key)
        {
            return _cache.GetAsync(key);
        }

        public Task SetAsync(string key, byte[] value)
        {
            return _cache.SetAsync(key, value);

        }

        public Task RefreshAsync(string key)
        {
            return _cache.RefreshAsync(key);

        }

        public Task Remove(string key)
        {
            return _cache.RemoveAsync(key);
        }
    }
}
