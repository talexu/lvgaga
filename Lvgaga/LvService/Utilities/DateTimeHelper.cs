using System;

namespace LvService.Utilities
{
    public class DateTimeHelper
    {
        public static string GetInvertedTicks(DateTime utcDateTime)
        {
            return string.Format("{0:D19}", DateTime.MaxValue.Ticks - utcDateTime.Ticks);
        }

        public static string GetInvertedTicksNow()
        {
            return GetInvertedTicks(DateTime.UtcNow);
        }

        public static DateTime GetDateTimeFromInvertedTicks(string invertedTicks)
        {
            return new DateTime(DateTime.MaxValue.Ticks - Int64.Parse(invertedTicks));
        }
    }
}