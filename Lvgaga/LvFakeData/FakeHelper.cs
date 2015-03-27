using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LvModel.Common;
using LvModel.View.Tumblr;
using LvService.Commands.Azure.Storage.Blob;
using LvService.Commands.Azure.Storage.Table;
using LvService.Commands.Common;
using LvService.Commands.Tumblr;
using LvService.DbContexts;
using LvService.Factories.Azure.Storage;
using Microsoft.WindowsAzure.Storage;

namespace LvFakeData
{
    public class FakeHelper
    {
        private readonly IAzureStorage _azureStorage;

        public FakeHelper()
        {
            _azureStorage = new AzureStoragePool(new AzureStorageDb(CloudStorageAccount.DevelopmentStorageAccount));

        }

        public async Task UploadTestTumblrs(ICommand command = null)
        {
            if (command == null)
            {
                //command = new UploadFromStreamCommand(
                //new CreateTumblrCommand(
                //    new CreateTableEntityCommand(
                //        new ChangeTumblrRowkeyToZeroCommand(
                //            new CreateTableEntityCommand()))) { TableEntityFactory = new TableEntityFactory() });

                command = new UploadFromStreamCommand(
                    new CreateTumblrCommand(
                        new CreateTableEntitiesCommand()) { TableEntityFactory = new TableEntityFactory() });
            }

            foreach (var testImage in GetTestImages())
            {
                using (Stream stream = File.OpenRead(testImage))
                {
                    // upload
                    dynamic p = new ExpandoObject();

                    // Blob
                    p.Container = await _azureStorage.GetContainerReferenceAsync(Constants.ImageContainerName);
                    p.Stream = stream;
                    p.BlobName = Path.GetFileName(testImage);

                    // Create Tumblr
                    p.PartitionKey = Constants.ImagePartitionKey;
                    p.TumblrText = new TumblrText
                    {
                        Text = "你也曾当过笨蛋，也曾试着当瞎子当聋子的去信任一个人，你也知道世界上最可悲的就是自我欺骗，但是人笨过傻过瞎过就够了，你更要懂得爱自己，而不是一直重蹈覆辙，还自以为多痴情。",
                        Category = TumblrCategory.C1
                    };

                    // Table
                    p.Table = await _azureStorage.GetTableReferenceAsync(Constants.TumblrTableName);

                    // Execute
                    await command.ExecuteAsync(p);
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