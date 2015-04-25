using LvService.DbContexts;
using Microsoft.WindowsAzure.Storage;

namespace LvService.Tests.Utilities
{
    public class AzureStorageFixture
    {
        public IAzureStorage AzureStorage { get; private set; }

        public AzureStorageFixture()
        {
            AzureStorage = new AzureStorageDb(CloudStorageAccount.DevelopmentStorageAccount);
        }
    }
}