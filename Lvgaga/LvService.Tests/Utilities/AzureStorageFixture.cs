using System;
using LvModel.Common;
using LvService.Commands.Azure.Storage.Blob;
using LvService.Commands.Azure.Storage.Table;
using LvService.Commands.Tumblr;
using LvService.DbContexts;
using LvService.Factories.Azure.Storage;
using LvService.Factories.Uri;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Tests.Utilities
{
    public class AzureStorageFixture
    {
        public IAzureStorage AzureStorage { get; private set; }

        // const
        public const string ContainerPrefix = "c";
        public const string TablePrefix = "t";

        // UriFactory
        public UriFactory UriFactory = new UriFactory();

        // Container
        public DeleteContainerCommand DeleteContainerCommand = new DeleteContainerCommand();

        // Blob
        public UploadFromStreamCommand UploadFromStreamCommand = new UploadFromStreamCommand();
        public DownloadToStreamCommand DownloadToStreamCommand = new DownloadToStreamCommand();
        public DeleteBlobCommand DeleteBlobCommand = new DeleteBlobCommand();

        // Table
        public DeleteTableCommand DeleteTableCommand = new DeleteTableCommand();

        // Entity
        public CreateTableEntityCommand CreateTableEntityCommand = new CreateTableEntityCommand();
        public CreateTableEntitiesCommand CreateTableEntitiesCommand = new CreateTableEntitiesCommand();
        public ReadTableEntityCommand ReadTableEntityCommand = new ReadTableEntityCommand();
        public ReadTableEntitiesCommand ReadTableEntitiesCommand = new ReadTableEntitiesCommand();
        public UpdateTableEntityCommand UpdateTableEntityCommand = new UpdateTableEntityCommand();
        public UpdateTableEntitiesCommand UpdateTableEntitiesCommand = new UpdateTableEntitiesCommand();
        public DeleteTableEntityCommand DeleteTableEntityCommand = new DeleteTableEntityCommand();
        public DeleteTableEntitiesCommand DeleteTableEntitiesCommand = new DeleteTableEntitiesCommand();

        // Tumblr
        public CreateTumblrCommand CreateTumblrCommand = new CreateTumblrCommand
        {
            TableEntityFactory = new TableEntityFactory(new UriFactory()),
            UriFactory = new UriFactory()
        };

        public ReadTableEntitiesCommand ReadTumblrEntityWithCategoryCommand =
            new ReadTableEntitiesCommand(new ReadTumblrEntitiesWithCategoryCommand());

        // Comment
        public CreateCommentCommand CreateCommentCommand = new CreateCommentCommand
        {
            TableEntityFactory = new TableEntityFactory(new UriFactory()),
            UriFactory = new UriFactory()
        };

        // Favorite
        public CreateFavoriteCommand CreateFavoriteCommand = new CreateFavoriteCommand
        {
            TableEntityFactory = new TableEntityFactory(new UriFactory()),
            UriFactory = new UriFactory()
        };

        public AzureStorageFixture()
        {
            //AzureStorage = new AzureStorageDb();
            AzureStorage = new AzureStoragePool(new AzureStorageDb(CloudStorageAccount.DevelopmentStorageAccount));
        }

        public string GetRandomName(string prefix)
        {
            return String.Format("{0}{1}", prefix, Guid.NewGuid().ToString().Replace("-", ""));
        }

        public string GetTableFilterByPartitionKey(string partitionKey)
        {
            return TableQuery.GenerateFilterCondition(LvConstants.PartitionKey, QueryComparisons.Equal, partitionKey);
        }
    }
}