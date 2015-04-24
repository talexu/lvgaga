using System.Dynamic;
using System.Threading.Tasks;

namespace LvService.Commands2.Azure.Storage.Table
{
    public class DeleteTableCommand : AbstractTableCommand
    {
        public override async Task ExecuteAsync(dynamic p)
        {
            if (!CanExecute(p as ExpandoObject)) return;
            await Table.DeleteIfExistsAsync();
        }
    }
}