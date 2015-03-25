using System;
using LvModel.Azure.StorageTable;
using LvModel.View.Tumblr;
using LvService.Utilities;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Factories.Azure.Storage
{
    public class TableEntityFactory : ITableEntityFactory
    {
        public ITableEntity CreateTumblrEntity(dynamic p)
        {
            string partitionKey = p.PartitionKey;
            string mediaUri = p.MediaUri;
            TumblrText tumblrText = p.TumblrText;
            var now = DateTime.UtcNow;

            return new TumblrEntity(partitionKey,
                string.Format("{0}_{1}", tumblrText.Category.ToString("D"), DateTimeHelper.GetInvertedTicks(now)))
            {
                MediaUri = mediaUri,
                Text = tumblrText.Text,
                CreateTime = now,
                State = TumblrState.Active
            };
        }
    }
}