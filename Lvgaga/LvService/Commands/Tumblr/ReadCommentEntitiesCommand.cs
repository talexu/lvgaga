using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using LvModel.Common;
using LvService.Commands.Azure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Commands.Tumblr
{
    public class ReadCommentEntitiesCommand : TableEntitiesCommandChain
    {
        private string _partitionKey;

        public ReadCommentEntitiesCommand()
        {

        }

        public ReadCommentEntitiesCommand(ITableEntitiesCommand command)
            : base(command)
        {

        }

        public new bool CanExecute<T>(dynamic p)
        {
            try
            {
                _partitionKey = p.PartitionKey;
                return !String.IsNullOrEmpty(_partitionKey);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override async Task<List<T>> ExecuteAsync<T>(dynamic p)
        {
            await base.ExecuteAsync<T>(p as ExpandoObject);

            if (!CanExecute<T>(p)) return null;

            p.Filter = TableQuery.GenerateFilterCondition(LvConstants.PartitionKey, QueryComparisons.Equal,
                _partitionKey);
            return null;
        }
    }
}