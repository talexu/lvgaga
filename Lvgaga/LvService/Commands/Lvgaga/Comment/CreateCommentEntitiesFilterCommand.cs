using System;
using System.Threading.Tasks;
using LvModel.Common;
using LvService.Commands.Common;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Commands.Lvgaga.Comment
{
    public class CreateCommentEntitiesFilterCommand : ICommand
    {
        private string _partitionKey;

        public bool CanExecute(dynamic p)
        {
            _partitionKey = p.PartitionKey;
            return !String.IsNullOrEmpty(_partitionKey);
        }

        public Task ExecuteAsync(dynamic p)
        {
            if (!CanExecute(p)) return Task.FromResult(false);

            p.Filter = TableQuery.GenerateFilterCondition(LvConstants.PartitionKey, QueryComparisons.Equal,
                _partitionKey);
            return Task.FromResult(true);
        }
    }
}