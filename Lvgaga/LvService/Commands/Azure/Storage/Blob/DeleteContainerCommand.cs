using System.Dynamic;
using System.Threading.Tasks;
using LvService.Commands.Common;

namespace LvService.Commands.Azure.Storage.Blob
{
    public class DeleteContainerCommand : ContainerCommand
    {
        public DeleteContainerCommand(ICommand command = null)
            : base(command)
        {

        }

        public override async Task ExecuteAsync(dynamic p)
        {
            await base.ExecuteAsync(p as ExpandoObject);

            if (!CanExecute(p as ExpandoObject)) return;

            await Container.DeleteIfExistsAsync();
        }
    }
}