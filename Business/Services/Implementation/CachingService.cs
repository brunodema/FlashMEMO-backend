using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Implementation
{
    public interface ICachingService
    {
        Task<T> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value);
        Task RefreshAsync<T>(string key);
        Task RemoveAsync(string key);
    }

    public class CachingOptions // Leave this class for now, but it might be useless depending on what can be configured on 'Startup'.
    {
        public string CachingURL { get; set; }
    }

    // I'll leave this commented for now, but the 'Multiplexer' approach seems to be the winner here... 

    //public class CachingService : ICachingService
    //{
    //    private readonly IDistributedCache _cache;
    //    private readonly CachingOptions _options;
    //    private readonly ILogger<CachingService> _logger;

    //    public CachingService(IDistributedCache cache, IOptions<CachingOptions> options, ILogger<CachingService> logger)
    //    {
    //        _cache = cache;
    //        _options = options.Value;
    //        _logger = logger;
    //    }

    //    public async Task<T> GetAsync<T>(string key)
    //    {
    //        try
    //        {
    //            var byteData = await _cache.GetAsync(key);
    //            if (byteData != null)
    //            {
    //                return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(byteData));
    //            }

    //            return default(T);
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogError("Caching service failed to get entry.", ex);
    //            return default(T);
    //        }
    //    }

    //    public Task SetAsync<T>(string key, T value)
    //    {
    //        try
    //        {
    //            var byteData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value));
    //            return _cache.SetAsync(key, byteData);
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogError("Caching service failed to set entry.", ex);
    //            return Task.CompletedTask;
    //        }
    //    }

    //    public Task RefreshAsync(string key)
    //    {
    //        try
    //        {
    //            return _cache.RefreshAsync(key);
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogError("Caching service failed to refresh entry.", ex);
    //            return Task.CompletedTask;
    //        }
    //    }

    //    public Task RemoveAsync(string key)
    //    {
    //        try
    //        {
    //            return _cache.RemoveAsync(key);
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogError("Caching service failed to remove entry.", ex);
    //            return Task.CompletedTask;
    //        }
    //    }
    //}

    public class CachingService : ICachingService
    {
        private readonly IConnectionMultiplexer _cache;
        private readonly ILogger<CachingService> _logger;

        public CachingService(IConnectionMultiplexer cache, ILogger<CachingService> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public async Task<T> GetAsync<T>(string key)
        {
            try
            {
                if (_cache.IsConnected)
                {
                    var cacheData = await _cache.GetDatabase().StringGetAsync(key);
                    if (!cacheData.IsNullOrEmpty)
                    {
                        _logger.LogDebug("Cache HIT!");
                        return JsonConvert.DeserializeObject<T>(cacheData.ToString());
                    }
                }
                else
                {
                    _logger.LogInformation($"Cache does not seem to be connected, can't get '{key}' from server.");
                }

                return default(T);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Caching service failed to get entry '{key}'.", ex);
                return default(T);
            }
        }

        public Task SetAsync<T>(string key, T value)
        {
            try
            {
                if (_cache.IsConnected)
                {
                    var cacheData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value));
                    _logger.LogDebug($"Setting cache entry '{key}'...");
                    return _cache.GetDatabase().StringSetAsync(key, cacheData);
                }
                else
                {
                    _logger.LogInformation($"Cache does not seem to be connected, can't set '{key}' on server.");
                }

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Caching service failed to set entry '{key}'.", ex);
                return Task.CompletedTask;
            }
        }

        public async Task RefreshAsync<T>(string key)
        {
            try
            {
                if (_cache.IsConnected)
                {
                    var entry = await this.GetAsync<T>(key);
                    _logger.LogDebug($"Refreshing cache entry '{key}'...");
                    await this.SetAsync(key, entry);
                }
                else
                {
                    _logger.LogInformation($"Cache does not seem to be connected, can't refresh '{key}' on server.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Caching service failed to refresh entry '{key}'.", ex);
            }
        }

        public Task RemoveAsync(string key)
        {
            try
            {
                if (_cache.IsConnected)
                {
                    _logger.LogDebug("Removing cache entry...");
                    return _cache.GetDatabase().StringGetDeleteAsync(key);
                }
                else
                {
                    _logger.LogInformation($"Cache does not seem to be connected, can't remove '{key}' from server.");
                }

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Caching service failed to remove entry '{key}'.", ex);
                return Task.CompletedTask;
            }
        }
    }
}
