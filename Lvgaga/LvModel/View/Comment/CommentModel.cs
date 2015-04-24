using System.Collections.Generic;
using LvModel.View.Tumblr;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvModel.View.Comment
{
    public class CommentModel : TumblrModel
    {
        public string Sas { get; set; }
        public List<CommentItem> Comments { get; set; }
        public TableContinuationToken ContinuationToken { get; set; }
    }
}