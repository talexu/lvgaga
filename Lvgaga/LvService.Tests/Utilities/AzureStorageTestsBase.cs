using LvService.Tests.Commands.Azure.Storage;
using Xunit;

namespace LvService.Tests.Utilities
{
    public abstract class AzureStorageTestsBase : IClassFixture<AzureStorageFixture>
    {
        protected readonly AzureStorageFixture Fixture;

        protected AzureStorageTestsBase(AzureStorageFixture fixture)
        {
            Fixture = fixture;
        }
    }
}