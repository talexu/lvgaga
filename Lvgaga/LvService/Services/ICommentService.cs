using System.Threading.Tasks;
using LvModel.View.Comment;

namespace LvService.Services
{
    public interface ICommentService
    {
        Task<CommentModel> GetCommentsAsync(string partitionKey, string rowKey, int takeCount);
    }
}