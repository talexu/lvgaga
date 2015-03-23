using System.Dynamic;
using System.Threading.Tasks;
using LvService.Commands.Common;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Commands.Azure.Storage.Table
{
    public class UpdateTableEntityCommand : TableCudCommand
    {
        public UpdateTableEntityCommand()
        {

        }

        public UpdateTableEntityCommand(ICommand command)
            : base(command)
        {

        }

        public override async Task ExecuteAsync(dynamic p)
        {
            if (!CanExecute(p)) return;
            await Table.ExecuteAsync(TableOperation.Replace(Entity));

            await base.ExecuteAsync(p as ExpandoObject);
        }
    }
}