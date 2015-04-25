using System;
using System.Collections.Generic;
using System.Dynamic;
using LvService.Tests.Utilities;
using System.Threading.Tasks;
using LvModel.Common;
using LvService.Commands.Common;
using LvService.Commands2.Azure.Storage.Table;
using LvService.Utilities;
using Microsoft.WindowsAzure.Storage.Table;
using Xunit;

namespace LvService.Tests.Commands.Azure.Storage.Table
{
    public class TableEntitiesCrudTests : IClassFixture<AzureStorageFixture>
    {
        private const string TablePrefix = "t";
        private readonly string _tempTableName;

        protected readonly AzureStorageFixture Fixture;

        public TableEntitiesCrudTests(AzureStorageFixture fixture)
        {
            Fixture = fixture;
            _tempTableName = TestDataGenerator.GetRandomName(TablePrefix);
        }

        [Fact]
        public async Task Crud()
        {
            ICommand createCommand = new CreateTableEntitiesCommand();
            ICommand readCommand = new ReadTableEntitiesCommand<TestTableEntity>();
            ICommand updateCommand = new UpdateTableEntitiesCommand();
            ICommand deleteCommand = new DeleteTableEntitiesCommand();
            ICommand deleteTableCommand = new DeleteTableCommand();

            var partitionKey = Guid.NewGuid().ToString();
            const int count = 5;
            var entities = new List<TestTableEntity>();
            for (var i = 0; i < count; i++)
            {
                entities.Add(new TestTableEntity
                {
                    PartitionKey = partitionKey,
                    RowKey = Guid.NewGuid().ToString(),
                    Text = i.ToString()
                });
            }

            var filter = TableQuery.GenerateFilterCondition(LvConstants.PartitionKey, QueryComparisons.Equal,
                partitionKey);

            // create
            dynamic c = await GetInitialExpandoObjectAsync();
            c.Entities = entities;
            await createCommand.ExecuteAsync(c);

            // read
            dynamic r = await GetInitialExpandoObjectAsync();
            r.Filter = filter;
            await readCommand.ExecuteAsync(r);
            List<TestTableEntity> entitiesR = r.Entities;
            Assert.True(entities.ToJsonString().CosineEqual(entitiesR.ToJsonString()));

            // update
            var entities2 = entities.CloneByJson();
            var updatedText = Guid.NewGuid().ToString();
            foreach (var entity in entities2)
            {
                entity.Text = updatedText;
            }
            dynamic u = await GetInitialExpandoObjectAsync();
            u.Entities = entities2;
            await updateCommand.ExecuteAsync(u);

            dynamic r2 = await GetInitialExpandoObjectAsync();
            r2.Filter = filter;
            await readCommand.ExecuteAsync(r2);
            List<TestTableEntity> entitiesR2 = r2.Entities;
            for (var i = 0; i < count; i++)
            {
                Assert.Equal(entities2[i].Text, entitiesR2[i].Text);
            }

            // delete
            dynamic d = await GetInitialExpandoObjectAsync();
            d.Entities = entities2;
            await deleteCommand.ExecuteAsync(d);

            dynamic r3 = await GetInitialExpandoObjectAsync();
            r3.Filter = filter;
            await readCommand.ExecuteAsync(r3);
            List<TestTableEntity> entitiesR3 = r3.Entities;
            Assert.Empty(entitiesR3);

            dynamic dt = await GetInitialExpandoObjectAsync();
            await deleteTableCommand.ExecuteAsync(dt);
        }

        private async Task<ExpandoObject> GetInitialExpandoObjectAsync()
        {
            dynamic d = new ExpandoObject();
            d.Table = await Fixture.AzureStorage.GetTableReferenceAsync(_tempTableName);

            return d;
        }
    }
}