using Microsoft.WindowsAzure.Storage.Table;
using System.Threading.Tasks;

namespace LvService.Commands2.Azure.Storage.Table
{
    public class DeleteTableEntityCommand : AbstractTableEntityCommand
    {
        public override async Task OperateAsync()
        {
            await Table.ExecuteAsync(TableOperation.Delete(Entity));
        }
    }
}