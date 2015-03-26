using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using LvModel.Azure.StorageTable;
using LvModel.View.Tumblr;
using LvService.Tests.Utilities;
using Microsoft.WindowsAzure.Storage.Table;
using Xunit;

namespace LvService.Tests.Commands.Tumblr
{
    public class CreateTumblrCommandTests : AzureStorageTestsBase
    {
        public CreateTumblrCommandTests(AzureStorageFixture fixture)
            : base(fixture)
        {

        }

        [Fact]
        public async Task CreateTumblr_Return_TumblrAndCategoryAllTumblr()
        {
            const string partitionKey = "TestPartitionKey";
            const string mediaUri = "TestMediaUri";
            var tumblrText = new TumblrText
            {
                Text = "TestText",
                Category = TumblrCategory.C1
            };

            dynamic p1 = new ExpandoObject();
            p1.PartitionKey = partitionKey;
            p1.MediaUri = mediaUri;
            p1.TumblrText = tumblrText;

            await Fixture.CreateTumblrCommand.ExecuteAsync(p1);
            TumblrEntity entity = p1.Entity;
            Assert.NotNull(entity);
            Assert.Equal(partitionKey, entity.PartitionKey);
            Assert.Equal(tumblrText.Category.ToString("D"), entity.RowKey.Substring(0, 1));
            Assert.Equal(mediaUri, entity.MediaUri);
            Assert.Equal(tumblrText.Text, entity.Text);

            List<ITableEntity> entities = p1.Entities;
            Assert.Equal(2, entities.Count());
            var entity0 = entities[0];
            var entity1 = entities[1];
            Assert.Equal(entity0.PartitionKey, entity1.PartitionKey);
            Assert.Equal(tumblrText.Category.ToString("D"), entity0.RowKey.Substring(0, 1));
            Assert.Equal(TumblrCategory.All.ToString("D"), entity1.RowKey.Substring(0, 1));
        }

        [Fact]
        public async Task CreateTumblr_Return_CategoryAllTumblr()
        {
            const string partitionKey = "TestPartitionKey";
            const string mediaUri = "TestMediaUri";
            var tumblrText = new TumblrText
            {
                Text = "TestText",
                Category = TumblrCategory.All
            };

            dynamic p1 = new ExpandoObject();
            p1.PartitionKey = partitionKey;
            p1.MediaUri = mediaUri;
            p1.TumblrText = tumblrText;

            await Fixture.CreateTumblrCommand.ExecuteAsync(p1);
            TumblrEntity entity = p1.Entity;
            Assert.NotNull(entity);
            Assert.Equal(partitionKey, entity.PartitionKey);
            Assert.Equal(tumblrText.Category.ToString("D"), entity.RowKey.Substring(0, 1));
            Assert.Equal(mediaUri, entity.MediaUri);
            Assert.Equal(tumblrText.Text, entity.Text);

            List<ITableEntity> entities = p1.Entities;
            Assert.Equal(1, entities.Count());
            var entity0 = entities[0];
            Assert.Equal(entity0.PartitionKey, entity.PartitionKey);
            Assert.Equal(tumblrText.Category.ToString("D"), entity0.RowKey.Substring(0, 1));
            Assert.Equal(TumblrCategory.All.ToString("D"), entity.RowKey.Substring(0, 1));
        }
    }
}