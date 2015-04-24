using System.Dynamic;
using System.IO;

namespace LvService.Commands2.Azure.Storage.Blob
{
    public class UploadBlobFromStreamCommand : AbstractBlobCommand
    {
        protected Stream Stream;

        public new bool CanExecute(dynamic p)
        {
            if (!base.CanExecute(p as ExpandoObject)) return false;

            Stream = p.Stream;
            return Stream != null;
        }

        public override async System.Threading.Tasks.Task ExecuteAsync(dynamic p)
        {
            if (!CanExecute(p)) return;

            var blockBlob = Container.GetBlockBlobReference(BlobName);
            await blockBlob.UploadFromStreamAsync(Stream);
            p.BlobUri = blockBlob.Uri.AbsoluteUri;
        }
    }
}