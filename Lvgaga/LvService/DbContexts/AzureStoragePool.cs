using System.Collections.Generic;
using System.Threading.Tasks;
using LvService.Utilities;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.DbContexts
{
    public class AzureStoragePool : IAzureStorage
    {
        private readonly IAzureStorage _azureStorage;
        private readonly IDictionary<string, CloudTable> _cloudTables;
        private readonly IDictionary<string, CloudBlobContainer> _cloudBlobContainers;

        public AzureStoragePool(IAzureStorage azureStorage)
        {
            _azureStorage = azureStorage;
            _cloudTables = new Dictionary<string, CloudTable>();
            _cloudBlobContainers = new Dictionary<string, CloudBlobContainer>();
        }

        public async Task<CloudTable> GetTableReferenceAsync(string tableName)
        {
            CloudTable table;
            if (_cloudTables.TryGetValue(tableName, out table)) return table;

            table = await _azureStorage.GetTableReferenceAsync(tableName);
            _cloudTables.AddOrUpdateValue(tableName, table);
            return table;
        }


        public async Task<CloudBlobContainer> GetContainerReferenceAsync(string containerName)
        {
            CloudBlobContainer container;
            if (_cloudBlobContainers.TryGetValue(containerName, out container)) return container;

            container = await _azureStorage.GetContainerReferenceAsync(containerName);
            _cloudBlobContainers.AddOrUpdateValue(containerName, container);
            return container;
        }
    }
}