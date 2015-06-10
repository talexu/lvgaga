using System.Threading.Tasks;
using LvModel.Azure.StorageTable;
using LvModel.View.Tumblr;
using LvService.Factories.Uri;

namespace LvService.Services
{
    public class CachedTumblrService : ITumblrService
    {
        private const string RegionOfTumblr = "tumblrmodel://";
        private const string RegionOfTumblrs = "tumblrsmodel://";

        private readonly ITumblrService _tumblrService;
        private readonly ICache _cache;
        private readonly ICacheKeyFactory _cacheKeyFactory;

        public CachedTumblrService(ITumblrService tumblrService, ICache cache, ICacheKeyFactory cacheKeyFactory)
        {
            _tumblrService = tumblrService;
            _cache = cache;
            _cacheKeyFactory = cacheKeyFactory;
        }

        public async Task<TumblrEntity> GetTumblrEntityAsync(string partitionKey, string rowKey)
        {
            return await _cache.Get(_cacheKeyFactory.CreateKey(RegionOfTumblr, partitionKey, rowKey),
                async () => await _tumblrService.GetTumblrEntityAsync(partitionKey, rowKey));
        }

        public async Task<TumblrsModel> GetTumblrsModelAsync(string partitionKey, TumblrCategory category, int takeCount)
        {
            return await _cache.Get(_cacheKeyFactory.CreateKey(RegionOfTumblrs, partitionKey, category.ToString("D")),
                async () => await _tumblrService.GetTumblrsModelAsync(partitionKey, category, takeCount));
        }
    }
}