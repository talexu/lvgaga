using System.IO;
using LvModel.Common;
using LvModel.View.Tumblr;
using LvService.Factories.Uri;
using Xunit;

namespace LvService.Tests.Factories
{
    public class UriFactoryTests : IClassFixture<UriFactoryFixture>
    {
        private readonly UriFactoryFixture _fixture;

        public UriFactoryTests(UriFactoryFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [InlineData("comments", "/comments")]
        [InlineData("comments/1", "/comments/1")]
        [InlineData("", "/")]
        public void CreateUri_Return_CorrectUri(string path, string expected)
        {
            Assert.Equal(expected, _fixture.UriFactory.CreateUri(path));
        }

        [Fact]
        public void CreateUri_Return_CorrectUri_PathCombine()
        {
            Assert.Equal("/comments/1",
                _fixture.UriFactory.CreateUri(Path.Combine("comments", "1")));
        }

        [Theory]
        [InlineData("0_123", TumblrCategory.All, "123")]
        [InlineData("All_123", TumblrCategory.All, "123")]
        [InlineData("1_123", TumblrCategory.C1, "123")]
        public void ParseTumblrRowKey_Return_CorrectTumblrCategoryAndInvertedTicks(string rowKey,
            TumblrCategory category, string invertedTicks)
        {
            //dynamic r = _fixture.UriFactory.ParseTumblrRowKey(rowKey);
            //Assert.Equal(category, r.TumblrCategory);
            Assert.Equal(invertedTicks, _fixture.UriFactory.CreateInvertedTicksByTumblrRowKey(rowKey));
        }

        [Theory]
        [InlineData("123_0", "123", MediaType.All)]
        [InlineData("123_All", "123", MediaType.All)]
        [InlineData("123_1", "123", MediaType.Image)]
        public void ParseCommentPartitionKey_Return_CorrectInvertedTicksAndMediaType(string partitionKey,
            string invertedTicks, MediaType mediaType)
        {
            //dynamic r = _fixture.UriFactory.ParseCommentPartitionKey(partitionKey);
            Assert.Equal(invertedTicks, _fixture.UriFactory.CreateInvertedTicksByCommentPartitionKey(partitionKey));
            //Assert.Equal(mediaType, r.MediaType);
        }
    }

    public class UriFactoryFixture
    {
        public IUriFactory UriFactory { get; set; }

        public UriFactoryFixture()
        {
            UriFactory = new UriFactory();
        }
    }
}