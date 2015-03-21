using System;
using System.Dynamic;
using System.Threading.Tasks;
using LvModel.Azure.StorageTable;
using LvModel.Common;
using LvService.Commands.Common;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Commands.Tumblr
{
    public class ReadTumblrsCommand : AzureStorageCommand
    {
        public ReadTumblrsCommand()
        {

        }

        public ReadTumblrsCommand(ICommand command)
            : base(command)
        {

        }

        public override bool CanExecute(dynamic p)
        {
            if (!base.CanExecute(p as ExpandoObject)) return false;

            try
            {
                return !String.IsNullOrEmpty(p.PartitionKey);
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
            string filter = TableQuery.GenerateFilterCondition(Constants.PartitionKey, QueryComparisons.Equal, p.PartitionKey);
            var query = new TableQuery<TumblrEntity>
            {
                TakeCount = 20
            }.Where(filter);
            p.Results = (await table.ExecuteQuerySegmentedAsync(query, null)).Results;

            await base.ExecuteAsync(p as ExpandoObject);
        }
    }
}