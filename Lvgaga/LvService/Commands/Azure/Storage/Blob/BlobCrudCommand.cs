using System;
using LvService.Commands.Common;
using Microsoft.WindowsAzure.Storage.Blob;

namespace LvService.Commands.Azure.Storage.Blob
{
    public class BlobCrudCommand : CommandChain
    {
        protected CloudBlobContainer CloudBlobContainer;
        protected string BlobName;

        public BlobCrudCommand()
        {

        }

        public BlobCrudCommand(ICommand command)
            : base(command)
        {

        }

        public new bool CanExecute(dynamic p)
        {
            try
            {
                CloudBlobContainer = p.Container;
                BlobName = p.BlobName;
                return CloudBlobContainer != null && !String.IsNullOrEmpty(BlobName);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}