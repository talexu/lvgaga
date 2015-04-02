using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Commands.Azure.Storage.Table
{
    public class TableEntityCommandChain : ITableEntityCommand
    {
        public ITableEntityCommand PreviousCommand { get; set; }

        public TableEntityCommandChain()
        {

        }

        public TableEntityCommandChain(ITableEntityCommand command)
        {
            PreviousCommand = command;
        }

        public bool CanExecute(dynamic p)
        {
            return PreviousCommand != null;
        }

        public virtual async Task<T> ExecuteAsync<T>(dynamic p) where T : ITableEntity, new()
        {
            if (CanExecute(p)) return await PreviousCommand.ExecuteAsync<T>(p);
            return default(T);
        }
    }
}