using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LvModel.Azure.StorageTable;
using LvModel.Common;
using LvService.Commands.Lvgaga.Common;
using LvService.Utilities;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Commands.Lvgaga.Favlrite
{
    public class CreateFavoriteCommand : AbstractCreateLvEntityCommand
    {
        private string _mediaType;
        private string _rowKey;
        private string _mediaUri;
        private string _mediaLargeUri;
        private string _mediaMediumUri;
        private string _mediaSmallUri;
        private string _text;
        private DateTime _createTime;

        public override bool CanExecute(dynamic p)
        {
            _mediaType = p.MediaType;
            _rowKey = p.RowKey;
            _mediaUri = p.MediaUri;
            _mediaLargeUri = p.MediaLargeUri;
            _mediaMediumUri = p.MediaMediumUri;
            _mediaSmallUri = p.MediaSmallUri;
            _text = p.Text;
            _createTime = p.CreateTime;

            return new[] { _mediaType, _rowKey, _mediaUri, _mediaLargeUri, _mediaMediumUri, _mediaSmallUri, _text }.AllNotNullOrEmpty() &&
                   _createTime != null;
        }

        public override Task ExecuteAsync(dynamic p)
        {
            if (!CanExecute(p)) return Task.FromResult(false);

            // Create FavoriteEntity
            FavoriteEntity favoriteEntity = TableEntityFactory.CreateFavoriteEntity(p);
            if (favoriteEntity == null) return Task.FromResult(false);
            p.Entity = favoriteEntity;

            // Copies
            var entities = new List<FavoriteEntity> { favoriteEntity };
            if (!_mediaType.Equals(LvConstants.MediaTypeOfAll))
            {
                var copyEntity = favoriteEntity.CloneByJson();
                copyEntity.RowKey = UriFactory.ReplaceMediaTypeOfRowKey(favoriteEntity.RowKey, MediaType.All);
                entities.Add(copyEntity);
            }

            p.Entities = entities.Cast<ITableEntity>().ToList();
            return Task.FromResult(true);
        }
    }
}