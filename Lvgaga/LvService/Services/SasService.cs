using System;
using System.Globalization;
using System.Threading.Tasks;
using LvService.Common;
using LvService.DbContexts;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Services
{
    public class SasService : ISasService
    {
        private readonly IAzureStorage _azureStorage;

        public SasService(IAzureStorage azureStorage)
        {
            _azureStorage = azureStorage;
        }

        public async Task<string> GetSasForTable(string tableName)
        {
            if (String.IsNullOrEmpty(tableName)) return null;

            var table = await _azureStorage.GetTableReferenceAsync(tableName);
            if (table == null) return null;

            var sas = table.GetSharedAccessSignature(
                new SharedAccessTablePolicy
                {
                    Permissions = SharedAccessTablePermissions.Query,
                    SharedAccessExpiryTime = LvConfiguration.GetExpireTime(LvConfiguration.TokenExpireOffset)
                },
                null, /* accessPolicyIdentifier */
                null, /* startPartitionKey */
                null, /* startRowKey */
                null, /* endPartitionKey */
                null); /* endRowKey */

            return string.Format(CultureInfo.InvariantCulture, "{0}{1}", table.Uri, sas);
        }

        public async Task<string> GetSasForTable(string tableName, string partitionKey)
        {
            if (String.IsNullOrEmpty(tableName)) return null;

            var table = await _azureStorage.GetTableReferenceAsync(tableName);
            if (table == null) return null;

            var sas = table.GetSharedAccessSignature(
                new SharedAccessTablePolicy
                {
                    Permissions = SharedAccessTablePermissions.Query,
                    SharedAccessExpiryTime = LvConfiguration.GetExpireTime(LvConfiguration.CacheExpireOffset)
                },
                null, /* accessPolicyIdentifier */
                partitionKey, /* startPartitionKey */
                null, /* startRowKey */
                partitionKey, /* endPartitionKey */
                null); /* endRowKey */

            return string.Format(CultureInfo.InvariantCulture, "{0}{1}", table.Uri, sas);
        }
    }
}