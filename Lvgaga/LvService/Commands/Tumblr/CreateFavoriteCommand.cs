using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using LvModel.Azure.StorageTable;
using LvModel.Common;
using LvService.Commands.Common;
using LvService.Utilities;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Commands.Tumblr
{
    public class CreateFavoriteCommand : CreateLvEntityCommand
    {
        private string _partitionKey;
        private string _rowKey;
        private string _mediaUri;
        private string _thumbnailUri;
        private string _text;
        private DateTime _createTime;

        public CreateFavoriteCommand()
        {

        }

        public CreateFavoriteCommand(ICommand command)
            : base(command)
        {

        }

        public new bool CanExecute(dynamic p)
        {
            try
            {
                _partitionKey = p.PartitionKey;
                _rowKey = p.RowKey;
                _mediaUri = p.MediaUri;
                _thumbnailUri = p.ThumbnailUri;
                _text = p.Text;
                _createTime = p.CreateTime;

                return new[] { _partitionKey, _rowKey, _mediaUri, _thumbnailUri, _text }.AllNotNullOrEmpty() &&
                       _createTime != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override async Task ExecuteAsync(dynamic p)
        {
            await base.ExecuteAsync(p as ExpandoObject);

            if (!CanExecute(p)) return;

            // Create FavoriteEntity
            FavoriteEntity favoriteEntity = TableEntityFactory.CreateFavoriteEntity(p);
            if (favoriteEntity == null) return;
            p.Entity = favoriteEntity;

            // Copies
            var entities = new List<FavoriteEntity> { favoriteEntity };
            if (!_partitionKey.Equals(LvConstants.PartitionKeyOfAll))
            {
                var copyEntity = favoriteEntity.CloneByJson();
                copyEntity.RowKey = UriFactory.ReplaceMediaTypeOfRowKey(favoriteEntity.RowKey, MediaType.All);
                entities.Add(copyEntity);
            }

            p.Entities = entities.Cast<ITableEntity>().ToList();
        }
    }
}