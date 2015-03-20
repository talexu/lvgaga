using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvModel.Azure.StorageTable
{
    public class TumblrEntity : TableEntity
    {
        public TumblrEntity()
        {
        }

        public TumblrEntity(string mediaType, string inverseCreateTime)
        {
            PartitionKey = mediaType;
            RowKey = inverseCreateTime;
        }

        public string Uri { get; set; }
        public string Text { get; set; }
        public DateTime CreateTime { get; set; }
        public TumblrState State { get; set; }
    }

    public enum TumblrState
    {
        Active = 0,
        Inactive = 1
    }
}
