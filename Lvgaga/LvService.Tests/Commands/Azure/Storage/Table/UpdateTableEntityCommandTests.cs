using System.Dynamic;
using System.Threading.Tasks;
using LvModel.Azure.StorageTable;
using LvService.Tests.Utilities;
using Microsoft.WindowsAzure.Storage.Table;
using Xunit;

namespace LvService.Tests.Commands.Azure.Storage.Table
{
    public class UpdateTableEntityCommandTests : AzureStorageTestsBase
    {
        private readonly string _tempTableName;

        public UpdateTableEntityCommandTests(AzureStorageFixture fixture)
            : base(fixture)
        {
            _tempTableName = fixture.GetRandomName(AzureStorageFixture.TablePrefix);
        }

        [Fact]
        public async Task CanExecute_Return_True_ProvidingTableAndEntity()
        {
            dynamic p = new ExpandoObject();
            p.Table = await Fixture.AzureStorage.GetTableReferenceAsync(_tempTableName);
            p.Entity = new TableEntity();
            Assert.True(Fixture.UpdateTableEntityCommand.CanExecute(p));

            await Fixture.DeleteTableCommand.ExecuteAsync(p);
        }
        [Fact]
        public void CanExecute_Return_False_WithoutTable()
        {
            dynamic p = new ExpandoObject();
            p.Entity = new TableEntity();
            Assert.False(Fixture.UpdateTableEntityCommand.CanExecute(p));
        }
        [Fact]
        public async Task CanExecute_Return_False_WithoutEntity()
        {
            dynamic p = new ExpandoObject();
            p.Table = await Fixture.AzureStorage.GetTableReferenceAsync(_tempTableName);
            Assert.False(Fixture.UpdateTableEntityCommand.CanExecute(p));

            await Fixture.DeleteTableCommand.ExecuteAsync(p);
        }
        [Fact]
        public void CanExecute_Return_False_WithoutTableAndEntity()
        {
            dynamic p = new ExpandoObject();
            Assert.False(Fixture.UpdateTableEntityCommand.CanExecute(p));
        }

        [Fact]
        public async Task ExecuteAsync()
        {
            var entity = TestDataGenerator.GetTumblrEntity();

            // create
            dynamic pc = new ExpandoObject();
            pc.Table = await Fixture.AzureStorage.GetTableReferenceAsync(_tempTableName);
            pc.Entity = entity;
            await Fixture.CreateTableEntityCommand.ExecuteAsync(pc);

            // read
            dynamic pr = new ExpandoObject();
            pr.Table = await Fixture.AzureStorage.GetTableReferenceAsync(_tempTableName);
            pr.PartitionKey = entity.PartitionKey;
            pr.RowKey = entity.RowKey;
            TumblrEntity entityR = await Fixture.ReadTableEntityCommand.ExecuteAsync<TumblrEntity>(pr);
            Assert.NotNull(entityR);
            Assert.Equal(entity.PartitionKey, entityR.PartitionKey);
            Assert.Equal(entity.RowKey, entityR.RowKey);
            Assert.Equal(entity.MediaUri, entityR.MediaUri);
            Assert.Equal(entity.Text, entityR.Text);

            // update
            entity.MediaUri = "modified media uri";
            entity.Text = "modified text";
            dynamic pu = new ExpandoObject();
            pu.Table = await Fixture.AzureStorage.GetTableReferenceAsync(_tempTableName);
            pu.Entity = entity;
            await Fixture.UpdateTableEntityCommand.ExecuteAsync(pu);

            dynamic pr2 = new ExpandoObject();
            pr2.Table = await Fixture.AzureStorage.GetTableReferenceAsync(_tempTableName);
            pr2.PartitionKey = entity.PartitionKey;
            pr2.RowKey = entity.RowKey;
            TumblrEntity entityR2 = await Fixture.ReadTableEntityCommand.ExecuteAsync<TumblrEntity>(pr2);
            Assert.NotNull(entityR2);
            Assert.Equal(entity.PartitionKey, entityR2.PartitionKey);
            Assert.Equal(entity.RowKey, entityR2.RowKey);
            Assert.Equal(entity.MediaUri, entityR2.MediaUri);
            Assert.Equal(entity.Text, entityR2.Text);

            // delete
            dynamic pd = new ExpandoObject();
            pd.Table = await Fixture.AzureStorage.GetTableReferenceAsync(_tempTableName);
            pd.Entity = entity;
            await Fixture.DeleteTableEntityCommand.ExecuteAsync(pd);

            dynamic pr3 = new ExpandoObject();
            pr3.Table = await Fixture.AzureStorage.GetTableReferenceAsync(_tempTableName);
            pr3.PartitionKey = entity.PartitionKey;
            pr3.RowKey = entity.RowKey;
            TumblrEntity entityR3 = await Fixture.ReadTableEntityCommand.ExecuteAsync<TumblrEntity>(pr3);
            Assert.Null(entityR3);

            await Fixture.DeleteTableCommand.ExecuteAsync(pc);
        }
    }
}