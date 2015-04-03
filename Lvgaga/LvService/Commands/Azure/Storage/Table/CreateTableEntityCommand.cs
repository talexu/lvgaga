using System;
using System.Dynamic;
using System.Threading.Tasks;
using LvService.Commands.Common;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Commands.Azure.Storage.Table
{
    public class CreateTableEntityCommand : TableCudCommand
    {
        public CreateTableEntityCommand()
        {

        }

        public CreateTableEntityCommand(ICommand command)
            : base(command)
        {

        }

        public override async Task ExecuteAsync(dynamic p)
        {
            await base.ExecuteAsync(p as ExpandoObject);

            if (!CanExecute(p)) return;

            try
            {
                await Table.ExecuteAsync(TableOperation.Insert(Entity));
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}