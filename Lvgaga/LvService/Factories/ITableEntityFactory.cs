using LvModel.Azure.StorageTable;

namespace LvService.Factories
{
    public interface ITableEntityFactory
    {
        TumblrEntity CreateTumblrEntity();
    }
}