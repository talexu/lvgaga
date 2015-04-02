using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Commands.Azure.Storage.Table
{
    public interface ITableEntityCommand
    {
        bool CanExecute(dynamic p);
        Task<T> ExecuteAsync<T>(dynamic p) where T : ITableEntity, new();
    }
}