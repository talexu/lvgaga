using System.Dynamic;
using System.Threading.Tasks;
using LvModel.Azure.StorageTable;
using LvModel.Common;
using LvModel.View.Tumblr;
using LvService.Commands.Common;
using LvService.DbContexts;
using LvService.Factories.Uri;
using LvService.Utilities;

namespace LvService.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IAzureStorage _azureStorage;
        private readonly ICommand _readEntityCommand;
        private readonly ICommand _createFavoriteCommand;
        private readonly ICommand _deleteFavoriteCommand;
        private readonly IUriFactory _uriFactory;

        public FavoriteService(
            IAzureStorage azureStorage,
            ICommand readEntityCommand,
            ICommand createFavoriteCommand,
            ICommand deleteFavoriteCommand,
            IUriFactory uriFactory)
        {
            _azureStorage = azureStorage;
            _readEntityCommand = readEntityCommand;
            _createFavoriteCommand = createFavoriteCommand;
            _deleteFavoriteCommand = deleteFavoriteCommand;
            _uriFactory = uriFactory;
        }

        public async Task<FavoriteEntity> CreateFavoriteAsync(string userId, string partitionKey, string rowKey)
        {
            dynamic p = await GetFavoriteExpandoObjectAsync(userId, partitionKey, rowKey);
            if (p == null) return null;

            await _createFavoriteCommand.ExecuteAsync(p);
            return p.Entity;
        }

        public async Task DeleteFavoriteAsync(string userId, string partitionKey, string rowKey)
        {
            dynamic p = new ExpandoObject();
            p.Table = await _azureStorage.GetTableReferenceAsync(LvConstants.TableNameOfFavorite);
            p.UserId = userId;
            p.RowKey = rowKey;
            p.MediaType = partitionKey;

            await _deleteFavoriteCommand.ExecuteAsync(p);
        }

        private async Task<TumblrEntity> GetTumblrEntityAsync(string partitionKey, string rowKey)
        {
            dynamic p = new ExpandoObject();
            p.Table = await _azureStorage.GetTableReferenceAsync(LvConstants.TableNameOfTumblr);
            p.PartitionKey = partitionKey;
            p.RowKey = rowKey;

            await _readEntityCommand.ExecuteAsync(p);
            return p.Entity;
        }

        private async Task<ExpandoObject> GetFavoriteExpandoObjectAsync(string userId, string partitionKey, string rowKey)
        {
            var rowKeyAll = _uriFactory.CreateTumblrRowKey(TumblrCategory.All, rowKey);
            var tumblr = await GetTumblrEntityAsync(partitionKey, rowKeyAll);
            if (tumblr == null) return null;

            dynamic p = tumblr.ToExpandoObject();
            p.RowKey = _uriFactory.GetInvertedTicksFromTumblrRowKey(p.RowKey);
            p.UserId = userId;
            p.Table = await _azureStorage.GetTableReferenceAsync(LvConstants.TableNameOfFavorite);

            return p;
        }
    }
}