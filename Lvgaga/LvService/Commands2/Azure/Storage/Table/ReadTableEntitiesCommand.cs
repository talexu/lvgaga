using System;
using System.Dynamic;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Commands2.Azure.Storage.Table
{
    public class ReadTableEntitiesCommand<T> : AbstractTableCommand where T : ITableEntity, new()
    {
        protected string Filter;

        public new bool CanExecute(dynamic p)
        {
            if (!base.CanExecute(p as ExpandoObject)) return false;

            Filter = p.Filter;
            return !String.IsNullOrEmpty(Filter);
        }

        public override async System.Threading.Tasks.Task ExecuteAsync(dynamic p)
        {
            if (!CanExecute(p)) return;

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
            query = query.Where(Filter);

            var res = await Table.ExecuteQuerySegmentedAsync(query, null);
            p.ContinuationToken = res.ContinuationToken;
            p.Entities = res.Results;
        }
    }
}