using System;
using System.Dynamic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Commands.Azure.Storage.Table
{
    public class ReadTableEntityCommand : EntityTableCommandChain
    {
        public CloudTable Table { get; private set; }
        public string PartitionKey { get; private set; }
        public string RowKey { get; private set; }

        public ReadTableEntityCommand()
        {

        }

        public ReadTableEntityCommand(IEntityTableCommand command)
            : base(command)
        {

        }

        public override bool CanExecute<T>(dynamic p)
        {
            try
            {
                Table = p.Table;
                PartitionKey = p.PartitionKey;
                RowKey = p.RowKey;
                return Table != null && !String.IsNullOrEmpty(PartitionKey) && !String.IsNullOrEmpty(RowKey);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override async Task<T> ExecuteAsync<T>(dynamic p)
        {
            if (!CanExecute<T>(p)) return default(T);

            var retrieveOperation = TableOperation.Retrieve<T>(PartitionKey, RowKey);
            p.Result = (await Table.ExecuteAsync(retrieveOperation)).Result;
            await base.ExecuteAsync<T>(p as ExpandoObject);

            return p.Result;
        }
    }
}