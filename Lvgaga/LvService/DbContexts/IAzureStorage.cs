using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.DbContexts
{
    public interface IAzureStorage
    {
        Task<CloudTable> GetTableReferenceAsync(string tableName);
    }
}