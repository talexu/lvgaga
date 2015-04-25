using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using LvModel.Common;
using LvModel.View.Tumblr;
using LvService.Commands.Common;
using LvService.DbContexts;
using LvService.Factories.ViewModel;

namespace LvService.Services
{
    public class TumblrService : ITumblrService
    {
        private readonly IAzureStorage _azureStorage;
        private readonly ICommand _readEntityCommand;
        private readonly ICommand _readEntitiesCommand;
        private readonly ITumblrFactory _tumblrFactory;
        private readonly ISasService _sasService;

        public TumblrService()
        {

        }

        public TumblrService(
            IAzureStorage azureStorage,
            ICommand readEntityCommand,
            ICommand readEntitiesCommand,
            ITumblrFactory tumblrFactory,
            ISasService sasService)
        {
            _azureStorage = azureStorage;
            _readEntityCommand = readEntityCommand;
            _readEntitiesCommand = readEntitiesCommand;
            _tumblrFactory = tumblrFactory;
            _sasService = sasService;
        }

        public async Task<TumblrModel> GetTumblrModelAsync(string partitionKey, string rowKey)
        {
            dynamic p = new ExpandoObject();
            p.Table = await _azureStorage.GetTableReferenceAsync(LvConstants.TableNameOfTumblr);
            p.PartitionKey = partitionKey;
            p.RowKey = rowKey;

            await _readEntityCommand.ExecuteAsync(p);
            return _tumblrFactory.CreateTumblrModel(p.Entity);
        }

        public async Task<TumblrsModel> GetTumblrModelsAsync(string partitionKey, TumblrCategory category, int takeCount)
        {
            dynamic p = new ExpandoObject();
            p.Table = await _azureStorage.GetTableReferenceAsync(LvConstants.TableNameOfTumblr);
            p.PartitionKey = partitionKey;
            p.Category = category;
            p.TakeCount = takeCount;

            await _readEntitiesCommand.ExecuteAsync(p);
            List<TumblrModel> tumblrs = _tumblrFactory.CreateTumblrModels(p.Entities);

            return new TumblrsModel
            {
                Tumblrs = tumblrs,
                Sas = await _sasService.GetSasForTable(LvConstants.TableNameOfTumblr),
                ContinuationToken = p.ContinuationToken
            };
        }
    }
}