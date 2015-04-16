using System.IO;
using System.Threading.Tasks;
using LvService.Services;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.DbContexts
{
    public class AzureStoragePool : IAzureStorage
    {
        private const string BlobPrefix = "storageblob://";
        private const string TablePrefix = "storagetable://";

        private readonly IAzureStorage _azureStorage;
        private readonly ICache _cache;

        public AzureStoragePool(IAzureStorage azureStorage, ICache cache)
        {
            _azureStorage = azureStorage;
            _cache = cache;
        }

        public async Task<CloudTable> GetTableReferenceAsync(string tableName)
        {
            var key = Path.Combine(TablePrefix, tableName);
            var table = _cache.Get<CloudTable>(key);
            if (table != null) return table;

            table = await _azureStorage.GetTableReferenceAsync(tableName);
            _cache.Set(key, table);
            return table;
        }

        public async Task<CloudBlobContainer> GetContainerReferenceAsync(string containerName)
        {
            var key = Path.Combine(TablePrefix, containerName);
            var container = _cache.Get<CloudBlobContainer>(key);
            if (container != null) return container;

            container = await _azureStorage.GetContainerReferenceAsync(containerName);
            _cache.Set(key, container);
            return container;
        }
    }
}