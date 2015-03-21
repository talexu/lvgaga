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

        public virtual CloudTable GetTableReference(string tableName)
        {
            var table = TableClient.GetTableReference(tableName);
            table.CreateIfNotExists();

            return table;
        }
    }
}