namespace LvModel.Azure.StorageTable
{
    /// <summary>
    /// 收藏实体, PartitionKey为UserId, RowKey为"媒体类型_反向时间戳"
    /// </summary>
    public class FavoriteEntity : TumblrEntity
    {
        public FavoriteEntity()
        {

        }

        public FavoriteEntity(string userId, string tumblrId)
        {
            PartitionKey = userId;
            RowKey = tumblrId;
        }
    }
}
