using System.Threading.Tasks;
using LvModel.Azure.StorageTable;
using LvModel.Common;
using LvModel.View.Comment;
using LvModel.View.Tumblr;
using LvService.Commands.Common;
using LvService.DbContexts;
using LvService.Factories.Uri;
using LvService.Utilities;

namespace LvService.Services
{
    public class SasCommentService : ICommentService
    {
        private readonly IAzureStorage _azureStorage;
        private readonly ICommand _createCommentCommand;
        private readonly ITumblrService _tumblrService;
        private readonly IUriFactory _uriFactory;
        private readonly ISasService _sasService;

        public SasCommentService(
            IAzureStorage azureStorage,
            ICommand createCommentCommand,
            ITumblrService tumblrService,
            IUriFactory uriFactory,
            ISasService sasService)
        {
            _azureStorage = azureStorage;
            _createCommentCommand = createCommentCommand;
            _tumblrService = tumblrService;
            _uriFactory = uriFactory;
            _sasService = sasService;
        }

        public async Task<CommentEntity> CreateCommentAsync(string partitionKey, PostedComment comment)
        {
            dynamic p = comment.ToExpandoObject();
            p.PartitionKey = partitionKey;
            p.Table = await _azureStorage.GetTableReferenceAsync(LvConstants.TableNameOfComment);

            await _createCommentCommand.ExecuteAsync(p);
            return p.Entity;
        }

        public async Task<CommentModel> GetCommentModelsAsync(string partitionKey, string rowKey, int takeCount)
        {
            var rowKeyAll = _uriFactory.CreateTumblrRowKey(TumblrCategory.All, rowKey);
            var tumblr =
                await
                    _tumblrService.GetTumblrEntityAsync(partitionKey, rowKeyAll);
            if (tumblr == null) return null;

            var model = tumblr.CloneByJson<TumblrEntity, CommentModel>();
            model.Sas = await _sasService.GetSasForTable(LvConstants.TableNameOfComment, rowKey);

            return model;
        }
    }
}