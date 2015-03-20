using System;
using System.Web;
using Microsoft.WindowsAzure.Storage.Blob;

namespace LvService.Commands.Common
{
    public class UploadFromStreamToBlobCommand : CommandChain
    {
        public CloudBlobContainer BlobContainer { get; set; }

        public UploadFromStreamToBlobCommand()
        {

        }

        public UploadFromStreamToBlobCommand(ICommand command)
            : base(command)
        {

        }

        public override bool CanExecute(dynamic p)
        {
            return p.RelativeUri is string && p.File is HttpPostedFileBase;
        }

        public override async void Execute(dynamic p)
        {
            if (CanExecute(p))
            {
                string relativeUri = p.Entity.Uri;
                HttpPostedFileBase file = p.File;
                if (!String.IsNullOrEmpty(relativeUri) && file != null)
                {
                    var blob = BlobContainer.GetBlockBlobReference(relativeUri);
                    using (var fileStream = file.InputStream)
                    {
                        await blob.UploadFromStreamAsync(fileStream);
                    }
                    p.FileAbsoluteUri = blob.Uri.AbsoluteUri;
                }
            }

            base.Execute(p as object);
        }
    }
}
