using LvService.Commands.Common;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Commands2.Azure.Storage.Table
{
    public class CreateTableEntityCommand : AbstractTableEntityCommand
    {
        public override async Task OperateAsync()
        {
            await Table.ExecuteAsync(TableOperation.Insert(Entity));
        }
    }
}