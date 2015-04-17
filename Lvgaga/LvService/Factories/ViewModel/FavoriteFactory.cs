using System.Collections.Generic;
using System.Linq;
using LvModel.Azure.StorageTable;
using LvModel.View.Favorite;

namespace LvService.Factories.ViewModel
{
    public class FavoriteFactory : IFavoriteFactory
    {
        private readonly ITumblrFactory _tumblrFactory;

        public FavoriteFactory(ITumblrFactory tumblrFactory)
        {
            _tumblrFactory = tumblrFactory;
        }

        public FavoriteModel CreateFavoriteModel(FavoriteEntity favoriteEntity)
        {
            if (favoriteEntity == null) return null;

            favoriteEntity.PartitionKey = favoriteEntity.MediaType;
            var tumblrs = _tumblrFactory.CreateTumblrModel(favoriteEntity);
            return new FavoriteModel()
            {
                PartitionKey = tumblrs.PartitionKey,
                RowKey = tumblrs.RowKey,
                Id = tumblrs.Id,
                Uri = tumblrs.Uri,
                MediaType = tumblrs.MediaType,
                MediaUri = tumblrs.MediaUri,
                ThumbnailUri = tumblrs.ThumbnailUri,
                TumblrCategory = tumblrs.TumblrCategory,
                Text = tumblrs.Text,
                CreateTime = tumblrs.CreateTime,
                IsFavorited = true
            };
        }

        public List<FavoriteModel> CreateFavoriteModels(IEnumerable<FavoriteEntity> favoriteEntities)
        {
            return favoriteEntities == null ? null : favoriteEntities.Select(CreateFavoriteModel).ToList();
        }
    }
}