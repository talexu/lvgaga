using System.Dynamic;
using System.Threading.Tasks;

namespace LvService.Commands2.Azure.Storage.Blob
{
    public class DeleteContainerCommand : AbstractBlobContainerCommand
    {
        public override async Task ExecuteAsync(dynamic p)
        {
            if (!CanExecute(p as ExpandoObject)) return;
            await Container.DeleteIfExistsAsync();
        }
    }
}