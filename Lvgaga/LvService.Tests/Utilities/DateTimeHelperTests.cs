using System;
using LvService.Utilities;
using Xunit;

namespace LvService.Tests.Utilities
{
    public class DateTimeHelperTests
    {
        [Fact]
        public void InvertedTicks_Return_Equal_InvertTwice()
        {
            var now = DateTime.UtcNow;
            var inverted = DateTimeHelper.GetInvertedTicks(now);
            var rNow = DateTimeHelper.GetDateTimeFromInvertedTicks(inverted);
            Assert.Equal(now, rNow);
        }
    }
}