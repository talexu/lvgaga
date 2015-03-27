using System.Dynamic;
using System.Threading.Tasks;
using LvService.Commands.Common;

namespace LvService.Commands.Azure.Storage.Blob
{
    public class DeleteBlobCommand : BlobCrudCommand
    {
        public DeleteBlobCommand()
        {

        }

        public DeleteBlobCommand(ICommand command)
            : base(command)
        {

        }

        public override async Task ExecuteAsync(dynamic p)
        {
            await base.ExecuteAsync(p as ExpandoObject);

            if (!CanExecute(p)) return;

            var blockBlob = Container.GetBlockBlobReference(BlobName);
            await blockBlob.DeleteIfExistsAsync();
        }
    }
}