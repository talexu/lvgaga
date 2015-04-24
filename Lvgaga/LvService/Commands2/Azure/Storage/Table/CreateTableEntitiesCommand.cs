using System.Linq;
using Microsoft.WindowsAzure.Storage.Table;
using System.Threading.Tasks;

namespace LvService.Commands2.Azure.Storage.Table
{
    public class CreateTableEntitiesCommand : AbstractTableEntitiesCommand
    {
        protected override void Operate(TableBatchOperation batchOperation, ITableEntity entity)
        {
            batchOperation.Insert(entity);
        }
    }
}