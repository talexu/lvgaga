using System.Dynamic;
using System.Threading.Tasks;
using LvModel.Azure.StorageTable;
using LvModel.Common;
using LvModel.View.Comment;
using LvModel.View.Tumblr;
using LvService.Commands.Azure.Storage.Table;
using LvService.DbContexts;
using LvService.Factories.Uri;
using LvService.Factories.ViewModel;

namespace LvService.Services
{
    public class CommentService : ICommentService
    {
        private readonly IAzureStorage _azureStorage;
        private readonly ITumblrService _tumblrService;
        private readonly ITableEntitiesCommand _entitiesCommand;
        private readonly ICommentFactory _commentFactory;

        public CommentService(IAzureStorage azureStorage, ITumblrService tumblrService, ITableEntitiesCommand entitiesCommand, ICommentFactory commentFactory)
        {
            _azureStorage = azureStorage;
            _tumblrService = tumblrService;
            _entitiesCommand = entitiesCommand;
            _commentFactory = commentFactory;
        }

        public async Task<CommentModel> GetCommentsAsync(string partitionKey, string rowKey, int takeCount)
        {
            var rowKeyAll = UriFactory.GetTumblrRowKey(TumblrCategory.All, rowKey);
            var tumblr =
                await
                    _tumblrService.GetTumblrModelAsync(partitionKey, rowKeyAll);
            if (tumblr == null) return null;

            dynamic p = new ExpandoObject();
            p.Table = await _azureStorage.GetTableReferenceAsync(LvConstants.TableNameOfComment);
            p.PartitionKey = rowKey;
            p.TakeCount = takeCount;

            return _commentFactory.CreateCommentModels(tumblr, await _entitiesCommand.ExecuteAsync<CommentEntity>(p));
        }
    }
}