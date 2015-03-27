using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Commands.Azure.Storage.Table
{
    public class ReadTableEntitiesCommand : TableEntitiesCommandChain
    {
        public CloudTable Table { get; private set; }
        public string Filter { get; private set; }
        public int TakeCount { get; private set; }

        public ReadTableEntitiesCommand(ITableEntitiesCommand command = null)
            : base(command)
        {

        }

        public new bool CanExecute(dynamic p)
        {
            try
            {
                Table = p.Table;
                Filter = p.Filter;
                TakeCount = p.TakeCount;
                return Table != null && !String.IsNullOrEmpty(Filter) && TakeCount > 0;
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

            var query = new TableQuery<T>
            {
                TakeCount = TakeCount
            }.Where(Filter);
            return (await Table.ExecuteQuerySegmentedAsync(query, null)).Results;
        }
    }
}