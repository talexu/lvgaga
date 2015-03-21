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

        public override bool Equals(object obj)
        {
            var obj2 = obj as TumblrEntity;
            if (obj2 == null) return false;
            if (PartitionKey != obj2.PartitionKey) return false;
            if (RowKey != obj2.RowKey) return false;
            if (Uri != obj2.Uri) return false;
            if (Text != obj2.Text) return false;
            if (!CreateTime.Equals(obj2.CreateTime)) return false;
            if (!State.Equals(obj2.State)) return false;

            return true;
        }
    }

    public enum TumblrState
    {
        Active = 0,
        Inactive = 1
    }
}
