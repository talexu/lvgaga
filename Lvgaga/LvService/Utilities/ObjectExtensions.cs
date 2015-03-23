using Newtonsoft.Json;

namespace LvService.Utilities
{
    public static class ObjectExtensions
    {
        public static string ToJsonString(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}