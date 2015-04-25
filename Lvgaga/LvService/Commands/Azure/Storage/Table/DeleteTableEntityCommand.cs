using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Commands.Azure.Storage.Table
{
    public class DeleteTableEntityCommand : AbstractTableEntityCommand
    {
        public override async Task OperateAsync()
        {
            await Table.ExecuteAsync(TableOperation.Delete(Entity));
        }
    }
}