using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.DbContexts
{
    public class AzureStorageDb : IAzureStorage
    {
        public CloudStorageAccount StorageAccount;
        public CloudTableClient TableClient;

        public AzureStorageDb()
        {
            //StorageAccount =
            //    CloudStorageAccount.Parse(
            //       WebConfigurationManager.ConnectionStrings["AzureStorageConnection"].ConnectionString);
            StorageAccount = CloudStorageAccount.DevelopmentStorageAccount;
            TableClient = StorageAccount.CreateCloudTableClient();
        }

        public virtual async Task<CloudTable> GetTableReferenceAsync(string tableName)
        {
            var table = TableClient.GetTableReference(tableName);
            await table.CreateIfNotExistsAsync();

            return table;
        }
    }
}