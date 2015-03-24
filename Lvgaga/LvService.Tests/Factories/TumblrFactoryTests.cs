using LvService.Utilities;
using System.Threading.Tasks;
using Xunit;

namespace LvService.Tests.Factories
{
    public class TumblrFactoryTests
    {
        //[Fact]
        public async Task GetFakeTumblrModels()
        {
            var models = await FakeDataHelper.GetFakeTumblrModels();
        }
    }
}