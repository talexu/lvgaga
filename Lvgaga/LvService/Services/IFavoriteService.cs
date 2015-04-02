using System.Collections.Generic;
using System.Threading.Tasks;
using LvModel.Azure.StorageTable;
using LvModel.Common;

namespace LvService.Services
{
    public interface IFavoriteService
    {
        Task<FavoriteEntity> CreateFavoriteAsync(string userId, string partitionKey, string rowKey);
        Task<List<FavoriteEntity>> GetFavoriteTumblrModelsAsync(string userId, MediaType mediaType, int takeCount);
        Task DeleteFavoriteAsync(string userId, string partitionKey, string rowKey);
    }
}