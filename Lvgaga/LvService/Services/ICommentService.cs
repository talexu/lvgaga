using System.Threading.Tasks;
using LvModel.View.Comment;

namespace LvService.Services
{
    public interface ICommentService
    {
        Task<bool> CreateCommentAsync(string partitionKey, PostedComment comment);
        Task<CommentModel> GetCommentsAsync(string partitionKey, string rowKey, int takeCount);
    }
}