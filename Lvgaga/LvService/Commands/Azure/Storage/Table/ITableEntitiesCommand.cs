using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Commands.Azure.Storage.Table
{
    public interface ITableEntitiesCommand
    {
        bool CanExecute<T>(dynamic p) where T : ITableEntity, new();
        Task<List<T>> ExecuteAsync<T>(dynamic p) where T : ITableEntity, new();
    }
}