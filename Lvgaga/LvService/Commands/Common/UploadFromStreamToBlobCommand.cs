using System;
using System.Dynamic;
using System.Web;

namespace LvService.Commands.Common
{
    public class UploadFromStreamToBlobCommand : AzureStorageCommand
    {
        public UploadFromStreamToBlobCommand()
        {

        }

        public UploadFromStreamToBlobCommand(ICommand command)
            : base(command)
        {

        }

        public override bool CanExecute(dynamic p)
        {
            if (!base.CanExecute(p as ExpandoObject)) return false;
            try
            {
                return p.TableEntity.Uri is string && p.File is HttpPostedFileBase;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override async void Execute(dynamic p)
        {
            if (CanExecute(p))
            {
                string relativeUri = p.TableEntity.Uri;
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

            base.Execute(p as ExpandoObject);
        }
    }
}
