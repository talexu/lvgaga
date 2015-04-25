using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Commands.Azure.Storage.Table
{
    public class UpdateTableEntityCommand : AbstractTableEntityCommand
    {
        public override async Task OperateAsync()
        {
            await Table.ExecuteAsync(TableOperation.Replace(Entity));
        }
    }
}