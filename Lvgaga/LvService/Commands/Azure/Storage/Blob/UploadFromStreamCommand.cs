using System;
using System.Dynamic;
using System.IO;
using System.Threading.Tasks;
using LvService.Commands.Common;

namespace LvService.Commands.Azure.Storage.Blob
{
    public class UploadFromStreamCommand : BlobCrudCommand
    {
        private Stream _source;

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

            try
            {
                _source = p.Stream;
                return _source != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override async Task ExecuteAsync(dynamic p)
        {
            if (!CanExecute(p)) return;

            var blockBlob = CloudBlobContainer.GetBlockBlobReference(BlobName);
            await blockBlob.UploadFromStreamAsync(_source);
            p.MediaUri = blockBlob.Uri.AbsoluteUri;

            await base.ExecuteAsync(p as ExpandoObject);
        }
    }
}