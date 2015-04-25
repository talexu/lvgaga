using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using LvModel.Azure.StorageTable;
using LvModel.Common;
using LvModel.View.Tumblr;
using LvService.Commands.Common;
using LvService.Commands2.Lvgaga.Favlrite;
using LvService.Factories.Azure.Storage;
using LvService.Factories.Uri;
using LvService.Utilities;
using Microsoft.WindowsAzure.Storage.Table;
using Xunit;
using LvService.Commands2.Lvgaga.Tumblr;

namespace LvService.Tests.Commands.Lvgaga.Favorite
{
    public class CreateFavoriteCommandTests : IClassFixture<FavoriteFixture>
    {
        private readonly FavoriteFixture _fixture;

        public CreateFavoriteCommandTests(FavoriteFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task CreateFavorite_Return_EquivalentOfTumblrEntity()
        {
            var partitionKey = LvConstants.MediaTypeOfAll;
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

            await _fixture.CreateTumblrCommand.ExecuteAsync(p1);
            TumblrEntity tumblr = p1.Entity;

            const string userId = "userid";
            dynamic p2 = tumblr.ToExpandoObject();
            p2.RowKey = _fixture.UriFactory.GetInvertedTicksFromTumblrRowKey(tumblr.RowKey);
            p2.UserId = userId;
            await _fixture.CreateFavoriteCommand.ExecuteAsync(p2);
            FavoriteEntity favorite = p2.Entity;
            Assert.Equal(favorite.PartitionKey, userId);
            Assert.Equal(favorite.RowKey,
                _fixture.UriFactory.CreateFavoriteRowKey(partitionKey,
                    _fixture.UriFactory.GetInvertedTicksFromTumblrRowKey(tumblr.RowKey)));
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
            var partitionKey = LvConstants.MediaTypeOfImage;
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

            await _fixture.CreateTumblrCommand.ExecuteAsync(p1);
            TumblrEntity tumblr = p1.Entity;

            const string userId = "userid";
            dynamic p2 = tumblr.ToExpandoObject();
            p2.RowKey = _fixture.UriFactory.GetInvertedTicksFromTumblrRowKey(tumblr.RowKey);
            p2.UserId = userId;
            await _fixture.CreateFavoriteCommand.ExecuteAsync(p2);
            FavoriteEntity favorite = p2.Entity;
            Assert.Equal(favorite.PartitionKey, userId);
            Assert.Equal(favorite.RowKey,
                _fixture.UriFactory.CreateFavoriteRowKey(partitionKey,
                    _fixture.UriFactory.GetInvertedTicksFromTumblrRowKey(tumblr.RowKey)));
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
                _fixture.UriFactory.GetInvertedTicksFromFavoriteRowKey(entity0.RowKey),
                _fixture.UriFactory.GetInvertedTicksFromFavoriteRowKey(entity1.RowKey)
            }.AllEqual());

            // Dynamic
            // RowKey
            Assert.Equal(partitionKey, entity0.RowKey.Substring(0, 1));
            Assert.Equal(LvConstants.MediaTypeOfAll, entity1.RowKey.Substring(0, 1));
        }
    }

    public class FavoriteFixture
    {
        public ICommand CreateTumblrCommand;
        public ICommand CreateFavoriteCommand;
        public IUriFactory UriFactory;

        public FavoriteFixture()
        {
            UriFactory = new UriFactory();
            CreateTumblrCommand = new CreateTumblrCommand
            {
                UriFactory = UriFactory,
                TableEntityFactory = new TableEntityFactory(UriFactory)
            };
            CreateFavoriteCommand = new CreateFavoriteCommand
            {
                UriFactory = UriFactory,
                TableEntityFactory = new TableEntityFactory(UriFactory)
            };
        }
    }
}