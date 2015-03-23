using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Commands.Azure.Storage.Table
{
    public interface IEntityTableCommand
    {
        bool CanExecute<T>(dynamic p) where T : ITableEntity, new();
        Task<T> ExecuteAsync<T>(dynamic p) where T : ITableEntity, new();
        Task<List<T>> ExecutesAsync<T>(dynamic p) where T : ITableEntity, new();
    }
}