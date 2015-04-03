using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using LvModel.Azure.StorageTable;
using LvModel.Common;
using LvModel.View.Tumblr;
using LvService.Tests.Utilities;
using LvService.Utilities;
using Microsoft.WindowsAzure.Storage.Table;
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
            var partitionKey = LvConstants.PartitionKeyOfAll;
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
            p2.RowKey = Fixture.UriFactory.GetInvertedTicksFromTumblrRowKey(tumblr.RowKey);
            p2.UserId = userId;
            await Fixture.CreateFavoriteCommandOffline.ExecuteAsync(p2);
            FavoriteEntity favorite = p2.Entity;
            Assert.Equal(favorite.PartitionKey, userId);
            Assert.Equal(favorite.RowKey,
                Fixture.UriFactory.CreateFavoriteRowKey(partitionKey,
                    Fixture.UriFactory.GetInvertedTicksFromTumblrRowKey(tumblr.RowKey)));
            Assert.Equal(favorite.MediaUri, tumblr.MediaUri);
            Assert.Equal(favorite.ThumbnailUri, tumblr.ThumbnailUri);
            Assert.Equal(favorite.Text, tumblr.Text);
            Assert.Equal(favorite.CreateTime, tumblr.CreateTime);
            Assert.Equal(favorite.MediaType, tumblr.MediaType);
            Assert.Equal(favorite.TumblrCategory, tumblr.TumblrCategory);

            List<ITableEntity> ientities = p2.Entities;
            var entities = ientities.Cast<FavoriteEntity>().ToList();
            Assert.Equal(1, entities.Count());
        }

        [Fact]
        public async Task CreateFavorite_Return_2_EquivalentOfTumblrEntity()
        {
            var partitionKey = LvConstants.PartitionKeyOfImage;
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
            p2.RowKey = Fixture.UriFactory.GetInvertedTicksFromTumblrRowKey(tumblr.RowKey);
            p2.UserId = userId;
            await Fixture.CreateFavoriteCommandOffline.ExecuteAsync(p2);
            FavoriteEntity favorite = p2.Entity;
            Assert.Equal(favorite.PartitionKey, userId);
            Assert.Equal(favorite.RowKey,
                Fixture.UriFactory.CreateFavoriteRowKey(partitionKey,
                    Fixture.UriFactory.GetInvertedTicksFromTumblrRowKey(tumblr.RowKey)));
            Assert.Equal(favorite.MediaUri, tumblr.MediaUri);
            Assert.Equal(favorite.ThumbnailUri, tumblr.ThumbnailUri);
            Assert.Equal(favorite.Text, tumblr.Text);
            Assert.Equal(favorite.CreateTime, tumblr.CreateTime);
            Assert.Equal(favorite.MediaType, tumblr.MediaType);
            Assert.Equal(favorite.TumblrCategory, tumblr.TumblrCategory);

            List<ITableEntity> ientities = p2.Entities;
            var entities = ientities.Cast<FavoriteEntity>().ToList();
            Assert.Equal(2, entities.Count());
            var entity0 = entities[0];
            var entity1 = entities[1];
            Assert.True(new[] { entity0.MediaUri, entity1.MediaUri }.AllEqual());
            Assert.True(new[] { entity0.ThumbnailUri, entity1.ThumbnailUri }.AllEqual());
            Assert.True(new[] { entity0.MediaUri, entity1.MediaUri }.AllEqual());
            Assert.True(new[] { entity0.CreateTime, entity1.CreateTime }.AllEqual());
            Assert.True(new[] { entity0.MediaType, entity1.MediaType }.AllEqual());
            Assert.True(new[] { entity0.TumblrCategory, entity1.TumblrCategory }.AllEqual());
            Assert.True(new[]
            {
                Fixture.UriFactory.GetInvertedTicksFromFavoriteRowKey(entity0.RowKey),
                Fixture.UriFactory.GetInvertedTicksFromFavoriteRowKey(entity1.RowKey)
            }.AllEqual());

            // Dynamic
            // RowKey
            Assert.Equal(partitionKey, entity0.RowKey.Substring(0, 1));
            Assert.Equal(LvConstants.PartitionKeyOfAll, entity1.RowKey.Substring(0, 1));
        }
    }
}