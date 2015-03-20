using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Factories
{
    public interface ITableEntityFactory
    {
        ITableEntity CreateTumblrEntity(dynamic p);
    }
}