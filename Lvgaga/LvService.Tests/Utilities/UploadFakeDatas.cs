using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LvModel.Common;
using LvService.Commands.Azure.Storage.Blob;
using LvService.Tests.Commands.Azure.Storage;
using Xunit;

namespace LvService.Tests.Utilities
{
    public class TumblrFactoryTests : IClassFixture<AzureStorageFixture>
    {
        private readonly AzureStorageFixture _azureStorageFixture;
        private readonly UploadFromStreamCommand _uploadFromStreamCommand;

        public TumblrFactoryTests(AzureStorageFixture azureStorageFixture)
        {
            _azureStorageFixture = azureStorageFixture;
            _uploadFromStreamCommand = new UploadFromStreamCommand();
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
    }
}