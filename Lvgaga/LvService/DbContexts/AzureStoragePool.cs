using System.Collections.Generic;
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

        public CloudTable GetTableReference(string tableName)
        {
            CloudTable table;
            if (_cloudTables.TryGetValue(tableName, out table)) return table;

            table = _azureStorage.GetTableReference(tableName);
            _cloudTables.TryAddValue(tableName, table);
            return table;
        }
    }
}