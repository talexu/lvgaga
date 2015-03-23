using System;
using System.Dynamic;
using System.Globalization;
using System.Threading.Tasks;
using LvModel.Common;
using LvService.Commands.Azure.Storage.Blob;
using LvService.Utilities;
using Xunit;

namespace LvService.Tests.Commands.Azure.Storage.Blob
{
    public class UploadFromStreamCommandTests : IClassFixture<AzureStorageFixture>
    {
        private readonly AzureStorageFixture _azureStorageFixture;
        private readonly UploadFromStreamCommand _uploadFromStreamCommand;
        private readonly DownloadToStreamCommand _downloadToStreamCommand;
        private readonly DeleteBlobCommand _deleteBlobCommand;

        public UploadFromStreamCommandTests(AzureStorageFixture azureStorageFixture)
        {
            _azureStorageFixture = azureStorageFixture;
            _uploadFromStreamCommand = new UploadFromStreamCommand();
            _downloadToStreamCommand = new DownloadToStreamCommand();
            _deleteBlobCommand = new DeleteBlobCommand();
        }

        [Fact]
        public async Task UploadFromStreamAsyncTest()
        {
            var fileName = Guid.NewGuid() + ".txt";
            var content = DateTime.Now.ToString(CultureInfo.CurrentCulture);

            // upload
            dynamic p = new ExpandoObject();
            p.Container =
                await _azureStorageFixture.AzureStorage.GetContainerReferenceAsync(Constants.ImageContainerName);
            p.Stream = content.ToMemoryStream();
            p.BlobName = fileName;
            await _uploadFromStreamCommand.ExecuteAsync(p);

            // download
            dynamic pd = new ExpandoObject();
            pd.Container =
                await _azureStorageFixture.AzureStorage.GetContainerReferenceAsync(Constants.ImageContainerName);
            pd.BlobName = fileName;
            await _downloadToStreamCommand.ExecuteAsync(pd);
            var download = pd.Result;
            Assert.Equal(content, download);

            // delete
            dynamic pdd = new ExpandoObject();
            pdd.Container =
                await _azureStorageFixture.AzureStorage.GetContainerReferenceAsync(Constants.ImageContainerName);
            pdd.BlobName = fileName;
            await _deleteBlobCommand.ExecuteAsync(pdd);
            await _downloadToStreamCommand.ExecuteAsync(pd);
            var download2 = pd.Result;
            Assert.Null(download2);
        }
    }
}