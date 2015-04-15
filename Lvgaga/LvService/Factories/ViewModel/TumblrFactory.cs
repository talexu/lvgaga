using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LvModel.Azure.StorageTable;
using LvModel.View.Tumblr;
using LvService.Factories.Uri;

namespace LvService.Factories.ViewModel
{
    public class TumblrFactory : ITumblrFactory
    {
        private const string CommentControllerName = "comments";
        private readonly IUriFactory _uriFactory;

        public TumblrFactory()
        {

        }

        public TumblrFactory(IUriFactory uriFactory)
        {
            _uriFactory = uriFactory;
        }

        public TumblrModel CreateTumblrModel(TumblrEntity tumblrEntity)
        {
            if (tumblrEntity == null) return null;

            var invertedTicks = _uriFactory.GetInvertedTicksFromTumblrRowKey(tumblrEntity.RowKey);
            var path = Path.Combine(tumblrEntity.MediaType, invertedTicks);

            return new TumblrModel
            {
                PartitionKey = tumblrEntity.PartitionKey,
                RowKey = invertedTicks,
                Id = path,
                Uri = _uriFactory.CreateUri(Path.Combine(CommentControllerName, path)),
                MediaType = tumblrEntity.MediaType,
                MediaUri = tumblrEntity.MediaUri,
                ThumbnailUri = tumblrEntity.ThumbnailUri,
                TumblrCategory = tumblrEntity.TumblrCategory,
                Text = tumblrEntity.Text,
                CreateTime = tumblrEntity.CreateTime
            };
        }

        public List<TumblrModel> CreateTumblrModels(IEnumerable<TumblrEntity> tumblrEntities)
        {
            return tumblrEntities == null ? null : tumblrEntities.Select(CreateTumblrModel).ToList();
        }
    }
}