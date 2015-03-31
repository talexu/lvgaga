using System.Dynamic;
using System.Threading.Tasks;
using LvModel.Azure.StorageTable;
using LvModel.View.Tumblr;
using LvService.Tests.Utilities;
using LvService.Utilities;
using Xunit;

namespace LvService.Tests.Commands.Tumblr
{
    public class CreateFavoriteCommandTests : AzureStorageTestsBase
    {
        public CreateFavoriteCommandTests(AzureStorageFixture fixture)
            : base(fixture)
        {

        }

        [Fact]
        public async Task CreateFavorite_Return_EquivalentOfTumblrEntity()
        {
            const string partitionKey = "TestPartitionKey";
            const string mediaUri = "TestMediaUri";
            const string thumbnailUri = "TestThumbnailUri";
            var tumblrText = new TumblrText
            {
                Text = "TestText",
                Category = TumblrCategory.C1
            };

            dynamic p1 = new ExpandoObject();
            p1.PartitionKey = partitionKey;
            p1.MediaUri = mediaUri;
            p1.ThumbnailUri = thumbnailUri;
            p1.TumblrText = tumblrText;

            await Fixture.CreateTumblrCommand.ExecuteAsync(p1);
            TumblrEntity tumblr = p1.Entity;

            const string userId = "userid";
            dynamic p2 = tumblr.ToExpandoObject();
            p2.UserId = userId;
            await Fixture.CreateFavoriteCommand.ExecuteAsync(p2);
            FavoriteEntity favorite = p2.Entity;
            Assert.Equal(favorite.PartitionKey, userId);
            Assert.Equal(favorite.RowKey,
                Fixture.UriFactory.CreateFavoriteRowKey(partitionKey,
                    Fixture.UriFactory.GetInvertedTicksFromTumblrRowKey(tumblr.RowKey)));
            Assert.Equal(favorite.MediaUri, tumblr.MediaUri);
            Assert.Equal(favorite.ThumbnailUri, tumblr.ThumbnailUri);
            Assert.Equal(favorite.Text, tumblr.Text);
            Assert.Equal(favorite.CreateTime, tumblr.CreateTime);
        }
    }
}