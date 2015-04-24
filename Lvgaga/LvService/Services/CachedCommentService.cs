using System.Threading.Tasks;
using LvModel.Azure.StorageTable;
using LvModel.View.Comment;
using LvService.Factories.Uri;

namespace LvService.Services
{
    public class CachedCommentService : ICommentService
    {
        private const string RegionOfComment = "commentmodel";

        private readonly ICommentService _commentService;
        private readonly ICache _cache;
        private readonly ICacheKeyFactory _cacheKeyFactory;

        public CachedCommentService(ICommentService commentService, ICache cache, ICacheKeyFactory cacheKeyFactory)
        {
            _commentService = commentService;
            _cache = cache;
            _cacheKeyFactory = cacheKeyFactory;
        }

        public async Task<CommentEntity> CreateCommentAsync(string partitionKey, PostedComment comment)
        {
            return await _commentService.CreateCommentAsync(partitionKey, comment);
        }

        public async Task<CommentModel> GetCommentModelsAsync(string partitionKey, string rowKey, int takeCount)
        {
            return
                await
                    _cache.Get(
                        _cacheKeyFactory.CreateKey(RegionOfComment, partitionKey, rowKey, takeCount.ToString()),
                        async () => await _commentService.GetCommentModelsAsync(partitionKey, rowKey, takeCount));
        }
    }
}