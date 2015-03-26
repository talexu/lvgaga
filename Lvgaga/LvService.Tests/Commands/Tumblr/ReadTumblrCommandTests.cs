using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using LvModel.Azure.StorageTable;
using LvModel.View.Tumblr;
using LvService.Tests.Utilities;
using Xunit;

namespace LvService.Tests.Commands.Tumblr
{
    public class ReadTumblrCommandTests : AzureStorageTestsBase
    {
        private readonly string _tempTableName;

        public ReadTumblrCommandTests(AzureStorageFixture fixture)
            : base(fixture)
        {
            _tempTableName = fixture.GetRandomName(AzureStorageFixture.TablePrefix);
        }

        [Fact]
        public async Task GetTumblrEntities_Return_Top()
        {
            const string partitionKey = "TestPartitionKey";
            const int total = 20;
            const int takeCount = 10;

            var entities = TestDataGenerator.GetTumblrEntities(total, partitionKey);

            dynamic p = new ExpandoObject();
            p.Table = await Fixture.AzureStorage.GetTableReferenceAsync(_tempTableName);
            p.Entities = entities;
            await Fixture.CreateTableEntitiesCommand.ExecuteAsync(p);

            dynamic pr1 = new ExpandoObject();
            pr1.Table = await Fixture.AzureStorage.GetTableReferenceAsync(_tempTableName);
            pr1.TakeCount = takeCount;
            pr1.PartitionKey = partitionKey;
            pr1.Category = TumblrCategory.C1;
            List<TumblrEntity> entitiesR = await Fixture.ReadTumblrEntityWithCategoryCommand.ExecuteAsync<TumblrEntity>(pr1);
            Assert.Equal(takeCount, entitiesR.Count);

            dynamic pr2 = new ExpandoObject();
            pr2.Table = await Fixture.AzureStorage.GetTableReferenceAsync(_tempTableName);
            pr2.TakeCount = takeCount;
            pr2.PartitionKey = partitionKey;
            pr2.Category = TumblrCategory.C2;
            List<TumblrEntity> entitiesR2 = await Fixture.ReadTumblrEntityWithCategoryCommand.ExecuteAsync<TumblrEntity>(pr2);
            Assert.Equal(0, entitiesR2.Count);

            dynamic pr3 = new ExpandoObject();
            pr3.Table = await Fixture.AzureStorage.GetTableReferenceAsync(_tempTableName);
            pr3.TakeCount = takeCount;
            pr3.PartitionKey = partitionKey;
            pr3.Category = TumblrCategory.All;
            List<TumblrEntity> entitiesR3 = await Fixture.ReadTumblrEntityWithCategoryCommand.ExecuteAsync<TumblrEntity>(pr3);
            Assert.Equal(0, entitiesR3.Count);

            // delete
            dynamic pd = new ExpandoObject();
            pd.Table = await Fixture.AzureStorage.GetTableReferenceAsync(_tempTableName);
            pd.Entities = entities;
            await Fixture.DeleteTableEntitiesCommand.ExecuteAsync(pd);

            await Fixture.DeleteTableCommand.ExecuteAsync(pd);
        }
    }
}