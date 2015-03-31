using System.IO;
using LvService.Commands.Common;
using Microsoft.WindowsAzure.Storage.Blob;

namespace LvService.Commands.Tumblr
{
    public class UploadTumblrCommand : CommandChain
    {
        protected CloudBlobContainer Container;
        protected string BlobName;
        protected Stream Stream;

        public UploadTumblrCommand()
        {

        }

        public UploadTumblrCommand(ICommand command)
            : base(command)
        {

        }
    }
}