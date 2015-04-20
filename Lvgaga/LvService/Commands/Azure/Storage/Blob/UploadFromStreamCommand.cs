using System.Dynamic;
using System.IO;
using System.Threading.Tasks;
using LvService.Commands.Common;

namespace LvService.Commands.Azure.Storage.Blob
{
    public class UploadFromStreamCommand : BlobCrudCommand
    {
        protected Stream Stream;

        public UploadFromStreamCommand()
        {

        }

        public UploadFromStreamCommand(ICommand command)
            : base(command)
        {

        }

        public new bool CanExecute(dynamic p)
        {
            if (!base.CanExecute(p as ExpandoObject)) return false;

            Stream = p.Stream;
            return Stream != null;
        }

        public override async Task ExecuteAsync(dynamic p)
        {
            await base.ExecuteAsync(p as ExpandoObject);

            if (!CanExecute(p)) return;

            var blockBlob = Container.GetBlockBlobReference(BlobName);
            await blockBlob.UploadFromStreamAsync(Stream);
            p.BlobUri = blockBlob.Uri.AbsoluteUri;
        }
    }
}