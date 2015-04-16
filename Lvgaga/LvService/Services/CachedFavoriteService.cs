using System.Collections.Generic;
using System.Threading.Tasks;
using LvModel.Azure.StorageTable;
using LvService.Factories.Uri;

namespace LvService.Services
{
    public class CachedFavoriteService : IFavoriteService
    {
        private const string RegionOfFavorite = "favoriteentity";

        private readonly IFavoriteService _favoriteService;
        private readonly ICache _cache;
        private readonly ICacheKeyFactory _cacheKeyFactory;

        public CachedFavoriteService(IFavoriteService favoriteService, ICache cache, ICacheKeyFactory cacheKeyFactory)
        {
            _favoriteService = favoriteService;
            _cache = cache;
            _cacheKeyFactory = cacheKeyFactory;
        }

        public async Task<FavoriteEntity> CreateFavoriteAsync(string userId, string partitionKey, string rowKey)
        {
            return await _favoriteService.CreateFavoriteAsync(userId, partitionKey, rowKey);
        }

        public async Task<List<FavoriteEntity>> GetFavoriteTumblrModelsAsync(string userId, string mediaType,
            string from, string to)
        {
            return
                await _cache.Get(_cacheKeyFactory.CreateKey(RegionOfFavorite, userId, mediaType, from, to),
                    async () => await _favoriteService.GetFavoriteTumblrModelsAsync(userId, mediaType, from, to));
        }

        public async Task<List<FavoriteEntity>> GetFavoriteTumblrModelsAsync(string userId, string mediaType,
            int takeCount)
        {
            return
                await _cache.Get(_cacheKeyFactory.CreateKey(RegionOfFavorite, userId, mediaType, takeCount.ToString()),
                    async () => await _favoriteService.GetFavoriteTumblrModelsAsync(userId, mediaType, takeCount));
        }

        public async Task DeleteFavoriteAsync(string userId, string partitionKey, string rowKey)
        {
            await _favoriteService.DeleteFavoriteAsync(userId, partitionKey, rowKey);
        }
    }
}