using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using LvModel.Common;
using LvService.Commands.Azure.Storage.Table;
using LvService.Factories.Uri;
using LvService.Utilities;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Commands.Tumblr
{
    public class ReadRangeFavoriteEntitiesCommand : TableEntitiesCommandChain
    {
        private string _partitionKey;
        private string _from;
        private string _to;
        private string _mediaType;
        public IUriFactory UriFactory { get; set; }

        public new bool CanExecute(dynamic p)
        {
            try
            {
                _partitionKey = p.PartitionKey;
                _from = p.From;
                _to = p.To;
                _mediaType = p.MediaType;

                return new[] { _partitionKey, _from, _to, _mediaType }.AllNotNullOrEmpty();
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override async Task<List<T>> ExecuteAsync<T>(dynamic p)
        {
            await base.ExecuteAsync<T>(p as ExpandoObject);

            if (!CanExecute(p)) return null;

            var pk = TableQuery.GenerateFilterCondition(LvConstants.PartitionKey, QueryComparisons.Equal,
                _partitionKey);

            var rk0 = TableQuery.GenerateFilterCondition(LvConstants.RowKey, QueryComparisons.GreaterThanOrEqual,
                UriFactory.CreateFavoriteRowKey(_mediaType, _from));
            var rk1 = TableQuery.GenerateFilterCondition(LvConstants.RowKey, QueryComparisons.LessThanOrEqual,
                UriFactory.CreateFavoriteRowKey(_mediaType, _to));
            var rk = TableQuery.CombineFilters(rk0, TableOperators.And, rk1);

            p.Filter = TableQuery.CombineFilters(pk, TableOperators.And, rk);

            return null;
        }
    }
}