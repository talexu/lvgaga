using System;
using LvService.Commands.Common;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Commands.Azure.Storage.Table
{
    public class TableCudCommand : CommandChain
    {
        public CloudTable Table { get; private set; }
        public ITableEntity Entity { get; private set; }

        public TableCudCommand()
        {

        }

        public TableCudCommand(ICommand command)
            : base(command)
        {

        }

        public new bool CanExecute(dynamic p)
        {
            try
            {
                Table = p.Table;
                Entity = p.Entity;
                return Table != null && Entity != null;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}