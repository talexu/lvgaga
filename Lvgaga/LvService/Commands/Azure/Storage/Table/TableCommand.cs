using System;
using LvService.Commands.Common;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Commands.Azure.Storage.Table
{
    public class TableCommand : CommandChain
    {
        protected CloudTable Table { get; private set; }

        public TableCommand()
        {

        }

        public TableCommand(ICommand command)
            : base(command)
        {

        }

        public new bool CanExecute(dynamic p)
        {
            try
            {
                Table = p.Table;
                return Table != null;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}