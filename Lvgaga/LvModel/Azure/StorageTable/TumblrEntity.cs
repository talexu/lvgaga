using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvModel.Azure.StorageTable
{
    /// <summary>
    /// 图文实体. PartitionKey为媒体类型编号, RowKey为"文字类型/反向时间戳"
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

        public string MediaUri { get; set; }
        public string Thumbnail { get; set; }
        public string Text { get; set; }
        public DateTime CreateTime { get; set; }
        public EntityState State { get; set; }
    }
}
