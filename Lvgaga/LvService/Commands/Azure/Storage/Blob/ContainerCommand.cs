using System;
using LvService.Commands.Common;
using Microsoft.WindowsAzure.Storage.Blob;

namespace LvService.Commands.Azure.Storage.Blob
{
    public class ContainerCommand : CommandChain
    {
        protected CloudBlobContainer Container;

        public ContainerCommand(ICommand command = null)
            : base(command)
        {

        }

        public new bool CanExecute(dynamic p)
        {
            try
            {
                Container = p.Container;
                return Container != null;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}