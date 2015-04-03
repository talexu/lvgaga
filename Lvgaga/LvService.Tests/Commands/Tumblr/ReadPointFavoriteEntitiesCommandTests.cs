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
    public class ReadPointFavoriteEntitiesCommandTests : AzureStorageTestsBase
    {
        public ReadPointFavoriteEntitiesCommandTests(AzureStorageFixture fixture)
            : base(fixture)
        {

        }

        [Fact]
        public async Task Read_Return2_MediaType()
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

            // create
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
            p2.Table = await Fixture.AzureStorage.GetTableReferenceAsync(tableName);
            await Fixture.CreateFavoriteCommandOnline.ExecuteAsync(p2);

            // read
            dynamic pr = new ExpandoObject();
            pr.Table = await Fixture.AzureStorage.GetTableReferenceAsync(tableName);
            pr.PartitionKey = userId;
            pr.RowKey = Fixture.UriFactory.GetInvertedTicksFromTumblrRowKey(tumblr.RowKey);
            pr.MediaType = tumblr.MediaType;
            List<FavoriteEntity> entities = await Fixture.ReadPointFavoriteEntitiesCommand.ExecuteAsync<FavoriteEntity>(pr);
            var entity0 = entities[0];
            var entity1 = entities[1];
            Assert.Equal(2, entities.Count);
            Assert.Equal(Fixture.UriFactory.GetInvertedTicksFromFavoriteRowKey(entity0.RowKey),
                Fixture.UriFactory.GetInvertedTicksFromFavoriteRowKey(entity1.RowKey));
            Assert.Equal(LvConstants.PartitionKeyOfAll, entity0.RowKey.Substring(0, 1));
            Assert.Equal(partitionKey, entity1.RowKey.Substring(0, 1));

            // delete
            dynamic pde = new ExpandoObject();
            pde.Table = await Fixture.AzureStorage.GetTableReferenceAsync(tableName);
            pde.PartitionKey = userId;
            pde.RowKey = Fixture.UriFactory.GetInvertedTicksFromTumblrRowKey(tumblr.RowKey);
            pde.MediaType = tumblr.MediaType;
            await Fixture.DeletePointFavoriteEntitiesCommand.ExecuteAsync<FavoriteEntity>(pde);

            dynamic pr2 = new ExpandoObject();
            pr2.Table = await Fixture.AzureStorage.GetTableReferenceAsync(tableName);
            pr2.PartitionKey = userId;
            pr2.RowKey = Fixture.UriFactory.GetInvertedTicksFromTumblrRowKey(tumblr.RowKey);
            pr2.MediaType = tumblr.MediaType;
            List<FavoriteEntity> entities2 = await Fixture.ReadPointFavoriteEntitiesCommand.ExecuteAsync<FavoriteEntity>(pr);
            Assert.Empty(entities2);

            // clear
            dynamic pdt = new ExpandoObject();
            pdt.Table = await Fixture.AzureStorage.GetTableReferenceAsync(tableName);
            await Fixture.DeleteTableCommand.ExecuteAsync(pdt);
        }
    }
}