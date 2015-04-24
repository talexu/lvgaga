using System;
using System.Dynamic;

namespace LvService.Commands2.Azure.Storage.Blob
{
    public abstract class AbstractBlobCommand : AbstractBlobContainerCommand
    {
        protected string BlobName;

        public new bool CanExecute(dynamic p)
        {
            if (!base.CanExecute(p as ExpandoObject)) return false;

            BlobName = p.BlobName;
            return !String.IsNullOrEmpty(BlobName);
        }
    }
}