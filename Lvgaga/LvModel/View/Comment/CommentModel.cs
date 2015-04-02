using System.Collections.Generic;
using LvModel.View.Tumblr;

namespace LvModel.View.Comment
{
    public class CommentModel : TumblrModel
    {
        public bool IsFavorited { get; set; }
        public List<CommentItem> Comments { get; set; }
    }
}