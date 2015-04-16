using System.Threading.Tasks;
using LvService.Factories.Uri;
using LvService.Services;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.DbContexts
{
    public class AzureStoragePool : IAzureStorage
    {
        private const string RegionOfBlob = "storageblob://";
        private const string RegionOfTable = "storagetable://";

        private readonly IAzureStorage _azureStorage;
        private readonly ICache _cache;
        private readonly ICacheKeyFactory _cacheKeyFactory;

        public AzureStoragePool(IAzureStorage azureStorage, ICache cache, ICacheKeyFactory cacheKeyFactory)
        {
            _azureStorage = azureStorage;
            _cache = cache;
            _cacheKeyFactory = cacheKeyFactory;
        }

        public async Task<CloudTable> GetTableReferenceAsync(string tableName)
        {
            return await _cache.Get(_cacheKeyFactory.CreateKey(RegionOfTable, tableName),
                async () => await _azureStorage.GetTableReferenceAsync(tableName));
        }

        public async Task<CloudBlobContainer> GetContainerReferenceAsync(string containerName)
        {
            return await _cache.Get(_cacheKeyFactory.CreateKey(RegionOfBlob, containerName),
                async () => await _azureStorage.GetContainerReferenceAsync(containerName));
        }
    }
}