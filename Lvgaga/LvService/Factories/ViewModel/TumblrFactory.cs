using System.Collections.Generic;
using LvModel.Azure.StorageTable;
using LvModel.View.Tumblr;

namespace LvService.Factories.ViewModel
{
    public class TumblrFactory : ITumblrFactory
    {

        public TumblrModel CreateTumblrModel(TumblrEntity tumblrEntity)
        {
            throw new System.NotImplementedException();
        }

        public List<TumblrModel> CreateTumblrModels(IEnumerable<TumblrEntity> tumblrEntities)
        {
            throw new System.NotImplementedException();
        }
    }
}