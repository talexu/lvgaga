using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvModel.Azure.StorageTable
{
    public class CommentEntity : TableEntity
    {
        public CommentEntity(string tumblrId, string inverseCommentTime)
        {
            PartitionKey = tumblrId;
            RowKey = inverseCommentTime;
        }

        public CommentEntity()
        {

        }

        public string UserId { get; set; }
        public string Text { get; set; }
        public DateTime CommentTime { get; set; }
    }
}
