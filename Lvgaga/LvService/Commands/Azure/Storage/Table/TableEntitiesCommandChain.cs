using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Commands.Azure.Storage.Table
{
    public class TableEntitiesCommandChain : ITableEntitiesCommand
    {
        public ITableEntitiesCommand NextCommand { get; set; }

        public TableEntitiesCommandChain()
        {

        }

        public TableEntitiesCommandChain(ITableEntitiesCommand command)
        {
            NextCommand = command;
        }

        public virtual bool CanExecute<T>(dynamic p) where T : ITableEntity, new()
        {
            return NextCommand != null && NextCommand.CanExecute<T>(p);
        }

        public virtual async Task<List<T>> ExecuteAsync<T>(dynamic p) where T : ITableEntity, new()
        {
            if (NextCommand != null && NextCommand.CanExecute<T>(p)) return await NextCommand.ExecuteAsync<T>(p);
            return null;
        }
    }
}