using LvService.DbContexts;

namespace LvService.Tests.Commands.Tumblr
{
    public class AzureStorageFixture
    {
        public IAzureStorage AzureStorage { get; private set; }

        public AzureStorageFixture()
        {
            AzureStorage = new AzureStorageDb();
        }
    }
}