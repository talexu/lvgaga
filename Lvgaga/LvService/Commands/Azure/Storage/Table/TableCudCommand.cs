using System;
using System.Dynamic;
using LvService.Commands.Common;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Commands.Azure.Storage.Table
{
    public class TableCudCommand : TableCommand
    {
        protected ITableEntity Entity { get; private set; }

        public TableCudCommand(ICommand command = null)
            : base(command)
        {

        }

        public new bool CanExecute(dynamic p)
        {
            if (!base.CanExecute(p as ExpandoObject)) return false;

            try
            {
                Entity = p.Entity;
                return Entity != null;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}