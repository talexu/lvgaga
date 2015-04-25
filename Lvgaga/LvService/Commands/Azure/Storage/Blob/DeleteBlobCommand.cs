using System.Threading.Tasks;

namespace LvService.Commands.Azure.Storage.Blob
{
    public class DeleteBlobCommand : AbstractBlobCommand
    {
        public override async Task ExecuteAsync(dynamic p)
        {
            if (!CanExecute(p)) return;

            var blockBlob = Container.GetBlockBlobReference(BlobName);
            await blockBlob.DeleteIfExistsAsync();
        }
    }
}