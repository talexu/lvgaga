﻿using System;
using System.Collections.Generic;
using System.Linq;
using LvModel.Azure.StorageTable;
using LvModel.Common;
using LvService.Commands2.Lvgaga.Common;
using LvService.Utilities;
using Microsoft.WindowsAzure.Storage.Table;
using System.Threading.Tasks;

namespace LvService.Commands2.Lvgaga.Favlrite
{
    public class CreateFavoriteCommand : AbstractCreateLvEntityCommand
    {
        private string _mediaType;
        private string _rowKey;
        private string _mediaUri;
        private string _thumbnailUri;
        private string _text;
        private DateTime _createTime;

        public override bool CanExecute(dynamic p)
        {
            _mediaType = p.MediaType;
            _rowKey = p.RowKey;
            _mediaUri = p.MediaUri;
            _thumbnailUri = p.ThumbnailUri;
            _text = p.Text;
            _createTime = p.CreateTime;

            return new[] { _mediaType, _rowKey, _mediaUri, _thumbnailUri, _text }.AllNotNullOrEmpty() &&
                   _createTime != null;
        }

        public override System.Threading.Tasks.Task ExecuteAsync(dynamic p)
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