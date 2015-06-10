using LvModel.Azure.StorageTable;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvModel.View.Comment
{
    public class CommentModel : TumblrEntity
    {
        public string Sas { get; set; }
        public TableContinuationToken ContinuationToken { get; set; }
    }
}