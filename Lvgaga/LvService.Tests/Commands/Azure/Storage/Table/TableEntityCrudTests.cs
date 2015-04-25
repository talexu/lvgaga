using System;
using System.Dynamic;
using System.Threading.Tasks;
using LvService.Commands.Common;
using LvService.Commands2.Azure.Storage.Table;
using LvService.DbContexts;
using LvService.Tests.Utilities;
using LvService.Utilities;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Xunit;

namespace LvService.Tests.Commands.Azure.Storage.Table
{
    public class TableEntityCrudTests : IClassFixture<AzureStorageFixture>
    {
        private const string TablePrefix = "t";
        private readonly string _tempTableName;

        protected readonly AzureStorageFixture Fixture;

        public TableEntityCrudTests(AzureStorageFixture fixture)
        {
            Fixture = fixture;
            _tempTableName = TestDataGenerator.GetRandomName(TablePrefix);
        }

        [Fact]
        public async Task Crud()
        {
            ICommand createCommand = new CreateTableEntityCommand();
            ICommand readCommand = new ReadTableEntityCommand<TestTableEntity>();
            ICommand updateCommand = new UpdateTableEntityCommand();
            ICommand deleteCommand = new DeleteTableEntityCommand();
            ICommand deleteTableCommand = new DeleteTableCommand();

            var partitionKey = Guid.NewGuid().ToString();
            var rowKey = Guid.NewGuid().ToString();
            var text = Guid.NewGuid().ToString();

            var entity = new TestTableEntity
            {
                PartitionKey = partitionKey,
                RowKey = rowKey,
                Text = text
            };

            // create
            dynamic c = await GetInitialExpandoObjectAsync();
            c.Entity = entity;
            await createCommand.ExecuteAsync(c);

            // read
            dynamic r = await GetInitialExpandoObjectAsync();
            r.PartitionKey = partitionKey;
            r.RowKey = rowKey;
            await readCommand.ExecuteAsync(r);
            TestTableEntity entityR = r.Entity;
            Assert.Equal(entity.PartitionKey, entityR.PartitionKey);
            Assert.Equal(entity.RowKey, entityR.RowKey);
            Assert.Equal(entity.Text, entityR.Text);

            // update
            var text2 = Guid.NewGuid().ToString();
            var entity2 = entity.CloneByJson();
            entity2.Text = text2;
            dynamic u = await GetInitialExpandoObjectAsync();
            u.Entity = entity2;
            await updateCommand.ExecuteAsync(u);

            dynamic r2 = await GetInitialExpandoObjectAsync();
            r2.PartitionKey = partitionKey;
            r2.RowKey = rowKey;
            await readCommand.ExecuteAsync(r);
            TestTableEntity entityR2 = r.Entity;
            Assert.Equal(entity2.PartitionKey, entityR2.PartitionKey);
            Assert.Equal(entity2.RowKey, entityR2.RowKey);
            Assert.Equal(entity2.Text, entityR2.Text);

            // delete
            dynamic d = await GetInitialExpandoObjectAsync();
            d.Entity = entity2;
            await deleteCommand.ExecuteAsync(d);
            dynamic r3 = await GetInitialExpandoObjectAsync();
            r3.PartitionKey = partitionKey;
            r3.RowKey = rowKey;
            await readCommand.ExecuteAsync(r);
            TestTableEntity entityR3 = r.Entity;
            Assert.Null(entityR3);

            dynamic dt = await GetInitialExpandoObjectAsync();
            await deleteTableCommand.ExecuteAsync(dt);
        }

        private async Task<ExpandoObject> GetInitialExpandoObjectAsync()
        {
            dynamic u = new ExpandoObject();
            u.Table = await Fixture.AzureStorage.GetTableReferenceAsync(_tempTableName);

            return u;
        }
    }

    public class TestTableEntity : TableEntity
    {
        public string Text { get; set; }
    }
}