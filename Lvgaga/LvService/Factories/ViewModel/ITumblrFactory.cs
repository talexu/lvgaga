using System.Collections.Generic;
using LvModel.Azure.StorageTable;
using LvModel.View.Tumblr;

namespace LvService.Factories.ViewModel
{
    public interface ITumblrFactory
    {
        TumblrModel CreateTumblrModel(TumblrEntity tumblrEntity);
        List<TumblrModel> CreateTumblrModels(IEnumerable<TumblrEntity> tumblrEntities);
    }
}