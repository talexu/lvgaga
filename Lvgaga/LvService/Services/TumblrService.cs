using System.Dynamic;
using System.Threading.Tasks;
using LvModel.Azure.StorageTable;
using LvModel.Common;
using LvModel.View.Tumblr;
using LvService.Commands.Common;
using LvService.DbContexts;

namespace LvService.Services
{
    public class TumblrService : ITumblrService
    {
        private readonly IAzureStorage _azureStorage;
        private readonly ICommand _readEntityCommand;
        private readonly ICommand _readEntitiesCommand;
        private readonly ISasService _sasService;

        public TumblrService()
        {

        }

        public TumblrService(
            IAzureStorage azureStorage,
            ICommand readEntityCommand,
            ICommand readEntitiesCommand,
            ISasService sasService)
        {
            _azureStorage = azureStorage;
            _readEntityCommand = readEntityCommand;
            _readEntitiesCommand = readEntitiesCommand;
            _sasService = sasService;
        }

        public async Task<TumblrEntity> GetTumblrEntityAsync(string partitionKey, string rowKey)
        {
            dynamic p = new ExpandoObject();
            p.Table = await _azureStorage.GetTableReferenceAsync(LvConstants.TableNameOfTumblr);
            p.PartitionKey = partitionKey;
            p.RowKey = rowKey;

            await _readEntityCommand.ExecuteAsync(p);
            return p.Entity;
        }

        public async Task<TumblrsModel> GetTumblrsModelAsync(string partitionKey, TumblrCategory category, int takeCount)
        {
            dynamic p = new ExpandoObject();
            p.Table = await _azureStorage.GetTableReferenceAsync(LvConstants.TableNameOfTumblr);
            p.PartitionKey = partitionKey;
            p.Category = category;
            p.TakeCount = takeCount;

            await _readEntitiesCommand.ExecuteAsync(p);

            return new TumblrsModel
            {
                Tumblrs = p.Entities,
                Sas = await _sasService.GetSasForTable(LvConstants.TableNameOfTumblr),
                ContinuationToken = p.ContinuationToken
            };
        }
    }
}