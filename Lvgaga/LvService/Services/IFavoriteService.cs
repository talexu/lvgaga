using System.Threading.Tasks;
using LvModel.Azure.StorageTable;

namespace LvService.Services
{
    public interface IFavoriteService
    {
        Task<FavoriteEntity> CreateFavoriteAsync(string userId, string partitionKey, string rowKey);
        Task DeleteFavoriteAsync(string userId, string partitionKey, string rowKey);
    }
}