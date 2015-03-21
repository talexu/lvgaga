using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.DbContexts
{
    public interface IAzureStorage
    {
        CloudTable GetTableReference(string tableName);
    }
}