using System.Collections.Generic;
using System.Threading.Tasks;
using LvService.Utilities;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.DbContexts
{
    public class AzureStoragePool : IAzureStorage
    {
        private readonly IAzureStorage _azureStorage;
        private readonly IDictionary<string, CloudTable> _cloudTables;

        public AzureStoragePool(IAzureStorage azureStorage)
        {
            _azureStorage = azureStorage;
            _cloudTables = new Dictionary<string, CloudTable>();
        }

        public async Task<CloudTable> GetTableReferenceAsync(string tableName)
        {
            CloudTable table;
            if (_cloudTables.TryGetValue(tableName, out table)) return table;

            table = await _azureStorage.GetTableReferenceAsync(tableName);
            _cloudTables.AddOrUpdateValue(tableName, table);
            return table;
        }
    }
}