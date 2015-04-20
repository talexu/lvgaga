using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Commands.Azure.Storage.Table
{
    public class ReadTableEntitiesCommand : TableEntitiesCommandChain
    {
        private CloudTable _table;
        private string _filter;

        public ReadTableEntitiesCommand()
        {

        }

        public ReadTableEntitiesCommand(ITableEntitiesCommand command)
            : base(command)
        {

        }

        public new bool CanExecute(dynamic p)
        {
            _table = p.Table;
            _filter = p.Filter;
            return _table != null && !String.IsNullOrEmpty(_filter);
        }

        public override async Task<List<T>> ExecuteAsync<T>(dynamic p)
        {
            await base.ExecuteAsync<T>(p as ExpandoObject);

            if (!CanExecute(p)) return null;

            var query = new TableQuery<T>();
            try
            {
                var takeCount = p.TakeCount;
                query.TakeCount = takeCount;
            }
            catch (Exception)
            {
                // ignored
            }
            query = query.Where(_filter);

            var res = await _table.ExecuteQuerySegmentedAsync(query, null);
            p.ContinuationToken = res.ContinuationToken;

            return res.Results;
        }
    }
}