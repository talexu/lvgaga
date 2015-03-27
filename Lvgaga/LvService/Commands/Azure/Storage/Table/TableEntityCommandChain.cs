using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Commands.Azure.Storage.Table
{
    public class TableEntityCommandChain : ITableEntityCommand
    {
        public ITableEntityCommand PreviousCommand { get; set; }

        public TableEntityCommandChain(ITableEntityCommand command = null)
        {
            PreviousCommand = command;
        }

        public bool CanExecute<T>(dynamic p) where T : ITableEntity, new()
        {
            return PreviousCommand != null;
        }

        public virtual async Task<T> ExecuteAsync<T>(dynamic p) where T : ITableEntity, new()
        {
            if (CanExecute<T>(p)) return await PreviousCommand.ExecuteAsync<T>(p);
            return default(T);
        }
    }
}