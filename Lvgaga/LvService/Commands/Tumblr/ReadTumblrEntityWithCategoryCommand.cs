﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using LvModel.Common;
using LvModel.View.Tumblr;
using LvService.Commands.Azure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Commands.Tumblr
{
    public class ReadTumblrEntityWithCategoryCommand : TableEntitiesCommandChain
    {
        private string _partitionKey;
        private int _category;

        public ReadTumblrEntityWithCategoryCommand()
        {

        }

        public ReadTumblrEntityWithCategoryCommand(ITableEntitiesCommand command)
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
            if (!CanExecute<T>(p)) return null;

            var filterByPk = TableQuery.GenerateFilterCondition(Constants.PartitionKey, QueryComparisons.Equal,
                _partitionKey);
            var filterByRk = TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition(Constants.RowKey, QueryComparisons.GreaterThanOrEqual,
                    _category.ToString()),
                TableOperators.And,
                TableQuery.GenerateFilterCondition(Constants.RowKey, QueryComparisons.LessThan,
                    (_category + 1).ToString()));
            p.Filter = TableQuery.CombineFilters(filterByPk, TableOperators.And, filterByRk);

            return await base.ExecuteAsync<T>(p as ExpandoObject);
        }
    }
}