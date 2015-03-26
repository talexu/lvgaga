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
        private readonly ITableEntitiesCommand _command;
        private readonly ITumblrFactory _tumblrFactory;

        public TumblrService()
        {

        }

        public TumblrService(IAzureStorage azureStorage, ITableEntitiesCommand command, ITumblrFactory tumblrFactory)
        {
            _azureStorage = azureStorage;
            _command = command;
            _tumblrFactory = tumblrFactory;
        }

        public async Task<List<TumblrModel>> GetTumblrModelsAsync(string partitionKey, TumblrCategory category, int takeCount)
        {
            dynamic p = new ExpandoObject();
            p.Table = await _azureStorage.GetTableReferenceAsync(Constants.TumblrTableName);
            p.PartitionKey = partitionKey;
            p.Category = category;
            p.TakeCount = takeCount;

            return _tumblrFactory.CreateTumblrModels(await _command.ExecuteAsync<TumblrEntity>(p));
        }
    }
}