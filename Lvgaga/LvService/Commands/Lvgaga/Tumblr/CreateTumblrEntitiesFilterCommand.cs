using System;
using System.Threading.Tasks;
using LvModel.Common;
using LvModel.View.Tumblr;
using LvService.Commands.Common;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Commands.Lvgaga.Tumblr
{
    public class CreateTumblrEntitiesFilterCommand : ICommand
    {
        private string _partitionKey;
        private int _category;

        public bool CanExecute(dynamic p)
        {
            _partitionKey = p.PartitionKey;
            TumblrCategory cate = p.Category;
            return int.TryParse(cate.ToString("D"), out _category) && !String.IsNullOrEmpty(_partitionKey);
        }

        public Task ExecuteAsync(dynamic p)
        {
            if (!CanExecute(p)) return Task.FromResult(false);

            var filterByPk = TableQuery.GenerateFilterCondition(LvConstants.PartitionKey, QueryComparisons.Equal,
                _partitionKey);
            var filterByRk = TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition(LvConstants.RowKey, QueryComparisons.GreaterThanOrEqual,
                    _category.ToString()),
                TableOperators.And,
                TableQuery.GenerateFilterCondition(LvConstants.RowKey, QueryComparisons.LessThan,
                    (_category + 1).ToString()));
            p.Filter = TableQuery.CombineFilters(filterByPk, TableOperators.And, filterByRk);

            return Task.FromResult(true);
        }
    }
}