using System.Collections.Generic;
using LvModel.Azure.StorageTable;
using LvModel.View.Comment;
using LvModel.View.Tumblr;

namespace LvService.Factories.ViewModel
{
    public interface ICommentFactory
    {
        CommentItem CreateCommentItem(CommentEntity entity);
        List<CommentItem> CreateCommentItems(IEnumerable<CommentEntity> entities);
        CommentModel CreateCommentModels(TumblrModel tumblr, IEnumerable<CommentEntity> entities);
    }
}