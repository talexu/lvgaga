namespace LvService.Commands2.Azure.Storage.Blob
{
    public class DeleteBlobCommand : AbstractBlobCommand
    {

        public override async System.Threading.Tasks.Task ExecuteAsync(dynamic p)
        {
            if (!CanExecute(p)) return;

            var blockBlob = Container.GetBlockBlobReference(BlobName);
            await blockBlob.DeleteIfExistsAsync();
        }
    }
}