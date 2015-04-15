using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.DbContexts
{
    public class AzureStorageDb : IAzureStorage
    {
        private readonly CloudBlobClient _blobClient;
        private readonly CloudTableClient _tableClient;

        public AzureStorageDb(CloudStorageAccount storageAccount)
        {
            storageAccount = storageAccount ??
                             CloudStorageAccount.Parse(
                                 WebConfigurationManager.ConnectionStrings["AzureStorageConnection"].ConnectionString);
            //storageAccount = CloudStorageAccount.DevelopmentStorageAccount;
            _blobClient = storageAccount.CreateCloudBlobClient();
            _tableClient = storageAccount.CreateCloudTableClient();

            InitializeCors(_blobClient, _tableClient);
        }

        /// <summary>
        /// Initialize Windows Azure Storage CORS settings.
        /// </summary>
        /// <param name="blobClient">Windows Azure storage blob client</param>
        /// <param name="tableClient">Windows Azure storage table client</param>
        private static void InitializeCors(CloudBlobClient blobClient, CloudTableClient tableClient)
        {
            // CORS should be enabled once at service startup
            var blobServiceProperties = new ServiceProperties();
            var tableServiceProperties = new ServiceProperties();

            // Nullifying un-needed properties so that we don't
            // override the existing ones
            blobServiceProperties.HourMetrics = null;
            tableServiceProperties.HourMetrics = null;
            blobServiceProperties.MinuteMetrics = null;
            tableServiceProperties.MinuteMetrics = null;
            blobServiceProperties.Logging = null;
            tableServiceProperties.Logging = null;

            // Enable and Configure CORS
            ConfigureCors(blobServiceProperties);
            ConfigureCors(tableServiceProperties);

            // Commit the CORS changes into the Service Properties
            blobClient.SetServiceProperties(blobServiceProperties);
            tableClient.SetServiceProperties(tableServiceProperties);
        }

        /// <summary>
        /// Adds CORS rule to the service properties.
        /// </summary>
        /// <param name="serviceProperties">ServiceProperties</param>
        private static void ConfigureCors(ServiceProperties serviceProperties)
        {
            serviceProperties.Cors = new CorsProperties();
            serviceProperties.Cors.CorsRules.Add(new CorsRule
            {
                AllowedHeaders = new List<string> { "*" },
                AllowedMethods = CorsHttpMethods.Get,
                AllowedOrigins = new List<string> { "*" },
                ExposedHeaders = new List<string> { "*" },
                MaxAgeInSeconds = 1800 // 30 minutes
            });
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