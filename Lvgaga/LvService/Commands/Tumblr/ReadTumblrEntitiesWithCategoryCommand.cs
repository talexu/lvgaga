using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using LvModel.Common;
using LvModel.View.Tumblr;
using LvService.Commands.Azure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Commands.Tumblr
{
    public class ReadTumblrEntitiesWithCategoryCommand : TableEntitiesCommandChain
    {
        private string _partitionKey;
        private int _category;

        public ReadTumblrEntitiesWithCategoryCommand()
        {

        }

        public ReadTumblrEntitiesWithCategoryCommand(ITableEntitiesCommand command)
            : base(command)
        {

        }

        public new bool CanExecute<T>(dynamic p)
        {
            try
            {
                _partitionKey = p.PartitionKey;
                TumblrCategory cate = p.Category;
                return int.TryParse(cate.ToString("D"), out _category) && !String.IsNullOrEmpty(_partitionKey);
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

            var filterByPk = TableQuery.GenerateFilterCondition(LvConstants.PartitionKey, QueryComparisons.Equal,
                _partitionKey);
            var filterByRk = TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition(LvConstants.RowKey, QueryComparisons.GreaterThanOrEqual,
                    _category.ToString()),
                TableOperators.And,
                TableQuery.GenerateFilterCondition(LvConstants.RowKey, QueryComparisons.LessThan,
                    (_category + 1).ToString()));
            p.Filter = TableQuery.CombineFilters(filterByPk, TableOperators.And, filterByRk);

            return null;
        }
    }
}