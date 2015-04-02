using System.Dynamic;
using LvModel.Azure.StorageTable;
using LvModel.Common;
using LvModel.View.Tumblr;
using LvService.Tests.Utilities;
using LvService.Utilities;
using Xunit;

namespace LvService.Tests.Factories
{
    public class TableEntityFactoryTests : AzureStorageTestsBase
    {
        public TableEntityFactoryTests(AzureStorageFixture fixture)
            : base(fixture)
        {

        }

        [Fact]
        public void CreateTumblrEntityTest()
        {
            dynamic p = new ExpandoObject();
            p.PartitionKey = LvConstants.PartitionKeyOfImage;
            p.MediaUri = "uri";
            p.ThumbnailUri = "thumb";
            p.TumblrText = new TumblrText
            {
                Text = "Test text",
                Category = TumblrCategory.C1
            };
            TumblrEntity data = Fixture.TableEntityFactory.CreateTumblrEntity(p);

            Assert.Equal(p.PartitionKey, data.PartitionKey);
            Assert.Equal(
                string.Format("{0}_{1}", p.TumblrText.Category.ToString("D"),
                    DateTimeHelper.GetInvertedTicks(data.CreateTime)), data.RowKey);
            Assert.Equal(p.MediaUri, data.MediaUri);
            Assert.Equal(p.ThumbnailUri, data.ThumbnailUri);
            Assert.Equal(p.TumblrText.Text, data.Text);
        }

        [Fact]
        public void CreateFavoriteEntity_Return_EquivalentEntityOfTumblr()
        {
            dynamic p = new ExpandoObject();
            p.PartitionKey = LvConstants.PartitionKeyOfImage;
            p.MediaUri = "uri";
            p.ThumbnailUri = "thumb";
            p.TumblrText = new TumblrText
            {
                Text = "Test text",
                Category = TumblrCategory.C1
            };
            TumblrEntity tumblr = Fixture.TableEntityFactory.CreateTumblrEntity(p);

            const string userId = "userid";
            dynamic p1 = tumblr.ToExpandoObject();
            p1.RowKey = Fixture.UriFactory.GetInvertedTicksFromTumblrRowKey(tumblr.RowKey);
            p1.UserId = userId;
            FavoriteEntity favorite = Fixture.TableEntityFactory.CreateFavoriteEntity(p1);

            Assert.Equal(userId, favorite.PartitionKey);
            Assert.Equal(
                Fixture.UriFactory.CreateFavoriteRowKey(p.PartitionKey,
                    Fixture.UriFactory.GetInvertedTicksFromTumblrRowKey(tumblr.RowKey)), favorite.RowKey);
            Assert.Equal(tumblr.MediaUri, favorite.MediaUri);
            Assert.Equal(tumblr.ThumbnailUri, favorite.ThumbnailUri);
            Assert.Equal(tumblr.Text, favorite.Text);
            Assert.Equal(tumblr.CreateTime, favorite.CreateTime);
        }
    }
}
