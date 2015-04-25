using System;
using LvModel.View.Tumblr;
using LvService.Utilities;
using Xunit;

namespace LvService.Tests.Utilities
{
    public class ObjectExtensionTests
    {
        [Fact]
        public void CopyByJson_Return_True_WhenCopy()
        {
            var t = new TumblrText
            {
                Text = "Hello World!",
                Category = TumblrCategory.C1
            };
            var p = t.CloneByJson();

            Assert.NotSame(t, p);
            Assert.Equal(t.Text, p.Text);
            Assert.Equal(t.Category, p.Category);
        }

        [Theory]
        [InlineData(new[] { "123", "123" }, true)]
        [InlineData(new[] { 1, 1 }, true)]
        [InlineData(new[] { true, true }, true)]
        [InlineData(null, true)]
        [InlineData(new String[] { }, true)]
        [InlineData(new[] { true, false }, false)]
        [InlineData(new[] { 1, 2 }, false)]
        [InlineData(new[] { "123", "456" }, false)]
        [InlineData(new[] { "123", null }, false)]
        public void AllEqual_Return_CorrectValue(object[] objs, bool allEqual)
        {
            Assert.Equal(allEqual, objs.AllEqual());
        }
    }
}