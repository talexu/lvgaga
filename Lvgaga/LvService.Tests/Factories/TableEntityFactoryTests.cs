using System.Dynamic;
using LvModel.Azure.StorageTable;
using LvModel.Common;
using LvModel.View.Tumblr;
using LvService.Factories.Azure.Storage;
using LvService.Factories.Uri;
using LvService.Utilities;
using Xunit;

namespace LvService.Tests.Factories
{
    public class TableEntityFactoryTests
    {
        private readonly ITableEntityFactory _tableEntityFactory;

        public TableEntityFactoryTests()
        {
            _tableEntityFactory = new TableEntityFactory(new UriFactory());
        }

        [Fact]
        public void CreateTumblrEntityTest()
        {
            dynamic p = new ExpandoObject();
            p.PartitionKey = LvConstants.PartitionKeyOfImage;
            p.MediaUri = "uri";
            p.ThumbnailUri = "thumb";
            p.TumblrText = new TumblrText
            {
                Text = "Test text",
                Category = TumblrCategory.C1
            };
            TumblrEntity data = _tableEntityFactory.CreateTumblrEntity(p);

            Assert.Equal(p.PartitionKey, data.PartitionKey);
            Assert.Equal(
                string.Format("{0}_{1}", p.TumblrText.Category.ToString("D"),
                    DateTimeHelper.GetInvertedTicks(data.CreateTime)), data.RowKey);
            Assert.Equal(p.MediaUri, data.MediaUri);
            Assert.Equal(p.ThumbnailUri, data.ThumbnailUri);
            Assert.Equal(p.TumblrText.Text, data.Text);
            Assert.Equal(EntityState.Active, data.State);
        }
    }
}
