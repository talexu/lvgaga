using LvService.DbContexts;

namespace LvService.Tests.Commands.Azure.Storage
{
    public class AzureStorageFixture
    {
        public IAzureStorage AzureStorage { get; private set; }

        public AzureStorageFixture()
        {
            //AzureStorage = new AzureStorageDb();
            AzureStorage = new AzureStoragePool(new AzureStorageDb());
        }
    }
}