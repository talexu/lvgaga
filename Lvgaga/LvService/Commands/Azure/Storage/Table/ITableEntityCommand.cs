using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Commands.Azure.Storage.Table
{
    public interface ITableEntityCommand
    {
        bool CanExecute<T>(dynamic p) where T : ITableEntity, new();
        Task<T> ExecuteAsync<T>(dynamic p) where T : ITableEntity, new();
    }
}