using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using LvService.Commands.Common;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Commands.Azure.Storage.Table
{
    public class CreateTableEntitiesCommand : TableCudBatchCommand
    {
        public CreateTableEntitiesCommand()
        {

        }

        public CreateTableEntitiesCommand(ICommand command)
            : base(command)
        {

        }

        public override async Task ExecuteAsync(dynamic p)
        {
            await base.ExecuteAsync(p as ExpandoObject);

            if (!CanExecute(p)) return;

            foreach (var group in Entities.GroupBy(e => e.PartitionKey))
            {
                var batchOperation = new TableBatchOperation();
                foreach (var entity in group)
                {
                    batchOperation.Insert(entity);
                }

                await Table.ExecuteBatchAsync(batchOperation);
            }
        }
    }
}