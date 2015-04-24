using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LvModel.Azure.StorageTable;
using LvModel.Common;
using LvModel.View.Tumblr;
using LvService.Commands2.Lvgaga.Common;
using LvService.Utilities;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Commands2.Lvgaga.Tumblr
{
    public class CreateTumblrCommand : AbstractCreateLvEntityCommand
    {
        protected string PartitionKey;
        protected string MediaUri;
        protected string ThumbnailUri;
        protected TumblrText TumblrText;

        public override bool CanExecute(dynamic p)
        {
            PartitionKey = p.PartitionKey;
            MediaUri = p.MediaUri;
            ThumbnailUri = p.ThumbnailUri;
            TumblrText = p.TumblrText;
            return new[] { PartitionKey, MediaUri, ThumbnailUri }.AllNotNullOrEmpty() && TumblrText != null;
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
            if (!TumblrText.Category.Equals(TumblrCategory.All))
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