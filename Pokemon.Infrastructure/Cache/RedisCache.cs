using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.Infrastructure.Cache
{
    public class RedisCache
    {
        private readonly IDistributedCache _distributedCache;   
        public RedisCache(IDistributedCache distributedCache ) 
        {
            _distributedCache = distributedCache;
        } 

        public T GetCachedData<T>(string key)
        {
            var jsonData = _distributedCache.GetString(key);
            if (string.IsNullOrEmpty(jsonData))
            {
                return default(T);
            }   

            return JsonConvert.DeserializeObject<T>(jsonData);
        }

        public async Task SetCachedDataAsync<T>(string key, T data, TimeSpan? absoluteExpireTime = null, TimeSpan? slidingExpireTime = null)
        {
           
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromMinutes(60),
                SlidingExpiration = slidingExpireTime ?? TimeSpan.FromMinutes(30)
            };
            var jsonData = JsonConvert.SerializeObject(data);

            await _distributedCache.SetStringAsync(key, jsonData, options);
        }

        public async Task RemoveCachedDataAsync(string key)
        {
            await _distributedCache.RemoveAsync(key);
        }   
    }
}
