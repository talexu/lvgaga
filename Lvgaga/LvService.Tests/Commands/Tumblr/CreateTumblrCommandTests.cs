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
    public class CreateTumblrCommandTests : AzureStorageTestsBase
    {
        public CreateTumblrCommandTests(AzureStorageFixture fixture)
            : base(fixture)
        {

        }

        [Fact]
        public async Task CreateTumblr_Return4_Tumblr()
        {
            var partitionKey = LvConstants.PartitionKeyOfImage;
            const string mediaUri = "media uri";
            const TumblrCategory category = TumblrCategory.C1;
            const string thumbnailUri = "TestThumbnailUri";
            var tumblrText = new TumblrText
            {
                Text = "TestText",
                Category = category
            };

            dynamic p1 = new ExpandoObject();
            p1.PartitionKey = partitionKey;
            p1.MediaUri = mediaUri;
            p1.ThumbnailUri = thumbnailUri;
            p1.TumblrText = tumblrText;

            await Fixture.CreateTumblrCommand.ExecuteAsync(p1);
            TumblrEntity entity = p1.Entity;
            Assert.NotNull(entity);
            Assert.Equal(partitionKey, entity.PartitionKey);
            Assert.Equal(tumblrText.Category.ToString("D"), entity.RowKey.Substring(0, 1));
            Assert.Equal(mediaUri, entity.MediaUri);
            Assert.Equal(tumblrText.Text, entity.Text);

            List<ITableEntity> ientities = p1.Entities;
            var entities = ientities.Cast<TumblrEntity>().ToList();
            Assert.Equal(4, entities.Count());
            var entity0 = entities[0];
            var entity1 = entities[1];
            var entity2 = entities[2];
            var entity3 = entities[3];
            Assert.True(new[] { entity0.MediaUri, entity1.MediaUri, entity2.MediaUri, entity3.MediaUri }.AllEqual());
            Assert.True(
                new[] { entity0.ThumbnailUri, entity1.ThumbnailUri, entity2.ThumbnailUri, entity3.ThumbnailUri }.AllEqual());
            Assert.True(new[] { entity0.MediaUri, entity1.MediaUri, entity2.MediaUri, entity3.MediaUri }.AllEqual());
            Assert.True(
                new[] { entity0.CreateTime, entity1.CreateTime, entity2.CreateTime, entity3.CreateTime }.AllEqual());
            Assert.True(
                new[] { entity0.MediaType, entity1.MediaType, entity2.MediaType, entity3.MediaType }.AllEqual());
            Assert.True(
                new[] { entity0.TumblrCategory, entity1.TumblrCategory, entity2.TumblrCategory, entity3.TumblrCategory }
                    .AllEqual());
            Assert.True(new[]
            {
                Fixture.UriFactory.GetInvertedTicksFromTumblrRowKey(entity0.RowKey),
                Fixture.UriFactory.GetInvertedTicksFromTumblrRowKey(entity1.RowKey),
                Fixture.UriFactory.GetInvertedTicksFromTumblrRowKey(entity2.RowKey),
                Fixture.UriFactory.GetInvertedTicksFromTumblrRowKey(entity3.RowKey)
            }.AllEqual());

            // Dynamic
            // PartitionKey
            Assert.Equal(partitionKey, entity0.PartitionKey);
            Assert.Equal(partitionKey, entity1.PartitionKey);
            Assert.Equal(LvConstants.PartitionKeyOfAll, entity2.PartitionKey);
            Assert.Equal(LvConstants.PartitionKeyOfAll, entity3.PartitionKey);
            // RowKey
            Assert.Equal(category.ToString("D"), entity0.RowKey.Substring(0, 1));
            Assert.Equal(TumblrCategory.All.ToString("D"), entity1.RowKey.Substring(0, 1));
            Assert.Equal(category.ToString("D"), entity2.RowKey.Substring(0, 1));
            Assert.Equal(TumblrCategory.All.ToString("D"), entity3.RowKey.Substring(0, 1));
        }

        [Fact]
        public async Task CreateTumblr_Return2_1_Tumblr()
        {
            var partitionKey = LvConstants.PartitionKeyOfImage;
            const string mediaUri = "media uri";
            const TumblrCategory category = TumblrCategory.All;
            const string thumbnailUri = "TestThumbnailUri";
            var tumblrText = new TumblrText
            {
                Text = "TestText",
                Category = category
            };

            dynamic p1 = new ExpandoObject();
            p1.PartitionKey = partitionKey;
            p1.MediaUri = mediaUri;
            p1.ThumbnailUri = thumbnailUri;
            p1.TumblrText = tumblrText;

            await Fixture.CreateTumblrCommand.ExecuteAsync(p1);
            TumblrEntity entity = p1.Entity;
            Assert.NotNull(entity);
            Assert.Equal(partitionKey, entity.PartitionKey);
            Assert.Equal(tumblrText.Category.ToString("D"), entity.RowKey.Substring(0, 1));
            Assert.Equal(mediaUri, entity.MediaUri);
            Assert.Equal(tumblrText.Text, entity.Text);

            List<ITableEntity> ientities = p1.Entities;
            var entities = ientities.Cast<TumblrEntity>().ToList();
            Assert.Equal(2, entities.Count());
            var entity0 = entities[0];
            var entity1 = entities[1];
            Assert.True(new[] { entity0.MediaUri, entity1.MediaUri }.AllEqual());
            Assert.True(new[] { entity0.ThumbnailUri, entity1.ThumbnailUri }.AllEqual());
            Assert.True(new[] { entity0.MediaUri, entity1.MediaUri }.AllEqual());
            Assert.True(new[] { entity0.CreateTime, entity1.CreateTime }.AllEqual());
            Assert.True(
                new[] { entity0.MediaType, entity1.MediaType }.AllEqual());
            Assert.True(
                new[] { entity0.TumblrCategory, entity1.TumblrCategory }.AllEqual());
            Assert.True(new[]
            {
                Fixture.UriFactory.GetInvertedTicksFromTumblrRowKey(entity0.RowKey),
                Fixture.UriFactory.GetInvertedTicksFromTumblrRowKey(entity1.RowKey)
            }.AllEqual());

            // Dynamic
            // PartitionKey
            Assert.Equal(partitionKey, entity0.PartitionKey);
            Assert.Equal(LvConstants.PartitionKeyOfAll, entity1.PartitionKey);
            // RowKey
            Assert.Equal(category.ToString("D"), entity0.RowKey.Substring(0, 1));
            Assert.Equal(category.ToString("D"), entity1.RowKey.Substring(0, 1));
        }

        [Fact]
        public async Task CreateTumblr_Return2_2_Tumblr()
        {
            var partitionKey = LvConstants.PartitionKeyOfAll;
            const string mediaUri = "media uri";
            const TumblrCategory category = TumblrCategory.C1;
            const string thumbnailUri = "TestThumbnailUri";
            var tumblrText = new TumblrText
            {
                Text = "TestText",
                Category = category
            };

            dynamic p1 = new ExpandoObject();
            p1.PartitionKey = partitionKey;
            p1.MediaUri = mediaUri;
            p1.ThumbnailUri = thumbnailUri;
            p1.TumblrText = tumblrText;

            await Fixture.CreateTumblrCommand.ExecuteAsync(p1);
            TumblrEntity entity = p1.Entity;
            Assert.NotNull(entity);
            Assert.Equal(partitionKey, entity.PartitionKey);
            Assert.Equal(tumblrText.Category.ToString("D"), entity.RowKey.Substring(0, 1));
            Assert.Equal(mediaUri, entity.MediaUri);
            Assert.Equal(tumblrText.Text, entity.Text);

            List<ITableEntity> ientities = p1.Entities;
            var entities = ientities.Cast<TumblrEntity>().ToList();
            Assert.Equal(2, entities.Count());
            var entity0 = entities[0];
            var entity1 = entities[1];
            Assert.True(new[] { entity0.MediaUri, entity1.MediaUri }.AllEqual());
            Assert.True(new[] { entity0.ThumbnailUri, entity1.ThumbnailUri }.AllEqual());
            Assert.True(new[] { entity0.MediaUri, entity1.MediaUri }.AllEqual());
            Assert.True(new[] { entity0.CreateTime, entity1.CreateTime }.AllEqual());
            Assert.True(
                new[] { entity0.MediaType, entity1.MediaType }.AllEqual());
            Assert.True(
                new[] { entity0.TumblrCategory, entity1.TumblrCategory }.AllEqual());
            Assert.True(new[]
            {
                Fixture.UriFactory.GetInvertedTicksFromTumblrRowKey(entity0.RowKey),
                Fixture.UriFactory.GetInvertedTicksFromTumblrRowKey(entity1.RowKey)
            }.AllEqual());

            // Dynamic
            // PartitionKey
            Assert.Equal(partitionKey, entity0.PartitionKey);
            Assert.Equal(partitionKey, entity1.PartitionKey);
            // RowKey
            Assert.Equal(category.ToString("D"), entity0.RowKey.Substring(0, 1));
            Assert.Equal(TumblrCategory.All.ToString("D"), entity1.RowKey.Substring(0, 1));
        }
    }
}