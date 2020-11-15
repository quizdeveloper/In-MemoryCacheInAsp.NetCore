using Microsoft.Extensions.Caching.Memory;
using StaticCacheHeader.Bsl.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace StaticCacheHeader.Bsl.Caching
{
    public class CacheMemoryHelper : ICacheBase
    {
        private bool IsEnableCache = false;
        private IMemoryCache _cache;

        public CacheMemoryHelper(IMemoryCache cache)
        {
            this._cache = cache;
            this.IsEnableCache = AppSettings.Instance.Get<bool>("AppConfig:EnableCache");
        }

        public void Add<T>(T o, string key)
        {
            if (IsEnableCache)
            {
                T cacheEntry;

                // Look for cache key.
                if (!_cache.TryGetValue(key, out cacheEntry))
                {
                    // Key not in cache, so get data.
                    cacheEntry = o;

                    // Set cache options.
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        // Keep in cache for this time, reset time if accessed.
                        .SetSlidingExpiration(TimeSpan.FromSeconds(7200));

                    // Save data in cache.
                    _cache.Set(key, cacheEntry, cacheEntryOptions);
                }
            }
        }

        public T Get<T>(string key)
        {
            return _cache.Get<T>(key);
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }
    }
}
