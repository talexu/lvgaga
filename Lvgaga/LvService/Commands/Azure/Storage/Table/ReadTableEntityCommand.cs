using System.Dynamic;
using System.Threading.Tasks;
using LvService.Utilities;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Commands.Azure.Storage.Table
{
    public class ReadTableEntityCommand<T> : AbstractTableCommand where T : ITableEntity, new()
    {
        protected string PartitionKey;
        protected string RowKey;

        public new bool CanExecute(dynamic p)
        {
            if (!base.CanExecute(p as ExpandoObject)) return false;

            PartitionKey = p.PartitionKey;
            RowKey = p.RowKey;
            return new[] { PartitionKey, RowKey }.AllNotNullOrEmpty();
        }

        public override async Task ExecuteAsync(dynamic p)
        {
            if (!CanExecute(p)) return;

            var retrieveOperation = TableOperation.Retrieve<T>(PartitionKey, RowKey);
            p.Entity = (await Table.ExecuteAsync(retrieveOperation)).Result;
        }
    }
}