using System;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using LvService.Tests.Utilities;
using LvService.Utilities;
using Xunit;

namespace LvService.Tests.Commands.Azure.Storage.Blob
{
    public class BlobCrdCommandTests : AzureStorageTestsBase
    {
        private readonly string _tempContainerName;

        public BlobCrdCommandTests(AzureStorageFixture fixture)
            : base(fixture)
        {
            _tempContainerName = fixture.GetRandomName(AzureStorageFixture.ContainerPrefix);
        }

        [Fact]
        public async Task UploadFromStreamAsyncTest()
        {
            var fileName = Guid.NewGuid() + ".txt";
            var content = DateTime.Now.ToString(CultureInfo.CurrentCulture);

            // upload
            dynamic p = new ExpandoObject();
            p.Container =
                await Fixture.AzureStorage.GetContainerReferenceAsync(_tempContainerName);
            p.Stream = content.ToMemoryStream();
            p.BlobName = fileName;
            await Fixture.UploadFromStreamCommand.ExecuteAsync(p);

            // download
            dynamic pd = new ExpandoObject();
            pd.Container =
                await Fixture.AzureStorage.GetContainerReferenceAsync(_tempContainerName);
            pd.BlobName = fileName;
            await Fixture.DownloadToStreamCommand.ExecuteAsync(pd);
            MemoryStream ms = pd.Stream;
            var download = ms.ToStringFromMemoryStream();
            Assert.Equal(content, download);

            // delete
            dynamic pdd = new ExpandoObject();
            pdd.Container =
                await Fixture.AzureStorage.GetContainerReferenceAsync(_tempContainerName);
            pdd.BlobName = fileName;
            await Fixture.DeleteBlobCommand.ExecuteAsync(pdd);
            await Fixture.DownloadToStreamCommand.ExecuteAsync(pd);
            var download2 = pd.Stream;
            Assert.Null(download2);

            Fixture.DeleteContainerCommand.ExecuteAsync(p);
        }
    }
}