using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json;

namespace LvService.Utilities
{
    public static class ObjectExtensions
    {
        public static string ToJsonString(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static ExpandoObject ToExpandoObject(this object obj)
        {
            if (obj == null) return null;

            var json = JsonConvert.SerializeObject(obj);
            var eobj = JsonConvert.DeserializeObject<ExpandoObject>(json);

            return eobj;
        }

        public static T CloneByJson<T>(this T obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static List<T> CloneByJson<T>(this IEnumerable<T> objs)
        {
            return objs.Select(obj => obj.CloneByJson()).ToList();
        }

        public static bool AllEqual<T>(this T[] objs)
        {
            var obj = objs.First();
            return objs.All(s => s != null && s.Equals(obj));
        }
    }
}