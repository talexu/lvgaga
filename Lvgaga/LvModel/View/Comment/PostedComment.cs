namespace LvModel.View.Comment
{
    public class PostedComment
    {
        public string PartitionKey { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Text { get; set; }
    }
}