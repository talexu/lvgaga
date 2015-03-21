using System;
using System.Dynamic;
using System.Threading.Tasks;
using LvModel.Azure.StorageTable;
using LvService.Commands.Common;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Commands.Tumblr
{
    public class ReadTumblrCommand : AzureStorageCommand
    {
        public ReadTumblrCommand()
        {

        }

        public ReadTumblrCommand(ICommand command)
            : base(command)
        {

        }

        public override bool CanExecute(dynamic p)
        {
            if (!base.CanExecute(p as ExpandoObject)) return false;

            try
            {
                return !String.IsNullOrEmpty(p.PartitionKey) && !String.IsNullOrEmpty(p.RowKey);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override async Task ExecuteAsync(dynamic p)
        {
            if (!CanExecute(p)) return;

            CloudTable table = p.Table;
            TableOperation retrieveOperation = TableOperation.Retrieve<TumblrEntity>(p.PartitionKey, p.RowKey);
            p.Result = (await table.ExecuteAsync(retrieveOperation)).Result;

            await base.ExecuteAsync(p as ExpandoObject);
        }
    }
}