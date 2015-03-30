using Microsoft.WindowsAzure.Storage.Table;

namespace LvModel.Azure.StorageTable
{
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
