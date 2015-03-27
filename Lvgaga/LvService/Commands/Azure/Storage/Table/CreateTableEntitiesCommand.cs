using System.Dynamic;
using System.Threading.Tasks;
using LvService.Commands.Common;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Commands.Azure.Storage.Table
{
    public class CreateTableEntitiesCommand : TableCudBatchCommand
    {
        public CreateTableEntitiesCommand(ICommand command = null)
            : base(command)
        {

        }

        public override async Task ExecuteAsync(dynamic p)
        {
            await base.ExecuteAsync(p as ExpandoObject);

            if (!CanExecute(p)) return;

            var batchOperation = new TableBatchOperation();
            foreach (var entity in Entities)
            {
                batchOperation.Insert(entity);
            }
            await Table.ExecuteBatchAsync(batchOperation);
        }
    }
}