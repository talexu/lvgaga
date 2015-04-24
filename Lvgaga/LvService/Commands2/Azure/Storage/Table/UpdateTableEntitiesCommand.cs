using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Commands2.Azure.Storage.Table
{
    public class UpdateTableEntitiesCommand : AbstractTableEntitiesCommand
    {
        protected override void Operate(TableBatchOperation batchOperation, ITableEntity entity)
        {
            batchOperation.Replace(entity);
        }
    }
}