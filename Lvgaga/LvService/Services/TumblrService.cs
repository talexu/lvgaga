using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using LvModel.Azure.StorageTable;
using LvModel.Common;
using LvModel.View.Tumblr;
using LvService.Commands.Azure.Storage.Table;
using LvService.DbContexts;
using LvService.Factories.Uri;
using LvService.Factories.ViewModel;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Services
{
    public class TumblrService : ITumblrService
    {
        private readonly IAzureStorage _azureStorage;
        private readonly ITableEntitiesCommand _command;
        private readonly ITumblrFactory _tumblrFactory;

        public TumblrService()
        {
            _azureStorage = new AzureStoragePool(new AzureStorageDb());
            _command = new ReadTableEntitiesCommand();
            _tumblrFactory = new TumblrFactory(new UriFactory());
        }

        public TumblrService(IAzureStorage azureStorage, ITableEntitiesCommand command, ITumblrFactory tumblrFactory)
        {
            _azureStorage = azureStorage;
            _command = command;
            _tumblrFactory = tumblrFactory;
        }

        public async Task<List<TumblrModel>> GetTumblrModelsAsync(int takeCount)
        {
            dynamic p = new ExpandoObject();
            p.Table = await _azureStorage.GetTableReferenceAsync(Constants.TumblrTableName);
            p.Filter = TableQuery.GenerateFilterCondition(Constants.PartitionKey, QueryComparisons.Equal,
                Constants.MediaTypeImage);
            p.TakeCount = takeCount;

            var models = new List<TumblrModel>();
            foreach (var e in await _command.ExecuteAsync<TumblrEntity>(p))
            {
                models.Add(_tumblrFactory.CreateTumblrModel(e));
            }
            return models;
        }
    }
}