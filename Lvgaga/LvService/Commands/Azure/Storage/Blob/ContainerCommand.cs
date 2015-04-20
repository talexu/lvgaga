using LvService.Commands.Common;
using Microsoft.WindowsAzure.Storage.Blob;

namespace LvService.Commands.Azure.Storage.Blob
{
    public class ContainerCommand : CommandChain
    {
        protected CloudBlobContainer Container;

        public ContainerCommand()
        {

        }

        public ContainerCommand(ICommand command)
            : base(command)
        {

        }

        public new bool CanExecute(dynamic p)
        {
            Container = p.Container;
            return Container != null;
        }
    }
}