﻿using System.Collections.Generic;
using System.Linq;
using LvModel.Azure.StorageTable;
using LvModel.View.Comment;
using LvModel.View.Tumblr;
using LvService.Factories.Uri;

namespace LvService.Factories.ViewModel
{
    public class CommentFactory : ICommentFactory
    {
        private readonly IUriFactory _uriFactory;

        public CommentFactory()
        {

        }

        public CommentFactory(IUriFactory uriFactory)
        {
            _uriFactory = uriFactory;
        }

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

        public CommentModel CreateCommentModels(TumblrModel tumblr, IEnumerable<CommentEntity> entities)
        {
            if (tumblr == null) return null;

            return new CommentModel
            {
                PartitionKey = tumblr.PartitionKey,
                RowKey = _uriFactory.GetInvertedTicksFromTumblrRowKey(tumblr.RowKey),
                Uri = tumblr.Uri,
                MediaUri = tumblr.MediaUri,
                Text = tumblr.Text,
                CreateTime = tumblr.CreateTime,
                Comments = CreateCommentItems(entities)
            };
        }
    }
}