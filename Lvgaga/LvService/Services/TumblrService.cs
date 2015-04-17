using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using LvModel.Azure.StorageTable;
using LvModel.Common;
using LvModel.View.Tumblr;
using LvService.Commands.Azure.Storage.Table;
using LvService.DbContexts;
using LvService.Factories.ViewModel;

namespace LvService.Services
{
    public class TumblrService : ITumblrService
    {
        private readonly IAzureStorage _azureStorage;
        private readonly ITableEntityCommand _entityCommand;
        private readonly ITableEntitiesCommand _entitiesCommand;
        private readonly ITumblrFactory _tumblrFactory;
        private readonly ISasService _sasService;

        public TumblrService()
        {

        }

        public TumblrService(IAzureStorage azureStorage, ITableEntityCommand entityCommand,
            ITableEntitiesCommand entitiesCommand, ITumblrFactory tumblrFactory, ISasService sasService)
        {
            _azureStorage = azureStorage;
            _entityCommand = entityCommand;
            _entitiesCommand = entitiesCommand;
            _tumblrFactory = tumblrFactory;
            _sasService = sasService;
        }

        public async Task<TumblrModel> GetTumblrModelAsync(string partitionKey, string rowKey)
        {
            dynamic p = new ExpandoObject();
            p.Table = await _azureStorage.GetTableReferenceAsync(LvConstants.TableNameOfTumblr);
            p.PartitionKey = partitionKey;
            p.RowKey = rowKey;

            return _tumblrFactory.CreateTumblrModel(await _entityCommand.ExecuteAsync<TumblrEntity>(p));
        }

        public async Task<TumblrsModel> GetTumblrModelsAsync(string partitionKey, TumblrCategory category, int takeCount,
            string userId)
        {
            dynamic p = new ExpandoObject();
            p.Table = await _azureStorage.GetTableReferenceAsync(LvConstants.TableNameOfTumblr);
            p.PartitionKey = partitionKey;
            p.Category = category;
            p.TakeCount = takeCount;

            List<TumblrModel> tumblrs =
                _tumblrFactory.CreateTumblrModels(await _entitiesCommand.ExecuteAsync<TumblrEntity>(p));
            var model = new TumblrsModel
            {
                Tumblrs = tumblrs,
                Sas = await _sasService.GetSasForTable(LvConstants.TableNameOfTumblr),
                ContinuationToken = p.ContinuationToken
            };
            if (!String.IsNullOrEmpty(userId))
            {
                model.FavoriteSas = await _sasService.GetSasForTable(LvConstants.TableNameOfFavorite, userId);
            }

            return model;
        }
    }
}