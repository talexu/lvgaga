using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Commands.Azure.Storage.Table
{
    public class EntityTableCommandChain : IEntityTableCommand
    {
        public IEntityTableCommand NextCommand { get; set; }

        public EntityTableCommandChain()
        {

        }

        public EntityTableCommandChain(IEntityTableCommand command)
        {
            NextCommand = command;
        }

        public virtual bool CanExecute<T>(dynamic p) where T : ITableEntity, new()
        {
            return NextCommand != null && NextCommand.CanExecute<T>(p);
        }

        public virtual async Task<T> ExecuteAsync<T>(dynamic p) where T : ITableEntity, new()
        {
            if (NextCommand != null && NextCommand.CanExecute<T>(p)) return await NextCommand.ExecuteAsync<T>(p);
            return default(T);
        }

        public virtual async Task<List<T>> ExecutesAsync<T>(dynamic p) where T : ITableEntity, new()
        {
            if (NextCommand != null && NextCommand.CanExecute<T>(p)) return await NextCommand.ExecutesAsync<T>(p);
            return null;
        }
    }
}