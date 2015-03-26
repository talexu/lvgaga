using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using LvModel.Azure.StorageTable;
using LvService.Tests.Utilities;
using Xunit;

namespace LvService.Tests.Commands.Azure.Storage.Table
{
    public class UpdateTableEntitiesCommandTests : AzureStorageTestsBase
    {
        private readonly string _tempTableName;

        public UpdateTableEntitiesCommandTests(AzureStorageFixture fixture)
            : base(fixture)
        {
            _tempTableName = fixture.GetRandomName(AzureStorageFixture.TablePrefix);
        }

        [Fact]
        public async Task CanExecute_Return_True_ProvidingTableAndEntities()
        {
            dynamic p = new ExpandoObject();
            p.Table = await Fixture.AzureStorage.GetTableReferenceAsync(_tempTableName);
            p.Entities = TestDataGenerator.GetTableEntities();
            Assert.True(Fixture.UpdateTableEntitiesCommand.CanExecute(p));

            await Fixture.DeleteTableCommand.ExecuteAsync(p);
        }
        [Fact]
        public void CanExecute_Return_False_WithoutTable()
        {
            dynamic p = new ExpandoObject();
            p.Entities = TestDataGenerator.GetTableEntities();
            Assert.False(Fixture.UpdateTableEntitiesCommand.CanExecute(p));
        }
        [Fact]
        public async Task CanExecute_Return_False_WithoutEntity()
        {
            dynamic p = new ExpandoObject();
            p.Table = await Fixture.AzureStorage.GetTableReferenceAsync(_tempTableName);
            Assert.False(Fixture.UpdateTableEntitiesCommand.CanExecute(p));

            await Fixture.DeleteTableCommand.ExecuteAsync(p);
        }
        [Fact]
        public async Task CanExecute_Return_False_WithEnptyEntity()
        {
            dynamic p = new ExpandoObject();
            p.Table = await Fixture.AzureStorage.GetTableReferenceAsync(_tempTableName);
            p.Entities = TestDataGenerator.GetTableEntities(0);
            Assert.False(Fixture.UpdateTableEntitiesCommand.CanExecute(p));

            await Fixture.DeleteTableCommand.ExecuteAsync(p);
        }
        [Fact]
        public void CanExecute_Return_False_WithoutTableAndEntity()
        {
            dynamic p = new ExpandoObject();
            Assert.False(Fixture.UpdateTableEntitiesCommand.CanExecute(p));
        }

        [Fact]
        public async Task ExecuteAsync()
        {
            var partitionKey = Guid.NewGuid().ToString();
            const int size = 20;
            const string modifiedMediaUri = "modified media uri";
            const string modifiedText = "modified text";
            var entities = TestDataGenerator.GetTumblrEntities(size, partitionKey);

            // create
            dynamic pc = new ExpandoObject();
            pc.Table = await Fixture.AzureStorage.GetTableReferenceAsync(_tempTableName);
            pc.Entities = entities;
            await Fixture.CreateTableEntitiesCommand.ExecuteAsync(pc);

            // read
            dynamic pr = new ExpandoObject();
            pr.Table = await Fixture.AzureStorage.GetTableReferenceAsync(_tempTableName);
            pr.Filter = Fixture.GetTableFilterByPartitionKey(partitionKey);
            pr.TakeCount = size;
            IEnumerable<TumblrEntity> entitiesR = await Fixture.ReadTableEntitiesCommand.ExecuteAsync<TumblrEntity>(pr);
            Assert.NotNull(entitiesR);
            Assert.Equal(size, entitiesR.Count());

            // update
            foreach (var entity in entities)
            {
                entity.MediaUri = modifiedMediaUri;
                entity.Text = modifiedText;
            }
            dynamic pu = new ExpandoObject();
            pu.Table = await Fixture.AzureStorage.GetTableReferenceAsync(_tempTableName);
            pu.Entities = entities;
            await Fixture.UpdateTableEntitiesCommand.ExecuteAsync(pu);

            dynamic pr2 = new ExpandoObject();
            pr2.Table = await Fixture.AzureStorage.GetTableReferenceAsync(_tempTableName);
            pr2.Filter = Fixture.GetTableFilterByPartitionKey(partitionKey);
            pr2.TakeCount = size;
            IEnumerable<TumblrEntity> entitiesR2 = await Fixture.ReadTableEntitiesCommand.ExecuteAsync<TumblrEntity>(pr2);
            Assert.NotNull(entitiesR2);
            Assert.Equal(size, entitiesR2.Count());
            foreach (var entity in entitiesR2)
            {
                Assert.Equal(modifiedMediaUri, entity.MediaUri);
                Assert.Equal(modifiedText, entity.Text);
            }

            // delete
            dynamic pd = new ExpandoObject();
            pd.Table = await Fixture.AzureStorage.GetTableReferenceAsync(_tempTableName);
            pd.Entities = entities;
            await Fixture.DeleteTableEntitiesCommand.ExecuteAsync(pd);

            dynamic pr3 = new ExpandoObject();
            pr3.Table = await Fixture.AzureStorage.GetTableReferenceAsync(_tempTableName);
            pr3.Filter = Fixture.GetTableFilterByPartitionKey(partitionKey);
            pr3.TakeCount = size;
            IEnumerable<TumblrEntity> entitiesR3 = await Fixture.ReadTableEntitiesCommand.ExecuteAsync<TumblrEntity>(pr3);
            Assert.Empty(entitiesR3);

            await Fixture.DeleteTableCommand.ExecuteAsync(pc);
        }
    }
}