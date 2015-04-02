using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvModel.Azure.StorageTable
{
    /// <summary>
    /// 图文实体. PartitionKey为媒体类型编号, RowKey为"文字类型_反向时间戳"
    /// </summary>
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

        public string MediaType { get; set; }
        public string MediaUri { get; set; }
        public string ThumbnailUri { get; set; }
        public string TumblrCategory { get; set; }
        public string Text { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
