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

        public TumblrService()
        {

        }

        public TumblrService(IAzureStorage azureStorage, ITableEntityCommand entityCommand, ITableEntitiesCommand entitiesCommand, ITumblrFactory tumblrFactory)
        {
            _azureStorage = azureStorage;
            _entityCommand = entityCommand;
            _entitiesCommand = entitiesCommand;
            _tumblrFactory = tumblrFactory;
        }

        public async Task<TumblrModel> GetTumblrModelAsync(string partitionKey, string rowKey)
        {
            dynamic p = new ExpandoObject();
            p.Table = await _azureStorage.GetTableReferenceAsync(LvConstants.TableNameOfTumblr);
            p.PartitionKey = partitionKey;
            p.RowKey = rowKey;

            return _tumblrFactory.CreateTumblrModel(await _entityCommand.ExecuteAsync<TumblrEntity>(p));
        }

        public async Task<List<TumblrModel>> GetTumblrModelsAsync(string partitionKey, TumblrCategory category, int takeCount)
        {
            dynamic p = new ExpandoObject();
            p.Table = await _azureStorage.GetTableReferenceAsync(LvConstants.TableNameOfTumblr);
            p.PartitionKey = partitionKey;
            p.Category = category;
            p.TakeCount = takeCount;

            return _tumblrFactory.CreateTumblrModels(await _entitiesCommand.ExecuteAsync<TumblrEntity>(p));
        }
    }
}