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
            TumblrText tumblrText = p.TumblrText;

            var now = GetUtcNow();
            string invertedTicks = null;
            try
            {
                invertedTicks = p.InvertedTicks;
            }
            catch (Exception)
            {
                invertedTicks = DateTimeHelper.GetInvertedTicks(now);
            }
            finally
            {
                if (!String.IsNullOrEmpty(invertedTicks))
                {
                    p.InvertedTicks = invertedTicks;
                }
            }

            return new TumblrEntity(partitionKey, _uriFactory.CreateTumblrRowKey(tumblrText.Category, invertedTicks))
            {
                MediaUri = mediaUri,
                Text = tumblrText.Text,
                CreateTime = now,
                State = EntityState.Active
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
                CommentTime = now,
                State = EntityState.Active
            };
        }

        public ITableEntity CreateFavoriteEntity(dynamic p)
        {
            return null;
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