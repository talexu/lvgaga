using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using LvModel.Azure.StorageTable;
using LvModel.Common;
using LvModel.View.Tumblr;
using LvService.Commands.Azure.Storage.Table;
using LvService.Commands.Common;
using LvService.DbContexts;
using LvService.Factories.Uri;
using LvService.Utilities;

namespace LvService.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IAzureStorage _azureStorage;
        private readonly ITableEntityCommand _entityReaderCommand;
        private readonly ICommand _createFavoriteCommand;
        private readonly ITableEntitiesCommand _deleteFavoriteCommand;
        private readonly IUriFactory _uriFactory;

        public FavoriteService(IAzureStorage azureStorage, ITableEntityCommand entityReaderCommand, ICommand createFavoriteCommand, ITableEntitiesCommand deleteFavoriteCommand, IUriFactory uriFactory)
        {
            _azureStorage = azureStorage;
            _entityReaderCommand = entityReaderCommand;
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

        public Task<List<FavoriteEntity>> GetFavoriteTumblrModelsAsync(string userId, MediaType mediaType, int takeCount)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteFavoriteAsync(string userId, string partitionKey, string rowKey)
        {
            dynamic p = new ExpandoObject();
            p.Table = await _azureStorage.GetTableReferenceAsync(LvConstants.TableNameOfFavorite);
            p.PartitionKey = userId;
            p.RowKey = rowKey;
            p.MediaType = partitionKey;

            await _deleteFavoriteCommand.ExecuteAsync<FavoriteEntity>(p);
        }

        private async Task<TumblrEntity> GetTumblrEntityAsync(string partitionKey, string rowKey)
        {
            dynamic p = new ExpandoObject();
            p.Table = await _azureStorage.GetTableReferenceAsync(LvConstants.TableNameOfTumblr);
            p.PartitionKey = partitionKey;
            p.RowKey = rowKey;

            return await _entityReaderCommand.ExecuteAsync<TumblrEntity>(p);
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