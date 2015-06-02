using System.IO;
using System.Threading.Tasks;
using LvModel.Common;
using LvService.Commands.Common;
using LvService.DbContexts;

namespace LvService.Commands.Lvgaga.Tumblr
{
    public class WriteTumblrCommand : ICommand
    {
        private readonly IAzureStorage _azureStorage;
        private readonly ICommand _uploadBlobCommand;

        private readonly ICommand _generateLCommand;
        private readonly ICommand _generateMCommand;
        private readonly ICommand _generateSCommand;

        private readonly ICommand _createTumblrCommand;
        private readonly ICommand _createTableEntitiesCommand;

        public WriteTumblrCommand(IAzureStorage azureStorage, ICommand uploadBlobCommand, ICommand generateLCommand, ICommand generateMCommand, ICommand generateSCommand, ICommand createTumblrCommand, ICommand createTableEntitiesCommand)
        {
            _azureStorage = azureStorage;
            _uploadBlobCommand = uploadBlobCommand;
            _generateLCommand = generateLCommand;
            _generateMCommand = generateMCommand;
            _generateSCommand = generateSCommand;
            _createTumblrCommand = createTumblrCommand;
            _createTableEntitiesCommand = createTableEntitiesCommand;
        }

        public bool CanExecute(dynamic p)
        {
            return true;
        }

        public async Task ExecuteAsync(dynamic p)
        {
            Stream original = p.Stream;

            await _uploadBlobCommand.ExecuteAsync(p);
            p.MediaUri = p.BlobUri;

            p.Stream = original;
            await _generateLCommand.ExecuteAsync(p);
            p.Container = await _azureStorage.GetContainerReferenceAsync(LvConstants.ContainerNameOfLargeImage);
            await _uploadBlobCommand.ExecuteAsync(p);
            p.MediaLargeUri = p.BlobUri;

            p.Stream = original;
            await _generateMCommand.ExecuteAsync(p);
            p.Container = await _azureStorage.GetContainerReferenceAsync(LvConstants.ContainerNameOfMediumImage);
            await _uploadBlobCommand.ExecuteAsync(p);
            p.MediaMediumUri = p.BlobUri;

            p.Stream = original;
            await _generateSCommand.ExecuteAsync(p);
            p.Container = await _azureStorage.GetContainerReferenceAsync(LvConstants.ContainerNameOfSmallImage);
            await _uploadBlobCommand.ExecuteAsync(p);
            p.MediaSmallUri = p.BlobUri;

            await _createTumblrCommand.ExecuteAsync(p);
            await _createTableEntitiesCommand.ExecuteAsync(p);
        }
    }
}