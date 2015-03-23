using System.Collections.Generic;

namespace LvService.Utilities
{
    public static class DictionaryExtension
    {
        public static bool TryAddValue<TK, TV>(this IDictionary<TK, TV> dic, TK key, TV value)
        {
            if (dic == null) return false;
            if (dic.ContainsKey(key))
            {
                dic[key] = value;
                return true;
            }
            dic.Add(new KeyValuePair<TK, TV>(key, value));
            return true;
        }
    }
}