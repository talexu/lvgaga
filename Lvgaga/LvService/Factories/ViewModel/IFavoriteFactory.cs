using System.Collections.Generic;
using LvModel.Azure.StorageTable;
using LvModel.View.Favorite;

namespace LvService.Factories.ViewModel
{
    public interface IFavoriteFactory
    {
        FavoriteModel CreateFavoriteModel(FavoriteEntity favoriteEntity);
        List<FavoriteModel> CreateFavoriteModels(IEnumerable<FavoriteEntity> favoriteEntities);
    }
}