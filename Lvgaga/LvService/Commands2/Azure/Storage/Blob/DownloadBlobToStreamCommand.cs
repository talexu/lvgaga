using System.IO;
using System.Threading.Tasks;
namespace LvService.Commands2.Azure.Storage.Blob
{
    public class DownloadBlobToStreamCommand : AbstractBlobCommand
    {
        public override async Task ExecuteAsync(dynamic p)
        {
            if (!CanExecute(p)) return;

            var blockBlob = Container.GetBlockBlobReference(BlobName);
            using (var memoryStream = new MemoryStream())
            {
                await blockBlob.DownloadToStreamAsync(memoryStream);
                p.Stream = memoryStream;
            }
        }
    }
}