using System;

namespace LvService.Services
{
    public interface ICache
    {
        T Get<T>(string key);

        void Set(string key, Object value);
        void Set(string key, Object value, DateTimeOffset absoluteExpiration);
    }
}