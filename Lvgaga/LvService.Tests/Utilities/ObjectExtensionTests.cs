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
            var t = new TumblrText()
            {
                Text = "Hello World!",
                Category = TumblrCategory.C1
            };
            var p = t.CloneByJson();

            Assert.NotSame(t, p);
            Assert.Equal(t.Text, p.Text);
            Assert.Equal(t.Category, p.Category);
        }
    }
}