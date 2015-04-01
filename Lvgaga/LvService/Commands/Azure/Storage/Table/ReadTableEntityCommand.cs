using System;
using System.Dynamic;
using System.Threading.Tasks;
using LvService.Utilities;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Commands.Azure.Storage.Table
{
    public class ReadTableEntityCommand : TableEntityCommandChain
    {
        public CloudTable Table { get; private set; }
        public string PartitionKey { get; private set; }
        public string RowKey { get; private set; }

        public ReadTableEntityCommand()
        {

        }

        public ReadTableEntityCommand(ITableEntityCommand command)
            : base(command)
        {

        }

        public new bool CanExecute<T>(dynamic p)
        {
            try
            {
                Table = p.Table;
                PartitionKey = p.PartitionKey;
                RowKey = p.RowKey;
                return Table != null && new[] { PartitionKey, RowKey }.AllNotNullOrEmpty();
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override async Task<T> ExecuteAsync<T>(dynamic p)
        {
            await base.ExecuteAsync<T>(p as ExpandoObject);

            if (!CanExecute<T>(p)) return default(T);

            var retrieveOperation = TableOperation.Retrieve<T>(PartitionKey, RowKey);
            return (T)(await Table.ExecuteAsync(retrieveOperation)).Result;
        }
    }
}