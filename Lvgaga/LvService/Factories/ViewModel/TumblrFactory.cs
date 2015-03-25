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

            var result = new TumblrModel
            {
                PartitionKey = tumblrEntity.PartitionKey,
                RowKey = tumblrEntity.RowKey,
                Uri = _uriFactory.CreateUri(Path.Combine(CommentControllerName, tumblrEntity.RowKey)),
                MediaUri = tumblrEntity.MediaUri,
                Text = tumblrEntity.Text,
                CreateTime = tumblrEntity.CreateTime
            };

            return result;
        }

        public List<TumblrModel> CreateTumblrModels(IEnumerable<TumblrEntity> tumblrEntities)
        {
            return tumblrEntities == null ? null : tumblrEntities.Select(CreateTumblrModel).ToList();
        }
    }
}