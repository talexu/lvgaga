using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using LvModel.Common;
using LvService.Commands.Azure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Commands.Tumblr
{
    public class ReadFavoriteEntitiesWithMediaTypeCommand : TableEntitiesCommandChain
    {
        private string _partitionKey;
        private int _mediaType;

        public ReadFavoriteEntitiesWithMediaTypeCommand()
        {

        }

        public ReadFavoriteEntitiesWithMediaTypeCommand(ITableEntitiesCommand command)
            : base(command)
        {

        }

        public new bool CanExecute(dynamic p)
        {
            _partitionKey = p.PartitionKey;
            string media = p.MediaType;
            return int.TryParse(media, out _mediaType) && !String.IsNullOrEmpty(_partitionKey);
        }

        public override async Task<List<T>> ExecuteAsync<T>(dynamic p)
        {
            await base.ExecuteAsync<T>(p as ExpandoObject);

            if (!CanExecute(p)) return null;

            var filterByPk = TableQuery.GenerateFilterCondition(LvConstants.PartitionKey, QueryComparisons.Equal,
                _partitionKey);
            var filterByRk = TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition(LvConstants.RowKey, QueryComparisons.GreaterThanOrEqual,
                    _mediaType.ToString()),
                TableOperators.And,
                TableQuery.GenerateFilterCondition(LvConstants.RowKey, QueryComparisons.LessThan,
                    (_mediaType + 1).ToString()));
            p.Filter = TableQuery.CombineFilters(filterByPk, TableOperators.And, filterByRk);

            return null;
        }
    }
}