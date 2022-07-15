using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Implementation
{
    public interface ICachingService
    {
        Task<T> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value);
        Task RefreshAsync(string key);
        Task RemoveAsync(string key);
    }

    public class CachingOptions // Leave this class for now, but it might be useless depending on what can be configured on 'Startup'.
    {
        public string CachingURL { get; set; }
    }

    public class CachingService : ICachingService
    {
        private readonly IDistributedCache _cache;
        private readonly CachingOptions _options;
        private readonly ILogger<CachingService> _logger;

        public CachingService(IDistributedCache cache, IOptions<CachingOptions> options, ILogger<CachingService> logger)
        {
            _cache = cache;
            _options = options.Value;
            _logger = logger;
        }

        public async Task<T> GetAsync<T>(string key)
        {
            try
            {
                var byteData = await _cache.GetAsync(key);
                if (byteData != null)
                {
                    return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(byteData));
                }

                return default(T);
            }
            catch (Exception ex)
            {
                _logger.LogError("Caching service failed to get entry.", ex);
                return default(T);
            }
        }

        public Task SetAsync<T>(string key, T value)
        {
            try
            {
                var byteData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value));
                return _cache.SetAsync(key, byteData);
            }
            catch (Exception ex)
            {
                _logger.LogError("Caching service failed to set entry.", ex);
                return Task.CompletedTask;
            }
        }

        public Task RefreshAsync(string key)
        {
            try
            {
                return _cache.RefreshAsync(key);
            }
            catch (Exception ex)
            {
                _logger.LogError("Caching service failed to refresh entry.", ex);
                return Task.CompletedTask;
            }
        }

        public Task RemoveAsync(string key)
        {
            try
            {
                return _cache.RemoveAsync(key);
            }
            catch (Exception ex)
            {
                _logger.LogError("Caching service failed to remove entry.", ex);
                return Task.CompletedTask;
            }
        }
    }
}
