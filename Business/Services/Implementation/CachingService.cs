using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
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

        public CachingService(IDistributedCache cache, IOptions<CachingOptions> options)
        {
            _cache = cache;
            _options = options.Value;
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var byteData = await _cache.GetAsync(key);
            if (byteData != null)
            {
               return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(byteData));
            }

            return default(T);
        }

        public Task SetAsync<T>(string key, T value)
        {
            var byteData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value));
            return _cache.SetAsync(key, byteData);
        }

        public Task RefreshAsync(string key)
        {
            return _cache.RefreshAsync(key);
        }

        public Task RemoveAsync(string key)
        {
            return _cache.RemoveAsync(key);
        }
    }
}
