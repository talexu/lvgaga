using System.Collections.Generic;
using System.Threading.Tasks;
using LvModel.Azure.StorageTable;

namespace LvService.Services
{
    public interface IFavoriteService
    {
        Task<FavoriteEntity> CreateFavoriteAsync(string userId, string partitionKey, string rowKey);
        Task<List<FavoriteEntity>> GetFavoriteTumblrModelsAsync(string userId, string mediaType, string from, string to);
        Task<List<FavoriteEntity>> GetFavoriteTumblrModelsAsync(string userId, string mediaType, int takeCount);
        Task DeleteFavoriteAsync(string userId, string partitionKey, string rowKey);
    }
}