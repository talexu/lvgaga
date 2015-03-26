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
using LvService.Tests.Utilities;
using LvService.Utilities;
using Microsoft.WindowsAzure.Storage.Table;
using Xunit;

namespace LvService.Tests.Commands.Tumblr
{
    public class CreateReadTumblrCommandTests : IClassFixture<AzureStorageFixture>
    {
        private readonly CreateTumblrCommand _createTumblrCommand;
        private readonly CreateTableEntitiesCommand _createTableEntitiesCommand;
        private readonly ReadTableEntityCommand _readTableEntityCommand;
        private readonly ReadTableEntitiesCommand _readTableEntitiesCommand;
        private readonly ReadTumblrEntityWithCategoryCommand _readTumblrEntityWithCategoryCommand;
        private readonly UpdateTableEntityCommand _updateTableEntityCommand;
        private readonly DeleteTableEntityCommand _deleteTableEntityCommand;
        private readonly DeleteTableEntitiesCommand _deleteTableEntitiesCommand;

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
            _createTableEntitiesCommand = new CreateTableEntitiesCommand();

            // read
            _readTableEntityCommand = new ReadTableEntityCommand();
            _readTableEntitiesCommand = new ReadTableEntitiesCommand();
            _readTumblrEntityWithCategoryCommand = new ReadTumblrEntityWithCategoryCommand(_readTableEntitiesCommand);

            // update
            _updateTableEntityCommand = new UpdateTableEntityCommand();

            // delete
            _deleteTableEntityCommand = new DeleteTableEntityCommand();
            _deleteTableEntitiesCommand = new DeleteTableEntitiesCommand();
        }

        [Fact]
        public async Task TestCreateTumblrWithAllCategory()
        {
            var table = await _fixture.AzureStorage.GetTableReferenceAsync(_tableName);
            var createTumblrCommand = new CreateTumblrCommand(new CreateTableEntitiesCommand())
            {
                TableEntityFactory = new TableEntityFactory()
            };

            // create entity
            dynamic cp = new ExpandoObject();
            cp.PartitionKey = Constants.ImagePartitionKey;
            cp.MediaUri = "http://www.caoliu.com/1024.jpg";
            cp.Table = table;
            cp.TumblrText = GetTestTumblrText();
            await createTumblrCommand.ExecuteAsync(cp);
            TumblrEntity entity = cp.Entity;
            IEnumerable<ITableEntity> entities = cp.Entities;

            Assert.NotNull(entity);
            Assert.NotNull(entities);
            Assert.Equal(2, entities.Count());

            Assert.Equal(cp.PartitionKey, entity.PartitionKey);
            Assert.Equal(cp.MediaUri, entity.MediaUri);
            Assert.Equal(cp.TumblrText.Text, entity.Text);

            // read entity
            dynamic rp = new ExpandoObject();
            rp.Table = table;
            rp.PartitionKey = entity.PartitionKey;
            rp.RowKey = entity.RowKey;
            TumblrEntity entityR = await _readTableEntityCommand.ExecuteAsync<TumblrEntity>(rp);
            Assert.NotNull(entityR);

            rp.RowKey = TumblrCategory.All.ToString("D") + entity.RowKey.Substring(1);
            TumblrEntity entityRa = await _readTableEntityCommand.ExecuteAsync<TumblrEntity>(rp);
            Assert.NotNull(entityRa);

            // delete entity
            dynamic dp = new ExpandoObject();
            dp.Table = table;
            dp.Entities = entities;
            await _deleteTableEntitiesCommand.ExecuteAsync(dp);
        }

        [Fact]
        public async Task CrudExecuteTest()
        {
            var table = await _fixture.AzureStorage.GetTableReferenceAsync(_tableName);

            // create entity
            dynamic cp = new ExpandoObject();
            cp.PartitionKey = Constants.ImagePartitionKey;
            cp.MediaUri = "http://www.caoliu.com/1024.jpg";
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
                cp.PartitionKey = Constants.ImagePartitionKey;
                cp.MediaUri = "http://www.caoliu.com/1024.jpg";
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
                Constants.ImagePartitionKey);
            rp.TakeCount = pageSize;
            var entitiesR = await _readTableEntitiesCommand.ExecuteAsync<TumblrEntity>(rp);
            Assert.Equal(pageSize, entitiesR.Count);

            // read entities with category
            dynamic rpc = new ExpandoObject();
            rpc.Table = table;
            rpc.PartitionKey = Constants.ImagePartitionKey;
            rpc.Category = TumblrCategory.C1;
            rpc.TakeCount = pageSize;
            var entitiesRc = await _readTumblrEntityWithCategoryCommand.ExecuteAsync<TumblrEntity>(rpc);
            Assert.Equal(pageSize, entitiesRc.Count);

            rpc.Category = TumblrCategory.All;
            var entitiesRa = await _readTumblrEntityWithCategoryCommand.ExecuteAsync<TumblrEntity>(rpc);
            Assert.Equal(0, entitiesRa.Count);

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

        [Fact]
        public async Task BatchCreateTableEntitiesTest()
        {
            var table = await _fixture.AzureStorage.GetTableReferenceAsync(_tableName);

            // Create
            var entityFactory = new TableEntityFactory();
            dynamic p = new ExpandoObject();
            p.PartitionKey = Constants.ImagePartitionKey;
            p.MediaUri = "http://www.caoliu.com/1024.jpg";
            p.TumblrText = GetTestTumblrText();
            TumblrEntity entity1 = entityFactory.CreateTumblrEntity(p);

            p.MediaUri = "http://www.caoliu.com/2048.jpg";
            p.TumblrText = GetTestTumblrText();
            TumblrEntity entity2 = entityFactory.CreateTumblrEntity(p);

            dynamic pb = new ExpandoObject();
            pb.Table = table;
            pb.Entities = new[] { entity1, entity2 };
            await _createTableEntitiesCommand.ExecuteAsync(pb);

            // Read
            dynamic rp = new ExpandoObject();
            rp.Table = table;
            rp.PartitionKey = entity1.PartitionKey;
            rp.RowKey = entity1.RowKey;
            TumblrEntity entityR1 = await _readTableEntityCommand.ExecuteAsync<TumblrEntity>(rp);
            Assert.Equal(entity1.RowKey, entityR1.RowKey);
            Assert.Equal(entity1.MediaUri, entityR1.MediaUri);
            Assert.Equal(entity1.Text, entityR1.Text);

            rp = new ExpandoObject();
            rp.Table = table;
            rp.PartitionKey = entity2.PartitionKey;
            rp.RowKey = entity2.RowKey;
            TumblrEntity entityR2 = await _readTableEntityCommand.ExecuteAsync<TumblrEntity>(rp);
            Assert.Equal(entity2.RowKey, entityR2.RowKey);
            Assert.Equal(entity2.MediaUri, entityR2.MediaUri);
            Assert.Equal(entity2.Text, entityR2.Text);

            // Delete
            dynamic dp = new ExpandoObject();
            dp.Table = table;
            dp.Entity = entity1;
            _deleteTableEntityCommand.ExecuteAsync(dp);
            TumblrEntity entityD1 = await _readTableEntityCommand.ExecuteAsync<TumblrEntity>(dp);
            Assert.Null(entityD1);

            dp = new ExpandoObject();
            dp.Table = table;
            dp.Entity = entity2;
            _deleteTableEntityCommand.ExecuteAsync(dp);
            TumblrEntity entityD2 = await _readTableEntityCommand.ExecuteAsync<TumblrEntity>(dp);
            Assert.Null(entityD2);
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
