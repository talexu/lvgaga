using System.IO;
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
        [InlineData("comments", "http://www.lvgaga.com/comments")]
        [InlineData("comments/1", "http://www.lvgaga.com/comments/1")]
        [InlineData("", "http://www.lvgaga.com/")]
        public void CreateUri_Return_CorrectUri(string path, string expected)
        {
            Assert.Equal(expected, _fixture.UriFactory.CreateUri(path));
        }

        [Fact]
        public void CreateUri_Return_CorrectUri_PathCombine()
        {
            Assert.Equal("http://www.lvgaga.com/comments/1",
                _fixture.UriFactory.CreateUri(Path.Combine("comments", "1")));
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