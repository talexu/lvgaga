using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Table;
using System.Threading.Tasks;

namespace LvService.Commands2.Azure.Storage.Table
{
    public abstract class AbstractTableEntitiesCommand : AbstractTableCommand
    {
        protected IEnumerable<ITableEntity> Entities;

        public new bool CanExecute(dynamic p)
        {
            if (!base.CanExecute(p as ExpandoObject)) return false;

            Entities = p.Entities;
            return Entities != null && Entities.Any();
        }

        public override async Task ExecuteAsync(dynamic p)
        {
            if (!CanExecute(p)) return;

            foreach (var group in Entities.GroupBy(e => e.PartitionKey))
            {
                var batchOperation = new TableBatchOperation();
                foreach (var entity in group)
                {
                    Operate(batchOperation, entity);
                }

                await Table.ExecuteBatchAsync(batchOperation);
            }
        }

        protected abstract void Operate(TableBatchOperation batchOperation, ITableEntity entity);
    }
}