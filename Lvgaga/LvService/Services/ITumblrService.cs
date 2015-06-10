using System.Threading.Tasks;
using LvModel.Azure.StorageTable;
using LvModel.View.Tumblr;

namespace LvService.Services
{
    public interface ITumblrService
    {
        Task<TumblrEntity> GetTumblrEntityAsync(string partitionKey, string rowKey);
        Task<TumblrsModel> GetTumblrsModelAsync(string partitionKey, TumblrCategory category, int takeCount);
    }
}