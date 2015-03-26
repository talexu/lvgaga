using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.DbContexts
{
    public class AzureStorageDb : IAzureStorage
    {
        private readonly CloudBlobClient _blobClient;
        private readonly CloudTableClient _tableClient;

        public AzureStorageDb()
        {
            //StorageAccount =
            //    CloudStorageAccount.Parse(
            //       WebConfigurationManager.ConnectionStrings["AzureStorageConnection"].ConnectionString);
            var storageAccount = CloudStorageAccount.DevelopmentStorageAccount;
            _blobClient = storageAccount.CreateCloudBlobClient();
            _tableClient = storageAccount.CreateCloudTableClient();
        }

        public virtual async Task<CloudTable> GetTableReferenceAsync(string tableName)
        {
            var table = _tableClient.GetTableReference(tableName);
            await table.CreateIfNotExistsAsync();

            return table;
        }

        public virtual async Task<CloudBlobContainer> GetContainerReferenceAsync(string containerName)
        {
            // Retrieve a reference to a container. 
            var container = _blobClient.GetContainerReference(containerName);
            // Create the container if it doesn't already exist.
            await container.CreateIfNotExistsAsync();
            container.SetPermissions(new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            });

            return container;
        }
    }
}