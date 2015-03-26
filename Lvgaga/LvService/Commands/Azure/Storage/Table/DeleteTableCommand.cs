using System.Dynamic;
using System.Threading.Tasks;
using LvService.Commands.Common;

namespace LvService.Commands.Azure.Storage.Table
{
    public class DeleteTableCommand : TableCommand
    {
        public DeleteTableCommand()
        {

        }

        public DeleteTableCommand(ICommand command)
            : base(command)
        {

        }

        public override async Task ExecuteAsync(dynamic p)
        {
            if (!CanExecute(p as ExpandoObject)) return;
            await Table.DeleteIfExistsAsync();

            await base.ExecuteAsync(p as ExpandoObject);
        }
    }
}