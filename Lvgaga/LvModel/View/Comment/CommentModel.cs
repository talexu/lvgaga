using System.Collections.Generic;
using LvModel.View.Tumblr;

namespace LvModel.View.Comment
{
    public class CommentModel : TumblrModel
    {
        public List<CommentItem> Comments { get; set; }
    }
}