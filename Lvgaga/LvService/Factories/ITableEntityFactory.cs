using LvModel.Azure.StorageTable;
using LvModel.View.Tumblr;

namespace LvService.Factories
{
    public interface ITableEntityFactory
    {
        TumblrEntity CreateTumblrEntity(TumblrViewModel tumblrViewModel);
    }
}