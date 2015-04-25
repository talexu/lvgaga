using System.Threading.Tasks;
using LvService.Commands.Common;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Commands.Azure.Storage.Table
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