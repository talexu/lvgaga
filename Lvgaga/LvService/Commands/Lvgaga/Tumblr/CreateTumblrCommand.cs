using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LvModel.Azure.StorageTable;
using LvModel.Common;
using LvModel.View.Tumblr;
using LvService.Commands.Lvgaga.Common;
using LvService.Utilities;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Commands.Lvgaga.Tumblr
{
    public class CreateTumblrCommand : AbstractCreateLvEntityCommand
    {
        private string _partitionKey;
        private string _mediaUri;
        private string _mediaLargeUri;
        private string _mediaMediumUri;
        private string _mediaSmallUri;
        private TumblrText _tumblrText;

        public override bool CanExecute(dynamic p)
        {
            _partitionKey = p.PartitionKey;
            _mediaUri = p.MediaUri;
            _mediaLargeUri = p.MediaLargeUri;
            _mediaMediumUri = p.MediaMediumUri;
            _mediaSmallUri = p.MediaSmallUri;
            _tumblrText = p.TumblrText;
            return
                new[] { _partitionKey, _mediaUri, _mediaLargeUri, _mediaMediumUri, _mediaSmallUri }.AllNotNullOrEmpty() &&
                _tumblrText != null;
        }

        public override Task ExecuteAsync(dynamic p)
        {
            if (!CanExecute(p)) return Task.FromResult(false);

            // Create TumblrEntity
            TumblrEntity tumblrEntity = TableEntityFactory.CreateTumblrEntity(p);
            if (tumblrEntity == null) return Task.FromResult(false);
            p.Entity = tumblrEntity;

            // Copies
            var entities = new List<TumblrEntity> { tumblrEntity };
            if (!_tumblrText.Category.Equals(TumblrCategory.All))
            {
                var copyEntity = tumblrEntity.CloneByJson();
                copyEntity.RowKey = UriFactory.ReplaceTumblrCategoryOfRowKey(tumblrEntity.RowKey, TumblrCategory.All);
                entities.Add(copyEntity);
            }
            if (!tumblrEntity.PartitionKey.Equals(LvConstants.MediaTypeOfAll))
            {
                var copyEntities = entities.CloneByJson<TumblrEntity>();
                foreach (var copyEntity in copyEntities)
                {
                    copyEntity.PartitionKey = LvConstants.MediaTypeOfAll;
                    entities.Add(copyEntity);
                }
            }

            p.Entities = entities.Cast<ITableEntity>().ToList();

            return Task.FromResult(true);
        }
    }
}