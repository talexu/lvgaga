using System;
using System.Dynamic;
using LvService.Commands.Common;

namespace LvService.Commands.Azure.Storage.Blob
{
    public class BlobCrudCommand : ContainerCommand
    {
        protected string BlobName;

        public BlobCrudCommand(ICommand command = null)
            : base(command)
        {

        }

        public new bool CanExecute(dynamic p)
        {
            if (!base.CanExecute(p as ExpandoObject)) return false;

            try
            {
                BlobName = p.BlobName;
                return !String.IsNullOrEmpty(BlobName);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}