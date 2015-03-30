using LvModel.Common;
using LvModel.View.Tumblr;

namespace LvService.Factories.Uri
{
    public interface IUriFactory
    {
        string CreateUri(string path);

        string CreateTumblrRowKey(TumblrCategory category, string invertedTicks);
        string CreateInvertedTicksByTumblrRowKey(string rowKey);

        string CreateCommentPartitionKey(string invertedTicks, MediaType mediaType);
        string CreateInvertedTicksByCommentPartitionKey(string partitionKey);

        string CreateFavoriteRowKey(MediaType mediaType, string invertedTicks);
        string CreateInvertedTicksByFavoriteRowKey(string rowKey);
    }
}