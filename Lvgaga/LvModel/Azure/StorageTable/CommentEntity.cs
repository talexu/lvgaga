using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvModel.Azure.StorageTable
{
    /// <summary>
    /// 评论实体, PartitionKey为"反向时间戳_媒体类型", RowKey为"反向时间戳(发评论的)"
    /// 此处PartitionKey无文字类型的概念, 因为该实体和文字类型是一对多的关系
    /// </summary>
    public class CommentEntity : TableEntity
    {
        /// <summary>
        /// 构造函数, 传入PartitionKey和RowKey
        /// </summary>
        /// <param name="tumblrId">不带类型前缀的TumblrId</param>
        /// <param name="inverseCommentTime">评论时间的逆序</param>
        public CommentEntity(string tumblrId, string inverseCommentTime)
        {
            PartitionKey = tumblrId;
            RowKey = inverseCommentTime;
        }

        public CommentEntity()
        {

        }

        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Text { get; set; }
        public DateTime CommentTime { get; set; }
    }
}
