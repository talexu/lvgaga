﻿using System.Dynamic;
using System.Threading.Tasks;
using LvService.Commands.Common;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Commands.Azure.Storage.Table
{
    public class DeleteTableEntityCommand : TableCudCommand
    {
        public DeleteTableEntityCommand()
        {

        }

        public DeleteTableEntityCommand(ICommand command)
            : base(command)
        {

        }

        public override async Task ExecuteAsync(dynamic p)
        {
            await base.ExecuteAsync(p as ExpandoObject);

            if (!CanExecute(p)) return;

            await Table.ExecuteAsync(TableOperation.Delete(Entity));
        }
    }
}