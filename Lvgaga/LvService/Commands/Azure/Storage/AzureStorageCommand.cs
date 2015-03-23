using LvService.Commands.Common;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;

namespace LvService.Commands.Azure.Storage
{
    public class AzureStorageCommand : CommandChain
    {
        public CloudBlobContainer BlobContainer { get; set; }
        public CloudQueue ThumbnailRequestQueue { get; set; }

        public AzureStorageCommand()
        {

        }

        public AzureStorageCommand(ICommand command)
            : base(command)
        {

        }
    }
}