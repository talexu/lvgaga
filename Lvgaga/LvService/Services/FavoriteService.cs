using System.Collections.Generic;
using System.Threading.Tasks;
using LvModel.Azure.StorageTable;
using LvModel.Common;
using LvModel.View.Tumblr;
using LvService.Utilities;

namespace LvService.Services
{
    public class FavoriteService : IFavoriteService
    {

        public Task<FavoriteEntity> CreateFavoriteAsync(string userId, TumblrModel tumblr)
        {
            dynamic p = tumblr.ToExpandoObject();
            p.UserId = userId;

            return null;
        }

        public Task<List<TumblrModel>> GetFavoriteTumblrModelsAsync(string userId, MediaType mediaType, int takeCount)
        {
            throw new System.NotImplementedException();
        }
    }
}