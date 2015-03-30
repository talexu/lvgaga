using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Factories.Azure.Storage
{
    public interface ITableEntityFactory
    {
        ITableEntity CreateTumblrEntity(dynamic p);
        ITableEntity CreateCommentEntity(dynamic p);
        ITableEntity CreateFavoriteEntity(dynamic p);
    }
}