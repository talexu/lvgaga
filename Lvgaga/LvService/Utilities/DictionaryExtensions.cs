using System.Collections.Generic;

namespace LvService.Utilities
{
    public static class DictionaryExtensions
    {
        public static bool AddOrUpdateValue<TK, TV>(this IDictionary<TK, TV> dic, TK key, TV value)
        {
            if (dic == null) return false;

            var contains = dic.ContainsKey(key);
            if (contains)
            {
                dic[key] = value;
            }
            else
            {
                dic.Add(key, value);
            }

            return !contains;
        }
    }
}