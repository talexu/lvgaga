using System;
using System.Linq;

namespace LvService.Common
{
    public class LvConfiguration
    {
        private const double DefaultTokenExpireOffset = 0.5;
        public static readonly double TokenExpireOffset = DefaultTokenExpireOffset;

        private const double DefaultCacheExpireOffset = 30;
        public static readonly double CacheExpireOffset = new[] { TokenExpireOffset, TokenExpireOffset }.Min();

        private const double DefaultAsyncCacheExpireOffset = 30;
        public static readonly double AsyncCacheExpireOffset = new[] { CacheExpireOffset }.Min();

        public static DateTimeOffset GetExpireTime(double offset)
        {
            return DateTimeOffset.UtcNow.AddMinutes(offset);
        }
    }
}