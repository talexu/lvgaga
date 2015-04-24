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
    public class ReadPointFavoriteEntitiesCommand : TableEntitiesCommandChain
    {
        private string _partitionKey;
        private string _rowKey;
        private string _mediaType;
        public IUriFactory UriFactory { get; set; }

        public new bool CanExecute(dynamic p)
        {
            _partitionKey = p.PartitionKey;
            _rowKey = p.RowKey;
            _mediaType = p.MediaType;

            return new[] { _partitionKey, _rowKey, _mediaType }.AllNotNullOrEmpty();
        }

        public override async Task<List<T>> ExecuteAsync<T>(dynamic p)
        {
            await base.ExecuteAsync<T>(p as ExpandoObject);

            if (!CanExecute(p)) return null;

            var pk = TableQuery.GenerateFilterCondition(LvConstants.PartitionKey, QueryComparisons.Equal,
                _partitionKey);

            var rk1 = TableQuery.GenerateFilterCondition(LvConstants.RowKey, QueryComparisons.Equal, UriFactory.CreateFavoriteRowKey(_mediaType, _rowKey));
            var rk0 = TableQuery.GenerateFilterCondition(LvConstants.RowKey, QueryComparisons.Equal,
                UriFactory.CreateFavoriteRowKey(LvConstants.MediaTypeOfAll, _rowKey));
            var rk = TableQuery.CombineFilters(rk1, TableOperators.Or, rk0);

            p.Filter = TableQuery.CombineFilters(pk, TableOperators.And, rk);

            return null;
        }
    }
}