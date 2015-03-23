using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Commands.Azure.Storage.Table
{
    public class ReadTableEntitiesCommand : EntityTableCommandChain
    {
        public CloudTable Table { get; private set; }
        public string Filter { get; private set; }
        public int TakeCount { get; private set; }

        public ReadTableEntitiesCommand()
        {

        }

        public ReadTableEntitiesCommand(IEntityTableCommand command)
            : base(command)
        {

        }

        public override bool CanExecute<T>(dynamic p)
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

        public override async Task<List<T>> ExecutesAsync<T>(dynamic p)
        {
            if (!CanExecute<T>(p)) return null;

            var query = new TableQuery<T>
            {
                TakeCount = TakeCount
            }.Where(Filter);
            p.Results = (await Table.ExecuteQuerySegmentedAsync(query, null)).Results;
            await base.ExecutesAsync<T>(p as ExpandoObject);

            return p.Results;
        }
    }
}