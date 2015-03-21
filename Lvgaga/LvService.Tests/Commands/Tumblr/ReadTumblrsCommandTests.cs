using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using LvModel.Common;
using LvModel.View.Tumblr;
using LvService.Commands.Tumblr;
using LvService.Factories;
using Xunit;

namespace LvService.Tests.Commands.Tumblr
{
    public class ReadTumblrsCommandTests : IClassFixture<AzureStorageFixture>
    {
        private readonly CreateTumblrCommand _createTumblrCommand;
        private readonly ReadTumblrsCommand _readTumblrsCommand;

        private readonly AzureStorageFixture _fixture;

        private readonly string _tableName;

        public ReadTumblrsCommandTests(AzureStorageFixture fixture)
        {
            _fixture = fixture;
            _createTumblrCommand = new CreateTumblrCommand
            {
                TableEntityFactory = new TableEntityFactory()
            };
            _readTumblrsCommand = new ReadTumblrsCommand();

            _tableName = Constants.TumblrTableName;
        }

        [Fact]
        public async Task ExecuteTest()
        {
            var tumblrTexts = GetTestTumblrTexts();
            foreach (var tumblrText in tumblrTexts)
            {
                dynamic cp = new ExpandoObject();
                cp.Table = await _fixture.AzureStorage.GetTableReferenceAsync(_tableName);
                cp.TumblrText = tumblrText;
                await _createTumblrCommand.ExecuteAsync(cp);
            }

            dynamic rp = new ExpandoObject();
            rp.Table = await _fixture.AzureStorage.GetTableReferenceAsync(_tableName);
            rp.PartitionKey = Constants.MediaTypeImage;
            await _readTumblrsCommand.ExecuteAsync(rp);
            Assert.Equal(20, rp.Results.Count);
        }

        private static IEnumerable<TumblrText> GetTestTumblrTexts()
        {
            ICollection<TumblrText> result = new List<TumblrText>();
            for (var i = 0; i < 30; i++)
            {
                result.Add(new TumblrText
                {
                    Text = Guid.NewGuid().ToString(),
                    Category = TumblrCategory.C1
                });
            }

            return result;
        }
    }
}
