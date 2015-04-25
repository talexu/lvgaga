using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Commands.Azure.Storage.Table
{
    public class DeleteTableEntitiesCommand : AbstractTableEntitiesCommand
    {
        protected override void Operate(TableBatchOperation batchOperation, ITableEntity entity)
        {
            batchOperation.Delete(entity);
        }
    }
}