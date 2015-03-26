using System.Dynamic;
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
    }
}