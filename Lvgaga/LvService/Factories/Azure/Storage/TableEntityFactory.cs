using System;
using LvModel.Azure.StorageTable;
using LvModel.View.Tumblr;
using LvService.Factories.Uri;
using LvService.Utilities;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Factories.Azure.Storage
{
    public class TableEntityFactory : ITableEntityFactory
    {
        private readonly IUriFactory _uriFactory;
        private DateTime _lastTime = DateTime.UtcNow;

        public TableEntityFactory()
        {

        }

        public TableEntityFactory(IUriFactory uriFactory)
        {
            _uriFactory = uriFactory;
        }

        public ITableEntity CreateTumblrEntity(dynamic p)
        {
            string partitionKey = p.PartitionKey;
            string mediaUri = p.MediaUri;
            string thumbnailUri = p.ThumbnailUri;
            TumblrText tumblrText = p.TumblrText;

            var now = GetUtcNow();
            var invertedTicks = DateTimeHelper.GetInvertedTicks(now);

            return new TumblrEntity(partitionKey, _uriFactory.CreateTumblrRowKey(tumblrText.Category, invertedTicks))
            {
                MediaType = partitionKey,
                MediaUri = mediaUri,
                ThumbnailUri = thumbnailUri,
                Text = tumblrText.Text,
                TumblrCategory = tumblrText.Category.ToString("D"),
                CreateTime = now
            };
        }

        public ITableEntity CreateCommentEntity(dynamic p)
        {
            string partitionKey = p.PartitionKey;
            string userId = p.UserId;
            string userName = p.UserName;
            string text = p.Text;

            var now = GetUtcNow();

            return new CommentEntity(partitionKey, DateTimeHelper.GetInvertedTicks(now))
            {
                UserId = userId,
                UserName = userName,
                Text = text,
                CommentTime = now
            };
        }

        public ITableEntity CreateFavoriteEntity(dynamic p)
        {
            string userId = p.UserId;
            string rowKey = p.RowKey;
            string mediaUri = p.MediaUri;
            string thumbnailUri = p.ThumbnailUri;
            string text = p.Text;
            DateTime createTime = p.CreateTime;
            string mediaType = p.MediaType;
            string tumblrCategory = p.TumblrCategory;

            return new FavoriteEntity(userId, _uriFactory.CreateFavoriteRowKey(mediaType, rowKey))
            {
                MediaUri = mediaUri,
                ThumbnailUri = thumbnailUri,
                Text = text,
                CreateTime = createTime,
                MediaType = mediaType,
                TumblrCategory = tumblrCategory
            };
        }

        private DateTime GetUtcNow()
        {
            DateTime now;
            do
            {
                now = DateTime.UtcNow;
            } while (_lastTime.Equals(now));
            _lastTime = now;

            return now;
        }
    }
}