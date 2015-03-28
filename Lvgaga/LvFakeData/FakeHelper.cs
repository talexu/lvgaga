using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LvModel.Azure.StorageTable;
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

        public async Task UploadTestTumblrs()
        {
            ICommand createTumblrCommand = new CreateTableEntitiesCommand(
                    new CreateTumblrCommand(
                        new UploadFromStreamCommand()) { TableEntityFactory = new TableEntityFactory() });

            ICommand createCommentCommand = new CreateTableEntityCommand(
                new CreateCommentCommand { TableEntityFactory = new TableEntityFactory() });

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
                    pTumblr.PartitionKey = LvConstants.PartitionKeyOfImage;
                    pTumblr.TumblrText = new TumblrText
                    {
                        Text = "你也曾当过笨蛋，也曾试着当瞎子当聋子的去信任一个人，你也知道世界上最可悲的就是自我欺骗，但是人笨过傻过瞎过就够了，你更要懂得爱自己，而不是一直重蹈覆辙，还自以为多痴情。",
                        Category = TumblrCategory.C1
                    };

                    // Table
                    pTumblr.Table = await _azureStorage.GetTableReferenceAsync(LvConstants.TableNameOfTumblr);

                    // Execute
                    await createTumblrCommand.ExecuteAsync(pTumblr);

                    // Create Comment
                    TumblrEntity tumblrEntity = pTumblr.Entity;
                    dynamic pComment = new ExpandoObject();
                    pComment.PartitionKey = tumblrEntity.RowKey.Substring(2);
                    pComment.UserId = "UserID@" + Guid.NewGuid();
                    pComment.UserName = "张磊";
                    pComment.Text = "我非常同意上边这段话，特别是被北海道甩了的那一天，她对我说的每一句话都深深地刻在我心里，然而我却只对她说了一句“Hi”。";
                    pComment.Table = await _azureStorage.GetTableReferenceAsync(LvConstants.TableNameOfComment);

                    for (var i = 0; i < 12; i++)
                    {
                        // Execute
                        await createCommentCommand.ExecuteAsync(pComment);
                    }
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