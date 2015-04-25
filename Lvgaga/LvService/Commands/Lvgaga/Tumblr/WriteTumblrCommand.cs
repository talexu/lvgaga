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
        private readonly ICommand _generateThumbnailCommand;
        private readonly ICommand _createTumblrCommand;
        private readonly ICommand _createTableEntitiesCommand;

        public WriteTumblrCommand(IAzureStorage azureStorage, ICommand uploadBlobCommand, ICommand generateThumbnailCommand, ICommand createTumblrCommand, ICommand createTableEntitiesCommand)
        {
            _azureStorage = azureStorage;
            _uploadBlobCommand = uploadBlobCommand;
            _generateThumbnailCommand = generateThumbnailCommand;
            _createTumblrCommand = createTumblrCommand;
            _createTableEntitiesCommand = createTableEntitiesCommand;
        }

        public bool CanExecute(dynamic p)
        {
            return true;
        }

        public async Task ExecuteAsync(dynamic p)
        {
            await _uploadBlobCommand.ExecuteAsync(p);
            p.MediaUri = p.BlobUri;
            await _generateThumbnailCommand.ExecuteAsync(p);
            p.Container = await _azureStorage.GetContainerReferenceAsync(LvConstants.ContainerNameOfThumbnail);
            await _uploadBlobCommand.ExecuteAsync(p);
            p.ThumbnailUri = p.BlobUri;
            await _createTumblrCommand.ExecuteAsync(p);
            await _createTableEntitiesCommand.ExecuteAsync(p);
        }
    }
}