using System.Collections.Generic;
using LvModel.View.Tumblr;

namespace LvModel.View.Comment
{
    public class CommentModel : TumblrModel
    {
        public string Sas { get; set; }
        public string FavoriteSas { get; set; }
        public List<CommentItem> Comments { get; set; }
    }
}