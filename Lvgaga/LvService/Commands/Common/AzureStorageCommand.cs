using System;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Commands.Common
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

        public override bool CanExecute(dynamic p)
        {
            try
            {
                return p.Table is CloudTable;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}