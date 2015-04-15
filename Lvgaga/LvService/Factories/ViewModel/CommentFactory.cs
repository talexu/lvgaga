using System.Collections.Generic;
using System.Linq;
using LvModel.Azure.StorageTable;
using LvModel.View.Comment;
using LvModel.View.Tumblr;

namespace LvService.Factories.ViewModel
{
    public class CommentFactory : ICommentFactory
    {
        public CommentItem CreateCommentItem(CommentEntity entity)
        {
            if (entity == null) return null;

            return new CommentItem
            {
                PartitionKey = entity.PartitionKey,
                RowKey = entity.RowKey,
                UserId = entity.UserId,
                UserName = entity.UserName,
                Text = entity.Text,
                CommentTime = entity.CommentTime
            };
        }

        public List<CommentItem> CreateCommentItems(IEnumerable<CommentEntity> entities)
        {
            return (from entity in entities where entity != null select CreateCommentItem(entity)).ToList();
        }

        public CommentModel CreateCommentModel(TumblrModel tumblr)
        {
            if (tumblr == null) return null;

            return new CommentModel
            {
                PartitionKey = tumblr.PartitionKey,
                RowKey = tumblr.RowKey,
                Uri = tumblr.Uri,
                MediaUri = tumblr.MediaUri,
                Text = tumblr.Text,
                CreateTime = tumblr.CreateTime,
                IsFavorited = false,
            };
        }

        public CommentModel CreateCommentModels(TumblrModel tumblr, IEnumerable<CommentEntity> entities)
        {
            var commentModel = CreateCommentModel(tumblr);
            if (commentModel == null) return null;

            commentModel.Comments = CreateCommentItems(entities);
            return commentModel;
        }
    }
}