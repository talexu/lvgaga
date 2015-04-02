using System;
using System.Dynamic;
using System.Threading.Tasks;
using LvModel.Azure.StorageTable;
using LvModel.Common;
using LvService.Tests.Utilities;
using LvService.Utilities;
using Xunit;

namespace LvService.Tests.Commands.Tumblr
{
    public class ReadFavoriteCommandTests : AzureStorageTestsBase
    {
        public ReadFavoriteCommandTests(AzureStorageFixture fixture)
            : base(fixture)
        {

        }

        [Fact]
        public async Task ReadFavoriteEntity_Return_CorrentEntity()
        {
            var tableName = Fixture.GetRandomName(AzureStorageFixture.ContainerPrefix);
            const string userId = "test user id";
            var now = DateTimeHelper.GetInvertedTicksNow();
            var mediaType = MediaType.Image.ToString("D");
            var rowKey = Fixture.UriFactory.CreateFavoriteRowKey(mediaType, now);
            // Create
            FavoriteEntity favorite = new FavoriteEntity(userId, rowKey)
            {
                MediaType = mediaType,
                CreateTime = DateTime.UtcNow,
                MediaUri = "www.caoliu.com",
                Text = "test text",
                ThumbnailUri = "caoliu.com",
                TumblrCategory = "0"
            };

            dynamic pc = favorite.ToExpandoObject();
            pc.Table = await Fixture.AzureStorage.GetTableReferenceAsync(tableName);
            pc.Entity = favorite;
            await Fixture.CreateTableEntityCommand.ExecuteAsync(pc);

            // Read
            dynamic pr = new ExpandoObject();
            pr.Table = await Fixture.AzureStorage.GetTableReferenceAsync(tableName);
            pr.PartitionKey = userId;
            pr.RowKey = rowKey;
            FavoriteEntity read = await Fixture.ReadTableEntityCommand.ExecuteAsync<FavoriteEntity>(pr);
            Assert.Equal(favorite.PartitionKey, read.PartitionKey);
            Assert.Equal(favorite.RowKey, read.RowKey);
            Assert.Equal(favorite.MediaType, read.MediaType);
            Assert.Equal(favorite.TumblrCategory, read.TumblrCategory);

            // delete table
            dynamic pd = new ExpandoObject();
            pd.Table = await Fixture.AzureStorage.GetTableReferenceAsync(tableName);
            await Fixture.DeleteTableCommand.ExecuteAsync(pd);
        }
    }
}