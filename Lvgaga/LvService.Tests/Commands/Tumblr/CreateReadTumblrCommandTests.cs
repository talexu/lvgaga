using System;
using System.Dynamic;
using System.Threading.Tasks;
using LvModel.Azure.StorageTable;
using LvModel.Common;
using LvModel.View.Tumblr;
using LvService.Commands.Tumblr;
using LvService.Factories;
using Xunit;

namespace LvService.Tests.Commands.Tumblr
{
    public class CreateReadTumblrCommandTests : IClassFixture<AzureStorageFixture>
    {
        private readonly CreateTumblrCommand _createTumblrCommand;
        private readonly ReadTumblrCommand _readTumblrCommand;

        private readonly AzureStorageFixture _fixture;

        private readonly string _tableName;

        public CreateReadTumblrCommandTests(AzureStorageFixture fixture)
        {
            _fixture = fixture;
            _createTumblrCommand = new CreateTumblrCommand
            {
                TableEntityFactory = new TableEntityFactory()
            };
            _readTumblrCommand = new ReadTumblrCommand();

            _tableName = Constants.TumblrTableName;
        }

        [Fact]
        public void CanExecuteTest_Return_False_Null()
        {
            Assert.False(_createTumblrCommand.CanExecute(null));
        }

        [Fact]
        public async Task CanExecuteTest_Return_True()
        {
            dynamic p = new ExpandoObject();
            p.Table = await _fixture.AzureStorage.GetTableReferenceAsync(_tableName);
            p.TumblrText = new TumblrText
            {
                Text = "Test text",
                Category = TumblrCategory.C1
            };

            Assert.True(_createTumblrCommand.CanExecute(p));
        }

        [Fact]
        public async Task ExecuteTest()
        {
            dynamic cp = new ExpandoObject();
            cp.Table = await _fixture.AzureStorage.GetTableReferenceAsync(_tableName);
            cp.TumblrText = GetTestTumblrText();
            await _createTumblrCommand.ExecuteAsync(cp);
            TumblrEntity entity = cp.TableEntity;

            dynamic rp = new ExpandoObject();
            rp.Table = await _fixture.AzureStorage.GetTableReferenceAsync(_tableName);
            rp.PartitionKey = entity.PartitionKey;
            rp.RowKey = entity.RowKey;
            await _readTumblrCommand.ExecuteAsync(rp);
            TumblrEntity entity2 = rp.Result;

            Assert.Equal(entity, entity2);
        }

        private static TumblrText GetTestTumblrText()
        {
            return new TumblrText
            {
                Text = Guid.NewGuid().ToString(),
                Category = TumblrCategory.C1
            };
        }
    }
}
