using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using LvModel.Azure.StorageTable;
using LvModel.Common;
using LvModel.View.Tumblr;
using LvService.Commands.Common;
using LvService.Factories.Uri;
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
            try
            {
                _partitionKey = p.PartitionKey;
                _mediaUri = p.MediaUri;
                _thumbnailUri = p.ThumbnailUri;
                _tumblrText = p.TumblrText;
                return !String.IsNullOrEmpty(_partitionKey) &&
                       !String.IsNullOrEmpty(_mediaUri) &&
                       !String.IsNullOrEmpty(_thumbnailUri) &&
                       _tumblrText != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override async Task ExecuteAsync(dynamic p)
        {
            await base.ExecuteAsync(p as ExpandoObject);
            try
            {
                p.ThumbnailUri = p.BlobUri;
            }
            catch (Exception)
            {
                // ignored
            }

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
            if (!tumblrEntity.PartitionKey.Equals(LvConstants.PartitionKeyOfAll))
            {
                var copyEntities = entities.CloneByJson<TumblrEntity>();
                foreach (var copyEntity in copyEntities)
                {
                    copyEntity.PartitionKey = LvConstants.PartitionKeyOfAll;
                    entities.Add(copyEntity);
                }
            }

            p.Entities = entities.Cast<ITableEntity>().ToList();
        }
    }
}