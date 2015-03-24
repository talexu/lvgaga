using System.Threading.Tasks;
using LvService.Utilities;

namespace LvService.Tests.Factories
{
    public class TumblrFactoryTests
    {
        //[Fact]
        public async Task GetFakeTumblrModels()
        {
            var models = await FakeDataHelper.GetFakeTumblrModelsAsync();
        }
    }
}