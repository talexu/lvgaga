using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using LvModel.Azure.StorageTable;
using LvModel.Common;
using LvModel.View.Tumblr;
using LvService.Tests.Utilities;
using LvService.Utilities;
using Xunit;

namespace LvService.Tests.Commands.Tumblr
{
    public class ReadRangeFavoriteEntitiesCommandTests : AzureStorageTestsBase
    {
        public ReadRangeFavoriteEntitiesCommandTests(AzureStorageFixture fixture)
            : base(fixture)
        {

        }

        [Fact]
        public async Task GetRangeFavorite_Return_During()
        {
            var partitionKey = LvConstants.PartitionKeyOfImage;
            const string mediaUri = "TestMediaUri";
            const string thumbnailUri = "TestThumbnailUri";
            var tableName = Fixture.GetRandomName("t");
            var tumblrText = new TumblrText
            {
                Text = "TestText",
                Category = TumblrCategory.C1
            };
            var tumblrs = new List<TumblrEntity>();
            const string userId = "userid";

            for (var i = 0; i < 20; i++)
            {
                // create
                dynamic p1 = new ExpandoObject();
                p1.PartitionKey = partitionKey;
                p1.MediaUri = mediaUri;
                p1.ThumbnailUri = thumbnailUri;
                p1.TumblrText = tumblrText;

                await Fixture.CreateTumblrCommand.ExecuteAsync(p1);
                TumblrEntity tumblr = p1.Entity;
                tumblrs.Add(tumblr);

                dynamic p2 = tumblr.ToExpandoObject();
                p2.RowKey = Fixture.UriFactory.GetInvertedTicksFromTumblrRowKey(tumblr.RowKey);
                p2.UserId = userId;
                p2.Table = await Fixture.AzureStorage.GetTableReferenceAsync(tableName);
                await Fixture.CreateFavoriteCommandOnline.ExecuteAsync(p2);
            }

            // read
            var tumblr0 = tumblrs[0];
            var tumblr10 = tumblrs[10];
            dynamic pr1 = new ExpandoObject();
            pr1.Table = await Fixture.AzureStorage.GetTableReferenceAsync(tableName);
            pr1.PartitionKey = userId;
            pr1.From = Fixture.UriFactory.GetInvertedTicksFromTumblrRowKey(tumblr10.RowKey);
            pr1.To = Fixture.UriFactory.GetInvertedTicksFromTumblrRowKey(tumblr0.RowKey);
            pr1.MediaType = partitionKey;
            List<FavoriteEntity> result1 = await Fixture.ReadRangeFavoriteEntitiesCommand.ExecuteAsync<FavoriteEntity>(pr1);
            Assert.Equal(11, result1.Count);

            dynamic pr2 = new ExpandoObject();
            pr2.Table = await Fixture.AzureStorage.GetTableReferenceAsync(tableName);
            pr2.PartitionKey = userId;
            pr2.From = Fixture.UriFactory.GetInvertedTicksFromTumblrRowKey(tumblr0.RowKey);
            pr2.To = Fixture.UriFactory.GetInvertedTicksFromTumblrRowKey(tumblr0.RowKey);
            pr2.MediaType = partitionKey;
            List<FavoriteEntity> result2 = await Fixture.ReadRangeFavoriteEntitiesCommand.ExecuteAsync<FavoriteEntity>(pr2);
            Assert.Equal(1, result2.Count);

            dynamic pr3 = new ExpandoObject();
            pr3.Table = await Fixture.AzureStorage.GetTableReferenceAsync(tableName);
            pr3.PartitionKey = userId;
            pr3.From = Fixture.UriFactory.GetInvertedTicksFromTumblrRowKey(tumblr0.RowKey);
            pr3.To = Fixture.UriFactory.GetInvertedTicksFromTumblrRowKey(tumblr10.RowKey);
            pr3.MediaType = partitionKey;
            List<FavoriteEntity> result3 = await Fixture.ReadRangeFavoriteEntitiesCommand.ExecuteAsync<FavoriteEntity>(pr3);
            Assert.Empty(result3);

            // delete
            dynamic pd = new ExpandoObject();
            pd.Table = await Fixture.AzureStorage.GetTableReferenceAsync(tableName);
            await Fixture.DeleteTableCommand.ExecuteAsync(pd);
        }
    }
}