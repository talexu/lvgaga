using System.Threading.Tasks;
using LvService.Commands.Common;
using Microsoft.WindowsAzure.Storage.Blob;

namespace LvService.Commands.Azure.Storage.Blob
{
    public abstract class AbstractBlobContainerCommand : ICommand
    {
        protected CloudBlobContainer Container;

        public bool CanExecute(dynamic p)
        {
            Container = p.Container;
            return Container != null;
        }

        public abstract Task ExecuteAsync(dynamic p);
    }
}