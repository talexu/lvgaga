using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Commands.Azure.Storage.Table
{
    public class TableEntityCommandChain : ITableEntityCommand
    {
        public ITableEntityCommand NextCommand { get; set; }

        public TableEntityCommandChain()
        {

        }

        public TableEntityCommandChain(ITableEntityCommand command)
        {
            NextCommand = command;
        }

        public bool CanExecute<T>(dynamic p) where T : ITableEntity, new()
        {
            return NextCommand != null;
        }

        public virtual async Task<T> ExecuteAsync<T>(dynamic p) where T : ITableEntity, new()
        {
            if (CanExecute<T>(p)) return await NextCommand.ExecuteAsync<T>(p);
            return default(T);
        }
    }
}