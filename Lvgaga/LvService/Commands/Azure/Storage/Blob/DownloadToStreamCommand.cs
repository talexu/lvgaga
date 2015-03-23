

using System;
using System.Dynamic;
using System.IO;
using System.Threading.Tasks;
using LvService.Commands.Common;
using LvService.Utilities;

namespace LvService.Commands.Azure.Storage.Blob
{
    public class DownloadToStreamCommand : BlobCrudCommand
    {
        public DownloadToStreamCommand()
        {

        }

        public DownloadToStreamCommand(ICommand command)
            : base(command)
        {

        }

        public override async Task ExecuteAsync(dynamic p)
        {
            if (!CanExecute(p)) return;

            var blockBlob = CloudBlobContainer.GetBlockBlobReference(BlobName);
            using (var memoryStream = new MemoryStream())
            {
                try
                {
                    await blockBlob.DownloadToStreamAsync(memoryStream);
                    p.Result = memoryStream.ToStringFromMemoryStream();
                }
                catch (Exception)
                {
                    p.Result = null;
                }

            }

            await base.ExecuteAsync(p as ExpandoObject);
        }
    }
}