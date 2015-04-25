using System;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using LvService.Commands.Common;
using LvService.Commands2.Azure.Storage.Blob;
using LvService.DbContexts;
using LvService.Tests.Utilities;
using LvService.Utilities;
using Microsoft.WindowsAzure.Storage;
using Xunit;

namespace LvService.Tests.Commands.Azure.Storage.Blob
{
    public class BlobCrudTests : IClassFixture<AzureStorageFixture>
    {
        private const string ContainerPrefix = "c";
        private readonly string _tempContainerName;

        protected readonly AzureStorageFixture Fixture;

        public BlobCrudTests(AzureStorageFixture fixture)
        {
            Fixture = fixture;
            _tempContainerName = TestDataGenerator.GetRandomName(ContainerPrefix);
        }

        [Fact]
        public async Task Crud()
        {
            ICommand uploadCommand = new UploadBlobFromStreamCommand();
            ICommand downloadCommand = new DownloadBlobToStreamCommand();
            ICommand deleteBlobCommand = new DeleteBlobCommand();
            ICommand deleteContainerCommand = new DeleteContainerCommand();

            var fileName = Guid.NewGuid() + ".txt";
            var content = DateTime.Now.ToString(CultureInfo.CurrentCulture);

            // upload
            dynamic p = new ExpandoObject();
            p.Container =
                await Fixture.AzureStorage.GetContainerReferenceAsync(_tempContainerName);
            p.Stream = content.ToMemoryStream();
            p.BlobName = fileName;
            await uploadCommand.ExecuteAsync(p);

            // download
            dynamic pd = new ExpandoObject();
            pd.Container =
                await Fixture.AzureStorage.GetContainerReferenceAsync(_tempContainerName);
            pd.BlobName = fileName;
            await downloadCommand.ExecuteAsync(pd);
            MemoryStream ms = pd.Stream;
            var download = ms.ToStringFromMemoryStream();
            Assert.Equal(content, download);

            // delete
            dynamic pdd = new ExpandoObject();
            pdd.Container =
                await Fixture.AzureStorage.GetContainerReferenceAsync(_tempContainerName);
            pdd.BlobName = fileName;
            await deleteBlobCommand.ExecuteAsync(pdd);

            await deleteContainerCommand.ExecuteAsync(p);
        }
    }
}