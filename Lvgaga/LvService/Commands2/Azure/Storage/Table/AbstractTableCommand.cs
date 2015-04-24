using LvService.Commands.Common;
using Microsoft.WindowsAzure.Storage.Table;
using System.Threading.Tasks;

namespace LvService.Commands2.Azure.Storage.Table
{
    public abstract class AbstractTableCommand : ICommand
    {
        protected CloudTable Table;

        public bool CanExecute(dynamic p)
        {
            Table = p.Table;
            return Table != null;
        }

        public abstract Task ExecuteAsync(dynamic p);
    }
}