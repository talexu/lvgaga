using System.Dynamic;
using System.Threading.Tasks;
using LvService.Commands.Common;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Commands.Azure.Storage.Table
{
    public class CreateTableEntityCommand : TableCudCommand
    {
        public CreateTableEntityCommand()
        {

        }

        public CreateTableEntityCommand(ICommand command)
            : base(command)
        {

        }

        public override async Task ExecuteAsync(dynamic p)
        {
            if (!CanExecute(p)) return;
            await Table.ExecuteAsync(TableOperation.Insert(Entity));

            await base.ExecuteAsync(p as ExpandoObject);
        }
    }
}