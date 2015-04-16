using System;
using System.Threading.Tasks;

namespace LvService.Services
{
    public interface ICache
    {
        T Get<T>(string key);
        Task<T> Get<T>(string key, Func<Task<T>> func);

        void Set(string key, Object value);
        void Set(string key, Object value, DateTimeOffset absoluteExpiration);
    }
}