using System.Threading.Tasks;
using LvModel.Azure.StorageTable;
using LvModel.View.Comment;

namespace LvService.Services
{
    public interface ICommentService
    {
        Task<CommentEntity> CreateCommentAsync(string partitionKey, PostedComment comment);
        Task<CommentModel> GetCommentModelsAsync(string partitionKey, string rowKey, int takeCount, string userId);
    }
}