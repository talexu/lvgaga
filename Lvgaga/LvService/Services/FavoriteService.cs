using System;
using System.Collections.Generic;
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
        private readonly ICommand _createFavoriteCommand;
        private readonly ITumblrService _tumblrService;
        private readonly IUriFactory _uriFactory;

        public FavoriteService(IAzureStorage azureStorage, ICommand createFavoriteCommand, ITumblrService tumblrService, IUriFactory uriFactory)
        {
            _azureStorage = azureStorage;
            _createFavoriteCommand = createFavoriteCommand;
            _tumblrService = tumblrService;
            _uriFactory = uriFactory;
        }

        public async Task<FavoriteEntity> CreateFavoriteAsync(string userId, string partitionKey, string rowKey)
        {
            var rowKeyAll = _uriFactory.CreateTumblrRowKey(TumblrCategory.All, rowKey);
            var tumblr =
                await
                    _tumblrService.GetTumblrModelAsync(partitionKey, rowKeyAll);
            if (tumblr == null) return null;

            dynamic p = tumblr.ToExpandoObject();
            p.UserId = userId;
            p.Table = await _azureStorage.GetTableReferenceAsync(LvConstants.TableNameOfFavorite);

            await _createFavoriteCommand.ExecuteAsync(p);
            return p.Entity;
        }

        public Task<List<FavoriteEntity>> GetFavoriteTumblrModelsAsync(string userId, MediaType mediaType, int takeCount)
        {
            throw new NotImplementedException();
        }
    }
}