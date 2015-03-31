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
using LvService.Factories.Uri;
using LvService.Utilities;
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
            TableEntityFactory tableEntityFactory = new TableEntityFactory(new UriFactory());
            ICommand createTumblrCommand = new CreateTableEntitiesCommand(
                new CreateTumblrCommand(
                    new UploadFromStreamCommand(
                        new UploadThumbnailCommand(
                            new UploadFromStreamCommand(
                                new UploadMediaCommand())))) { TableEntityFactory = tableEntityFactory });

            ICommand createCommentCommand = new CreateTableEntityCommand(
                new CreateCommentCommand { TableEntityFactory = tableEntityFactory });

            ICommand createFavoriteCommand = new CreateTableEntityCommand(
                new CreateFavoriteCommand { TableEntityFactory = tableEntityFactory });

            foreach (var testImage in GetTestImages())
            {
                using (Stream stream = File.OpenRead(testImage))
                {
                    // upload
                    dynamic pTumblr = new ExpandoObject();

                    // Blob
                    pTumblr.ContainerOfMedia = await _azureStorage.GetContainerReferenceAsync(LvConstants.ContainerNameOfImage);
                    pTumblr.ContainerOfThumbnail = await _azureStorage.GetContainerReferenceAsync(LvConstants.ContainerNameOfThumbnail);
                    pTumblr.StreamOfMedia = stream;
                    pTumblr.BlobNameOfMedia = Path.GetFileName(testImage);

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
                    // Create Tumblr End

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
                    // Create Comment End

                    // Create Favorite
                    dynamic pFavorite = tumblrEntity.ToExpandoObject();
                    pFavorite.UserId = "bjutales";
                    pFavorite.Table = await _azureStorage.GetTableReferenceAsync(LvConstants.TableNameOfFavorite);
                    await createFavoriteCommand.ExecuteAsync(pFavorite);
                    // Create Favorite End
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