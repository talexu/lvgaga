using System.Collections.Generic;
using System.Threading.Tasks;
using LvModel.Azure.StorageTable;
using LvModel.Common;
using LvModel.View.Tumblr;

namespace LvService.Services
{
    public interface IFavoriteService
    {
        Task<FavoriteEntity> CreateFavoriteAsync(string userId, TumblrModel tumblr);
        Task<List<TumblrModel>> GetFavoriteTumblrModelsAsync(string userId, MediaType mediaType, int takeCount);
    }
}