using System;
using LvModel.Azure.StorageTable;
using LvModel.View.Tumblr;
using LvService.Utilities;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Factories.Azure.Storage
{
    public class TableEntityFactory : ITableEntityFactory
    {
        private DateTime _lastTime = DateTime.UtcNow;

        public ITableEntity CreateTumblrEntity(dynamic p)
        {
            string partitionKey = p.PartitionKey;
            string mediaUri = p.MediaUri;
            TumblrText tumblrText = p.TumblrText;

            var now = GetUtcNow();

            return new TumblrEntity(partitionKey,
                string.Format("{0}_{1}", tumblrText.Category.ToString("D"), DateTimeHelper.GetInvertedTicks(now)))
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