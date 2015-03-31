using System;
using System.Dynamic;
using System.Threading.Tasks;
using LvService.Commands.Common;
using LvService.Utilities;

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
            var favoriteEntity = TableEntityFactory.CreateFavoriteEntity(p);
            if (favoriteEntity == null) return;
            p.Entity = favoriteEntity;
        }
    }
}