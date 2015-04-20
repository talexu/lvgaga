using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using LvService.Commands.Common;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Commands.Azure.Storage.Table
{
    public class DeleteTableEntitiesCommand : TableCudBatchCommand, ITableEntitiesCommand
    {
        public ITableEntitiesCommand TableEntitiesCommand { get; set; }

        public DeleteTableEntitiesCommand()
        {

        }

        public DeleteTableEntitiesCommand(ICommand command)
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
                    batchOperation.Delete(entity);
                }

                await Table.ExecuteBatchAsync(batchOperation);
            }
        }

        public async Task<List<T>> ExecuteAsync<T>(dynamic p) where T : ITableEntity, new()
        {
            var entities = await TableEntitiesCommand.ExecuteAsync<T>(p as ExpandoObject);
            p.Entities = entities.Cast<ITableEntity>().ToList();
            await ExecuteAsync(p);

            return entities;
        }
    }
}