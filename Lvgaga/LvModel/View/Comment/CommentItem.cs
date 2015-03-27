using System;

namespace LvModel.View.Comment
{
    public class CommentItem
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Text { get; set; }
        public DateTime CommentTime { get; set; }
    }
}