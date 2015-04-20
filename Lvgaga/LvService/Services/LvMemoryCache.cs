using System;
using System.Runtime.Caching;
using System.Threading.Tasks;
using LvService.Common;

namespace LvService.Services
{
    public class LvMemoryCache : ICache
    {
        private readonly ObjectCache _cache = MemoryCache.Default;
        private const string RegionOfAsync = "async";

        public T Get<T>(string key)
        {
            try
            {
                return (T)_cache.Get(key);
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

            #region Cache Task

            var asyncKey = String.Format("{0} {1}", RegionOfAsync, key);
            Task<T> cachedTask, newTask = null;
            lock (_cache)
            {
                cachedTask = _cache.Get(asyncKey) as Task<T>;
                if (cachedTask == null)
                {
                    newTask = func();
                    _cache.Set(asyncKey, newTask,
                        LvConfiguration.GetExpireTime(LvConfiguration.AsyncCacheExpireOffset));
                }
            }
            value = cachedTask != null
                ? await cachedTask.ContinueWith(r => r.Result)
                : newTask != null ? await newTask : await func();

            #endregion

            //value = await func();
            Set(key, value);
            return value;
        }

        public void Set(string key, object value)
        {
            Set(key, value, LvConfiguration.GetExpireTime(LvConfiguration.CacheExpireOffset));
        }

        public void Set(string key, object value, DateTimeOffset absoluteExpiration)
        {
            _cache.Set(key, value, absoluteExpiration);
        }
    }
}