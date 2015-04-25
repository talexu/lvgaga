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

        public AzureStorageFixture()
        {
            AzureStorage = new AzureStorageDb(CloudStorageAccount.DevelopmentStorageAccount);
        }
    }
}