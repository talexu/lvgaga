using System;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace LvService.Services
{
    public class LvMemoryCache : ICache
    {
        readonly ObjectCache _cache = MemoryCache.Default;

        public T Get<T>(string key)
        {
            try
            {
                return (T) _cache.Get(key);
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        public async Task<T> Get<T>(string key, Func<Task<T>> func)
        {
            var value = Get<T>(key);
            if (value != null) return value;

            value = await func();
            Set(key, value);
            return value;
        }

        public void Set(string key, object value)
        {
            Set(key, value, DateTimeOffset.UtcNow.AddHours(1));
        }

        public void Set(string key, object value, DateTimeOffset absoluteExpiration)
        {
            _cache.Set(key, value, absoluteExpiration);
        }
    }
}