using System.Dynamic;
using System.Threading.Tasks;
using LvService.Tests.Utilities;
using Microsoft.WindowsAzure.Storage.Table;
using Xunit;

namespace LvService.Tests.Commands.Azure.Storage.Table
{
    public class DeleteTableEntityCommandTests : AzureStorageTestsBase
    {
        private readonly string _tempTableName;

        public DeleteTableEntityCommandTests(AzureStorageFixture fixture)
            : base(fixture)
        {
            _tempTableName = fixture.GetRandomTableName();
        }

        [Fact]
        public async Task CanExecute_Return_True_ProvidingTableAndEntity()
        {
            dynamic p = new ExpandoObject();
            p.Table = await Fixture.AzureStorage.GetTableReferenceAsync(_tempTableName);
            p.Entity = new TableEntity();
            Assert.True(Fixture.DeleteTableEntityCommand.CanExecute(p));

            await Fixture.DeleteTableCommand.ExecuteAsync(p);
        }
        [Fact]
        public void CanExecute_Return_False_WithoutTable()
        {
            dynamic p = new ExpandoObject();
            p.Entity = new TableEntity();
            Assert.False(Fixture.DeleteTableEntityCommand.CanExecute(p));
        }
        [Fact]
        public async Task CanExecute_Return_False_WithoutEntity()
        {
            dynamic p = new ExpandoObject();
            p.Table = await Fixture.AzureStorage.GetTableReferenceAsync(_tempTableName);
            Assert.False(Fixture.DeleteTableEntityCommand.CanExecute(p));

            await Fixture.DeleteTableCommand.ExecuteAsync(p);
        }
        [Fact]
        public void CanExecute_Return_False_WithoutTableAndEntity()
        {
            dynamic p = new ExpandoObject();
            Assert.False(Fixture.DeleteTableEntityCommand.CanExecute(p));
        }

        [Fact]
        public async Task ExecuteAsync()
        {
            var entity = TestDataGenerator.GetTableEntity();

            dynamic pc = new ExpandoObject();
            pc.Table = await Fixture.AzureStorage.GetTableReferenceAsync(_tempTableName);
            pc.Entity = entity;
            await Fixture.CreateTableEntityCommand.ExecuteAsync(pc);

            dynamic pr = new ExpandoObject();
            pr.Table = await Fixture.AzureStorage.GetTableReferenceAsync(_tempTableName);
            pr.PartitionKey = entity.PartitionKey;
            pr.RowKey = entity.RowKey;
            TableEntity entityR = await Fixture.ReadTableEntityCommand.ExecuteAsync<TableEntity>(pr);
            Assert.NotNull(entityR);
            Assert.Equal(entity.PartitionKey, entityR.PartitionKey);
            Assert.Equal(entity.RowKey, entityR.RowKey);

            dynamic pd = new ExpandoObject();
            pd.Table = await Fixture.AzureStorage.GetTableReferenceAsync(_tempTableName);
            pd.Entity = entity;
            await Fixture.DeleteTableEntityCommand.ExecuteAsync(pd);

            dynamic pr2 = new ExpandoObject();
            pr2.Table = await Fixture.AzureStorage.GetTableReferenceAsync(_tempTableName);
            pr2.PartitionKey = entity.PartitionKey;
            pr2.RowKey = entity.RowKey;
            TableEntity entityR2 = await Fixture.ReadTableEntityCommand.ExecuteAsync<TableEntity>(pr);
            Assert.Null(entityR2);

            await Fixture.DeleteTableCommand.ExecuteAsync(pc);
        }
    }
}