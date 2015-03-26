using System;
using System.Dynamic;
using System.Threading.Tasks;
using LvService.Commands.Common;
using Microsoft.WindowsAzure.Storage.Blob;

namespace LvService.Commands.Azure.Storage.Blob
{
    public class DeleteContainerCommand : CommandChain
    {
        protected CloudBlobContainer CloudBlobContainer;

        public DeleteContainerCommand()
        {

        }

        public DeleteContainerCommand(ICommand command)
            : base(command)
        {

        }

        public new bool CanExecute(dynamic p)
        {
            try
            {
                CloudBlobContainer = p.Container;
                return CloudBlobContainer != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override async Task ExecuteAsync(dynamic p)
        {
            if (!CanExecute(p as ExpandoObject)) return;

            await CloudBlobContainer.DeleteIfExistsAsync();

            await base.ExecuteAsync(p as ExpandoObject);
        }
    }
}