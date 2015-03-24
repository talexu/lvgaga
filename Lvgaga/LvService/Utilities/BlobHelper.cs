using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Blob;

namespace LvService.Utilities
{
    public class BlobHelper
    {
        public static List<string> ListBlobs(CloudBlobContainer container)
        {
            return container.ListBlobs().Select(item => item.Uri.AbsoluteUri).ToList();
        }
    }
}