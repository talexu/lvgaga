using System.Dynamic;
using System.Threading.Tasks;
using LvService.Commands.Common;

namespace LvService.Commands.Azure.Storage.Table
{
    public class DeleteTableCommand : TableCommand
    {
        public DeleteTableCommand(ICommand command = null)
            : base(command)
        {

        }

        public override async Task ExecuteAsync(dynamic p)
        {
            await base.ExecuteAsync(p as ExpandoObject);

            if (!CanExecute(p as ExpandoObject)) return;

            await Table.DeleteIfExistsAsync();
        }
    }
}