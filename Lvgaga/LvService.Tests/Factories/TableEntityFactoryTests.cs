using System.Dynamic;
using LvModel.Azure.StorageTable;
using LvService.Factories.Azure.Storage;
using Xunit;

namespace LvService.Tests.Factories
{
    public class TableEntityFactoryTests
    {
        private readonly ITableEntityFactory _tableEntityFactory;

        public TableEntityFactoryTests()
        {
            _tableEntityFactory = new TableEntityFactory();
        }

        [Fact]
        public void CreateTumblrEntityTest()
        {
            dynamic p = new ExpandoObject();
            p.Text = "Test text";
            TumblrEntity data = _tableEntityFactory.CreateTumblrEntity(p);
            Assert.Equal(p.Text, data.Text);
        }
    }
}
