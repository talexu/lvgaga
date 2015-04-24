using System.Dynamic;
using Microsoft.WindowsAzure.Storage.Table;
using System.Threading.Tasks;

namespace LvService.Commands2.Azure.Storage.Table
{
    public abstract class AbstractTableEntityCommand : AbstractTableCommand
    {
        protected ITableEntity Entity;

        public new bool CanExecute(dynamic p)
        {
            if (!base.CanExecute(p as ExpandoObject)) return false;

            Entity = p.Entity;
            return Entity != null;
        }

        public override async Task ExecuteAsync(dynamic p)
        {
            if (!CanExecute(p)) return;
            await OperateAsync();
        }

        public abstract Task OperateAsync();
    }
}