using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using LvService.Tests.Utilities;
using Microsoft.WindowsAzure.Storage.Table;
using Xunit;

namespace LvService.Tests.Commands.Azure.Storage.Table
{
    public class CrdTableEntitiesCommandTests : AzureStorageTestsBase
    {
        private readonly string _tempTableName;

        public CrdTableEntitiesCommandTests(AzureStorageFixture fixture)
            : base(fixture)
        {
            _tempTableName = fixture.GetRandomTableName();
        }

        [Fact]
        public async Task CanExecute_Return_True_ProvidingTableAndEntities()
        {
            dynamic p = new ExpandoObject();
            p.Table = await Fixture.AzureStorage.GetTableReferenceAsync(_tempTableName);
            p.Entities = TestDataGenerator.GetTableEntities();
            Assert.True(Fixture.DeleteTableEntitiesCommand.CanExecute(p));

            await Fixture.DeleteTableCommand.ExecuteAsync(p);
        }
        [Fact]
        public void CanExecute_Return_False_WithoutTable()
        {
            dynamic p = new ExpandoObject();
            p.Entities = TestDataGenerator.GetTableEntities();
            Assert.False(Fixture.DeleteTableEntitiesCommand.CanExecute(p));
        }
        [Fact]
        public async Task CanExecute_Return_False_WithoutEntity()
        {
            dynamic p = new ExpandoObject();
            p.Table = await Fixture.AzureStorage.GetTableReferenceAsync(_tempTableName);
            Assert.False(Fixture.DeleteTableEntitiesCommand.CanExecute(p));

            await Fixture.DeleteTableCommand.ExecuteAsync(p);
        }
        [Fact]
        public async Task CanExecute_Return_False_WithEnptyEntity()
        {
            dynamic p = new ExpandoObject();
            p.Table = await Fixture.AzureStorage.GetTableReferenceAsync(_tempTableName);
            p.Entities = TestDataGenerator.GetTableEntities(0);
            Assert.False(Fixture.DeleteTableEntitiesCommand.CanExecute(p));

            await Fixture.DeleteTableCommand.ExecuteAsync(p);
        }
        [Fact]
        public void CanExecute_Return_False_WithoutTableAndEntity()
        {
            dynamic p = new ExpandoObject();
            Assert.False(Fixture.DeleteTableEntitiesCommand.CanExecute(p));
        }

        [Fact]
        public async Task ExecuteAsync()
        {
            var partitionKey = Guid.NewGuid().ToString();
            const int size = 20;
            var entities = TestDataGenerator.GetTableEntities(size, partitionKey);

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
            IEnumerable<TableEntity> entitiesR = await Fixture.ReadTableEntitiesCommand.ExecuteAsync<TableEntity>(pr);
            Assert.NotNull(entitiesR);
            Assert.Equal(size, entitiesR.Count());

            // delete
            dynamic pd = new ExpandoObject();
            pd.Table = await Fixture.AzureStorage.GetTableReferenceAsync(_tempTableName);
            pd.Entities = entities;
            await Fixture.DeleteTableEntitiesCommand.ExecuteAsync(pd);

            dynamic pr2 = new ExpandoObject();
            pr2.Table = await Fixture.AzureStorage.GetTableReferenceAsync(_tempTableName);
            pr2.Filter = Fixture.GetTableFilterByPartitionKey(partitionKey);
            pr2.TakeCount = size;
            IEnumerable<TableEntity> entitiesR2 = await Fixture.ReadTableEntitiesCommand.ExecuteAsync<TableEntity>(pr2);
            Assert.Empty(entitiesR2);

            await Fixture.DeleteTableCommand.ExecuteAsync(pc);
        }
    }
}