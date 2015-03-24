using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using LvModel.Azure.StorageTable;
using LvModel.Common;
using LvModel.View.Tumblr;
using LvService.Commands.Azure.Storage.Table;
using LvService.Commands.Tumblr;
using LvService.Factories.Azure.Storage;
using LvService.Tests.Commands.Azure.Storage;
using LvService.Utilities;
using Microsoft.WindowsAzure.Storage.Table;
using Xunit;

namespace LvService.Tests.Commands.Tumblr
{
    public class CreateReadTumblrCommandTests : IClassFixture<AzureStorageFixture>
    {
        private readonly CreateTumblrCommand _createTumblrCommand;
        private readonly ReadTableEntityCommand _readTableEntityCommand;
        private readonly ReadTableEntitiesCommand _readTableEntitiesCommand;
        private readonly UpdateTableEntityCommand _updateTableEntityCommand;
        private readonly DeleteTableEntityCommand _deleteTableEntityCommand;

        private readonly AzureStorageFixture _fixture;

        private readonly string _tableName;

        public CreateReadTumblrCommandTests(AzureStorageFixture fixture)
        {
            _fixture = fixture;
            _tableName = Constants.TumblrTableName;

            // create
            _createTumblrCommand = new CreateTumblrCommand
            {
                TableEntityFactory = new TableEntityFactory(),
                NextCommand = new CreateTableEntityCommand()
            };

            // read
            _readTableEntityCommand = new ReadTableEntityCommand();
            _readTableEntitiesCommand = new ReadTableEntitiesCommand();

            // update
            _updateTableEntityCommand = new UpdateTableEntityCommand();

            // delete
            _deleteTableEntityCommand = new DeleteTableEntityCommand();
        }

        [Fact]
        public async Task CrudExecuteTest()
        {
            var table = await _fixture.AzureStorage.GetTableReferenceAsync(_tableName);

            // create entity
            dynamic cp = new ExpandoObject();
            cp.Table = table;
            cp.TumblrText = GetTestTumblrText();
            await _createTumblrCommand.ExecuteAsync(cp);
            TumblrEntity entity = cp.Entity;

            // read entity
            dynamic rp = new ExpandoObject();
            rp.Table = table;
            rp.PartitionKey = entity.PartitionKey;
            rp.RowKey = entity.RowKey;
            TumblrEntity entityR = await _readTableEntityCommand.ExecuteAsync<TumblrEntity>(rp);
            Assert.True(entity.ToJsonString().CosineEqual(entityR.ToJsonString()));

            // update entity
            entityR.Text = "Changed text";
            dynamic up = new ExpandoObject();
            up.Table = table;
            up.Entity = entityR;
            await _updateTableEntityCommand.ExecuteAsync(up);
            TumblrEntity entityUr = await _readTableEntityCommand.ExecuteAsync<TumblrEntity>(rp);
            Assert.True(entityR.Text.Equals(entityUr.Text));

            // delete entity
            dynamic dp = new ExpandoObject();
            dp.Table = table;
            dp.Entity = entityUr;
            _deleteTableEntityCommand.ExecuteAsync(dp);
            TumblrEntity entityD = await _readTableEntityCommand.ExecuteAsync<TumblrEntity>(dp);
            Assert.Null(entityD);
        }

        [Fact]
        public async Task ReadEntitiesTest()
        {
            const int count = 30;
            const int pageSize = 20;
            var table = await _fixture.AzureStorage.GetTableReferenceAsync(_tableName);
            var texts = GetTestTumblrTexts(count);
            ICollection<TumblrEntity> entities = new List<TumblrEntity>(count);

            // create test entities
            foreach (dynamic cp in texts.Select(tumblrText => new ExpandoObject()))
            {
                cp.Table = table;
                cp.TumblrText = GetTestTumblrText();
                await _createTumblrCommand.ExecuteAsync(cp);
                TumblrEntity entity = cp.Entity;
                entities.Add(entity);
            }

            // read entities with page size
            dynamic rp = new ExpandoObject();
            rp.Table = table;
            rp.Filter = TableQuery.GenerateFilterCondition(Constants.PartitionKey, QueryComparisons.Equal,
                Constants.MediaTypeImage);
            rp.TakeCount = pageSize;
            var entitiesR = await _readTableEntitiesCommand.ExecuteAsync<TumblrEntity>(rp);
            Assert.Equal(pageSize, entitiesR.Count);

            // delete all test entities
            foreach (var tumblrEntity in entities)
            {
                dynamic dp = new ExpandoObject();
                dp.Table = table;
                dp.Entity = tumblrEntity;
                await _deleteTableEntityCommand.ExecuteAsync(dp);
            }

            var entitiesRd = await _readTableEntitiesCommand.ExecuteAsync<TumblrEntity>(rp);
            Assert.Equal(0, entitiesRd.Count);
        }

        private static TumblrText GetTestTumblrText()
        {
            return new TumblrText
            {
                Text = Guid.NewGuid().ToString(),
                Category = TumblrCategory.C1
            };
        }

        private static IEnumerable<TumblrText> GetTestTumblrTexts(int count)
        {
            ICollection<TumblrText> results = new List<TumblrText>(count);
            for (var i = 0; i < count; i++)
            {
                results.Add(GetTestTumblrText());
            }

            return results;
        }
    }
}
