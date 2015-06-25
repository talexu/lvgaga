using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Configuration;

namespace LvService.Common
{
    public class LvConfiguration
    {
        static readonly NameValueCollection AppSettings = WebConfigurationManager.AppSettings;

        private const string DefaultTokenExpireOffset = "60";
        public static readonly double TokenExpireOffset = double.Parse(AppSettings["TokenExpireOffset"] ?? DefaultTokenExpireOffset);

        private const string DefaultCacheExpireOffset = "30";
        public static readonly double CacheExpireOffset = new[] { TokenExpireOffset, double.Parse(AppSettings["CacheExpireOffset"] ?? DefaultCacheExpireOffset) }.Min();

        private const string DefaultAsyncCacheExpireOffset = "30";
        public static readonly double AsyncCacheExpireOffset = new[] { CacheExpireOffset, double.Parse(AppSettings["AsyncCacheExpireOffset"] ?? DefaultAsyncCacheExpireOffset) }.Min();

        private const string DefaultTakingCount = "20";
        public static readonly int TakingCount = int.Parse(AppSettings["TakingCount"] ?? DefaultTakingCount);

        public static readonly string DefaultHostName = AppSettings["DefaultHostName"] ?? "www.qingyulu.com";

        public static DateTimeOffset GetExpireTime(double offset)
        {
            return DateTimeOffset.UtcNow.AddMinutes(offset);
        }
    }
}