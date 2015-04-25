using System.Collections.Generic;
using System.Threading.Tasks;
using LvModel.Common;
using LvService.Commands.Lvgaga.Common;
using LvService.Utilities;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Commands.Lvgaga.Favlrite
{
    public class DeleteFavoriteCommand : AbstractCreateLvEntityCommand
    {
        private string _userId;
        private string _mediaType;
        private string _rowKey;

        public override bool CanExecute(dynamic p)
        {
            _userId = p.UserId;
            _mediaType = p.MediaType;
            _rowKey = p.RowKey;

            return new[] { _userId, _mediaType, _rowKey }.AllNotNullOrEmpty();
        }

        public override Task ExecuteAsync(dynamic p)
        {
            if (!CanExecute(p)) return Task.FromResult(false);

            var partitionKey = _userId;
            var rowKey = UriFactory.CreateFavoriteRowKey(_mediaType, _rowKey);
            // Create FavoriteEntity
            ITableEntity favoriteEntity = new TableEntity(partitionKey, rowKey) { ETag = "*" };
            p.Entity = favoriteEntity;

            // Copies
            var entities = new List<ITableEntity> { favoriteEntity };
            if (!_mediaType.Equals(LvConstants.MediaTypeOfAll))
            {
                entities.Add(new TableEntity(partitionKey, UriFactory.ReplaceMediaTypeOfRowKey(rowKey, MediaType.All)) { ETag = "*" });
            }

            p.Entities = entities;
            return Task.FromResult(true);
        }
    }
}