using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LvModel.Azure.StorageTable;
using LvModel.Common;
using LvModel.View.Tumblr;
using LvService.DbContexts;
using LvService.Factories.Uri;
using LvService.Factories.ViewModel;
using Microsoft.WindowsAzure.Storage.Blob;

namespace LvService.Utilities
{
    public class FakeDataHelper
    {
        private static readonly IAzureStorage AzureStorage = new AzureStoragePool(new AzureStorageDb());
        private static readonly ITumblrFactory TumblrFactory = new TumblrFactory(new UriFactory());

        public static async Task<List<TumblrModel>> GetFakeTumblrModels()
        {
            var container = await AzureStorage.GetContainerReferenceAsync(Constants.ImageContainerName);

            return BlobHelper.ListBlobs(container).Select(blob => new TumblrEntity()
            {
                PartitionKey = Guid.NewGuid().ToString(),
                RowKey = Guid.NewGuid().ToString(),
                MediaUri = blob,
                Text = "你也曾当过笨蛋，也曾试着当瞎子当聋子的去信任一个人，你也知道世界上最可悲的就是自我欺骗，但是人笨过傻过瞎过就够了，你更要懂得爱自己，而不是一直重蹈覆辙，还自以为多痴情。",
                CreateTime = DateTime.Now
            }).Select(entity => TumblrFactory.CreateTumblrModel(entity)).ToList();
        }
    }
}