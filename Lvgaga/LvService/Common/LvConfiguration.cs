using System;
using System.Linq;

namespace LvService.Common
{
    public class LvConfiguration
    {
        private const double DefaultTokenExpireOffset = 1;
        public static readonly double TokenExpireOffset = DefaultTokenExpireOffset;

        private const double DefaultCacheExpireOffset = 30;
        public static readonly double CacheExpireOffset = new[] { TokenExpireOffset, DefaultCacheExpireOffset }.Min();

        private const double DefaultAsyncCacheExpireOffset = 30;
        public static readonly double AsyncCacheExpireOffset = new[] { CacheExpireOffset, DefaultAsyncCacheExpireOffset }.Min();

        public const int DefaultTakingCount = 20;

        public static DateTimeOffset GetExpireTime(double offset)
        {
            return DateTimeOffset.UtcNow.AddMinutes(offset);
        }
    }
}