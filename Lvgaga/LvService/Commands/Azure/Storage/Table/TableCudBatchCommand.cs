using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using LvService.Commands.Common;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Commands.Azure.Storage.Table
{
    public class TableCudBatchCommand : TableCommand
    {
        protected IEnumerable<ITableEntity> Entities { get; private set; }

        public TableCudBatchCommand()
        {

        }

        public TableCudBatchCommand(ICommand command)
            : base(command)
        {

        }

        public new bool CanExecute(dynamic p)
        {
            if (!base.CanExecute(p as ExpandoObject)) return false;

            try
            {
                Entities = p.Entities;
                return Entities != null && Entities.Any();
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}