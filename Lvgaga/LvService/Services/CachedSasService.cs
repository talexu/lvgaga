using System.Threading.Tasks;
using LvService.Factories.Uri;

namespace LvService.Services
{
    public class CachedSasService : ISasService
    {
        private const string RegionOfTableSas = "tablesas";

        private readonly ISasService _sasService;
        private readonly ICache _cache;
        private readonly ICacheKeyFactory _cacheKeyFactory;

        public CachedSasService(ISasService sasService, ICache cache, ICacheKeyFactory cacheKeyFactory)
        {
            _sasService = sasService;
            _cache = cache;
            _cacheKeyFactory = cacheKeyFactory;
        }

        public async Task<string> GetSasForTable(string tableName)
        {
            return await _cache.Get(_cacheKeyFactory.CreateKey(RegionOfTableSas, tableName),
                async () => await _sasService.GetSasForTable(tableName));
        }

        public async Task<string> GetSasForTable(string tableName, string partitionKey)
        {
            return await _cache.Get(_cacheKeyFactory.CreateKey(RegionOfTableSas, tableName, partitionKey),
                async () => await _sasService.GetSasForTable(tableName, partitionKey));
        }
    }
}