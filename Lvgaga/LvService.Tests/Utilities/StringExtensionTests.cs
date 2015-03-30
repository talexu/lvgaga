using System.Linq;
using LvModel.View.Tumblr;
using LvService.Utilities;
using Xunit;

namespace LvService.Tests.Utilities
{
    public class StringExtensionTests : IClassFixture<TestFilesLoader>
    {
        private readonly TestFilesLoader _fixture;
        private readonly string[] _testDataFolder = { "Utilities", "StringExtensionTestData" };

        public StringExtensionTests(TestFilesLoader fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [InlineData("123_456", new[] { "123", "456" })]
        public void SplitByUnderline_Return_CorrectStrings(string str, string[] strs)
        {
            Assert.True(strs.SequenceEqual(str.SplitByUnderline()));
        }

        [Theory]
        [InlineData("123_456", 0, "123")]
        [InlineData("123_456", 1, "456")]
        public void SubstringByUnderline_Return_CorrectString(string str, int index, string strp)
        {
            Assert.Equal(strp, str.SubstringByUnderline(index));
        }

        [Theory]
        [InlineData("Hello World!", "Hello World!")]
        [InlineData("", "")]
        public void EqualWithCosine_Return_True_WhenCompareTwoEqualStrings(string str1, string str2)
        {
            Assert.True(str1.CosineEqual(str2));
        }

        [Theory]
        [InlineData("Hello World!", "Hello World?")]
        [InlineData("", "!")]
        [InlineData("!", "")]
        [InlineData("", null)]
        [InlineData(null, "")]
        [InlineData("Hello World!", null)]
        [InlineData(null, "Hello World!")]
        public void EqualWithCosine_Return_False_WhenCompareTwoDifferentStrings(string str1, string str2)
        {
            Assert.False(str1.CosineEqual(str2));
        }

        [Theory]
        [InlineData("json_o.json", "json_o.json")]
        [InlineData("json_o.json", "json_c.json")]
        [InlineData("json_o.json", "json_e.json")]
        [InlineData("json_e.json", "json_c.json")]
        public void EqualWithCosine_Return_True_WhenCompareTwoEqualJsons(string file1, string file2)
        {
            _fixture.InitializeFolder(_testDataFolder);
            Assert.True(_fixture.ReadAllText(file1).CosineEqual(_fixture.ReadAllText(file2)));
        }

        [Theory]
        [InlineData("json_o.json", "json_d1.json")]
        [InlineData("json_o.json", "json_d2.json")]
        [InlineData("json_o.json", "json_d3.json")]
        [InlineData("json_e.json", "json_d1.json")]
        [InlineData("json_e.json", "json_d2.json")]
        [InlineData("json_e.json", "json_d3.json")]
        public void EqualWithCosine_Return_False_WhenCompareTwoDifferentJsons(string file1, string file2)
        {
            _fixture.InitializeFolder(_testDataFolder);
            Assert.False(_fixture.ReadAllText(file1).CosineEqual(_fixture.ReadAllText(file2)));
        }

        [Fact]
        public void ToStream_ToString_Return_True_WhenInvokeTwice()
        {
            const string str = "Test string";
            Assert.Equal(str, str.ToMemoryStream().ToStringFromMemoryStream());
        }

        [Theory]
        [InlineData("All", TumblrCategory.All)]
        [InlineData(0, TumblrCategory.All)]
        [InlineData("0", TumblrCategory.All)]
        [InlineData("C1", TumblrCategory.C1)]
        [InlineData(1, TumblrCategory.C1)]
        [InlineData("1", TumblrCategory.C1)]
        public void ToEnum_Return_CorrectTumblrCategory(string str, TumblrCategory category)
        {
            Assert.Equal(category, str.ToEnum<TumblrCategory>());
        }
    }
}