using System;
using System.Dynamic;
using System.Threading.Tasks;
using LvModel.Azure.StorageTable;
using LvModel.Common;
using LvModel.View.Comment;
using LvModel.View.Tumblr;
using LvService.Commands.Azure.Storage.Table;
using LvService.Commands.Common;
using LvService.DbContexts;
using LvService.Factories.Uri;
using LvService.Factories.ViewModel;
using LvService.Utilities;

namespace LvService.Services
{
    public class CommentService : ICommentService
    {
        private readonly IAzureStorage _azureStorage;
        private readonly ICommand _createCommentCommand;
        private readonly ITableEntityCommand _entityReaderCommand;
        private readonly ITableEntitiesCommand _entitiesReaderCommand;
        private readonly ITumblrService _tumblrService;

        private readonly ICommentFactory _commentFactory;
        private readonly IUriFactory _uriFactory;
        private readonly ISasService _sasService;

        public CommentService(IAzureStorage azureStorage, ICommand createCommentCommand, ITumblrService tumblrService,
            ITableEntityCommand entityCommand, ITableEntitiesCommand entitiesCommand, ICommentFactory commentFactory,
            IUriFactory uriFactory, ISasService sasService)
        {
            _azureStorage = azureStorage;
            _createCommentCommand = createCommentCommand;
            _tumblrService = tumblrService;
            _entityReaderCommand = entityCommand;
            _entitiesReaderCommand = entitiesCommand;
            _commentFactory = commentFactory;
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

        public async Task<CommentModel> GetCommentModelsAsync(string partitionKey, string rowKey, int takeCount, string userId = null)
        {
            var rowKeyAll = _uriFactory.CreateTumblrRowKey(TumblrCategory.All, rowKey);
            var tumblr =
                await
                    _tumblrService.GetTumblrModelAsync(partitionKey, rowKeyAll);
            if (tumblr == null) return null;

            dynamic p = new ExpandoObject();
            p.Table = await _azureStorage.GetTableReferenceAsync(LvConstants.TableNameOfComment);
            p.PartitionKey = rowKey;
            p.TakeCount = takeCount;

            CommentModel model = _commentFactory.CreateCommentModels(tumblr, await _entitiesReaderCommand.ExecuteAsync<CommentEntity>(p));
            model.Sas = await _sasService.GetSasForTable(LvConstants.TableNameOfComment, rowKey);
            if (!String.IsNullOrEmpty(userId))
            {
                model.FavoriteSas = await _sasService.GetSasForTable(LvConstants.TableNameOfFavorite, userId);
            }
            if (String.IsNullOrEmpty(userId)) return model;

            dynamic pf = new ExpandoObject();
            pf.Table = await _azureStorage.GetTableReferenceAsync(LvConstants.TableNameOfFavorite);
            pf.PartitionKey = userId;
            pf.RowKey = _uriFactory.CreateFavoriteRowKey(partitionKey, rowKey);

            var favorite = await _entityReaderCommand.ExecuteAsync<FavoriteEntity>(pf);
            model.IsFavorited = favorite != null;

            return model;
        }
    }
}