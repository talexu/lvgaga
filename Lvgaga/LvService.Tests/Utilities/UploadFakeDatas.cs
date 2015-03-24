using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LvModel.Azure.StorageTable;
using LvModel.Common;
using LvService.Commands.Azure.Storage.Blob;
using LvService.Commands.Azure.Storage.Table;
using LvService.Tests.Commands.Azure.Storage;
using LvService.Utilities;
using Xunit;

namespace LvService.Tests.Utilities
{
    public class TumblrFactoryTests : IClassFixture<AzureStorageFixture>
    {
        private readonly AzureStorageFixture _azureStorageFixture;
        private readonly UploadFromStreamCommand _uploadFromStreamCommand;
        private readonly CreateTableEntityCommand _createTableEntityCommand;

        public TumblrFactoryTests(AzureStorageFixture azureStorageFixture)
        {
            _azureStorageFixture = azureStorageFixture;
            _uploadFromStreamCommand = new UploadFromStreamCommand();
            _createTableEntityCommand = new CreateTableEntityCommand();
        }

        //[Fact]
        public async Task UploadTestImagesToBlob()
        {
            foreach (var testImage in GetTestImages())
            {
                // upload
                dynamic p = new ExpandoObject();
                p.Container =
                    await _azureStorageFixture.AzureStorage.GetContainerReferenceAsync(Constants.ImageContainerName);
                p.Stream = File.OpenRead(testImage);
                p.BlobName = Path.GetFileName(testImage);
                await _uploadFromStreamCommand.ExecuteAsync(p);
            }
        }

        public IEnumerable<string> GetTestImages()
        {
            var theFolder = new DirectoryInfo(Path.GetFullPath("..\\..\\Resources\\images"));
            return theFolder.GetFiles().Select(nextFile => nextFile.FullName).ToList();
        }

        //[Fact]
        public async Task UploadTestTumblrsToStorageTable()
        {
            var container =
                await _azureStorageFixture.AzureStorage.GetContainerReferenceAsync(Constants.ImageContainerName);
            var table = await _azureStorageFixture.AzureStorage.GetTableReferenceAsync(Constants.TumblrTableName);

            foreach (var blob in BlobHelper.ListBlobs(container))
            {
                var now = DateTime.UtcNow;
                var entity = new TumblrEntity(Constants.MediaTypeImage, DateTimeHelper.GetInvertedTicks(now))
                {
                    MediaUri = blob,
                    Text = "你也曾当过笨蛋，也曾试着当瞎子当聋子的去信任一个人，你也知道世界上最可悲的就是自我欺骗，但是人笨过傻过瞎过就够了，你更要懂得爱自己，而不是一直重蹈覆辙，还自以为多痴情。",
                    CreateTime = now
                };
                dynamic p = new ExpandoObject();
                p.Table = table;
                p.Entity = entity;
                await _createTableEntityCommand.ExecuteAsync(p);
            }
        }
    }
}