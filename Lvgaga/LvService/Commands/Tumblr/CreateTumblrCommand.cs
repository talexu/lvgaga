using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using LvModel.Azure.StorageTable;
using LvModel.Common;
using LvModel.View.Tumblr;
using LvService.Commands.Common;
using LvService.Utilities;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Commands.Tumblr
{
    public class CreateTumblrCommand : CreateLvEntityCommand
    {
        private string _partitionKey;
        private string _mediaUri;
        private string _thumbnailUri;
        private TumblrText _tumblrText;

        public CreateTumblrCommand()
        {

        }

        public CreateTumblrCommand(ICommand command)
            : base(command)
        {

        }

        public new bool CanExecute(dynamic p)
        {
            _partitionKey = p.PartitionKey;
            _mediaUri = p.MediaUri;
            _thumbnailUri = p.ThumbnailUri;
            _tumblrText = p.TumblrText;
            return new[] { _partitionKey, _mediaUri, _thumbnailUri }.AllNotNullOrEmpty() && _tumblrText != null;
        }

        public override async Task ExecuteAsync(dynamic p)
        {
            await base.ExecuteAsync(p as ExpandoObject);
            p.ThumbnailUri = p.BlobUri;

            if (!CanExecute(p)) return;

            // Create TumblrEntity
            TumblrEntity tumblrEntity = TableEntityFactory.CreateTumblrEntity(p);
            if (tumblrEntity == null) return;
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
        }
    }
}