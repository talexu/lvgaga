using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LvModel.Common;
using LvModel.View.Tumblr;
using LvService.Commands.Azure.Storage.Blob;
using LvService.Commands.Azure.Storage.Table;
using LvService.Commands.Lvgaga.Tumblr;
using LvService.DbContexts;
using LvService.Factories.Azure.Storage;
using LvService.Factories.Uri;
using LvService.Services;
using Microsoft.WindowsAzure.Storage;

namespace LvFakeData
{
    public class FakeHelper
    {
        private readonly IAzureStorage _azureStorage;

        public FakeHelper()
        {
            //_azureStorage = new AzureStoragePool(new AzureStorageDb(CloudStorageAccount.DevelopmentStorageAccount), new LvMemoryCache(), new CacheKeyFactory());
            _azureStorage = new AzureStoragePool(
                new AzureStorageDb(CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=lvgagastg;AccountKey=QauIB618nLhQzbwblVG2fyXROpB0TPapFVkzcFXSWN5ZauV3/pZ2Z2y5fAnAxBGHWp7Fhvt1xP5mc1MQilbb6w==")),
                new LvMemoryCache(),
                new CacheKeyFactory());
        }

        public async Task UploadTestTumblrs()
        {
            var uriFactory = new UriFactory();
            var tableEntityFactory = new TableEntityFactory(uriFactory);

            var uploadBlobCommand = new UploadBlobFromStreamCommand();
            var generateThumbnailCommand = new GenerateThumbnailCommand();
            var createTumblrCommand = new CreateTumblrCommand
            {
                UriFactory = uriFactory,
                TableEntityFactory = tableEntityFactory
            };
            var createTableEntitiesCommand = new CreateTableEntitiesCommand();
            var command = new WriteTumblrCommand(_azureStorage, uploadBlobCommand, generateThumbnailCommand,
                createTumblrCommand, createTableEntitiesCommand);

            foreach (var testImage in GetTestImages())
            {
                using (Stream stream = File.OpenRead(testImage))
                {
                    // upload
                    dynamic pTumblr = new ExpandoObject();

                    // Blob
                    pTumblr.Container = await _azureStorage.GetContainerReferenceAsync(LvConstants.ContainerNameOfImage);
                    pTumblr.Stream = stream;
                    pTumblr.BlobName = Path.GetFileName(testImage);

                    // Create Tumblr
                    pTumblr.PartitionKey = LvConstants.MediaTypeOfImage;
                    pTumblr.TumblrText = new TumblrText
                    {
                        Text = "你也曾当过笨蛋，也曾试着当瞎子当聋子的去信任一个人，你也知道世界上最可悲的就是自我欺骗，但是人笨过傻过瞎过就够了，你更要懂得爱自己，而不是一直重蹈覆辙，还自以为多痴情。",
                        Category = TumblrCategory.C2
                    };

                    // Table
                    pTumblr.Table = await _azureStorage.GetTableReferenceAsync(LvConstants.TableNameOfTumblr);

                    // Execute
                    await command.ExecuteAsync(pTumblr);
                    // Create Tumblr End
                }
            }
        }

        private static IEnumerable<string> GetTestImages()
        {
            var theFolder = new DirectoryInfo(Path.GetFullPath("..\\..\\Resources\\images"));
            return theFolder.GetFiles().Select(nextFile => nextFile.FullName).ToList();
        }
    }
}