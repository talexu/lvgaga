namespace LvModel.Azure.StorageTable
{
    /// <summary>
    /// 收藏实体, PartitionKey为UserId, RowKey为"反向时间戳/媒体类型"
    /// </summary>
    public class FavoriteEntity : TumblrEntity
    {
        public string PartitionKeyOfTumblr { get; set; }

        public FavoriteEntity(string userId, string tumblrId)
        {
            PartitionKey = userId;
            RowKey = tumblrId;
        }
    }
}
