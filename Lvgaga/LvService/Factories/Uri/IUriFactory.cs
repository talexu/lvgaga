using LvModel.Common;
using LvModel.View.Tumblr;

namespace LvService.Factories.Uri
{
    public interface IUriFactory
    {
        string CreateUri(string path);

        string GetTumblrRowKey(TumblrCategory category, string invertedTicks);
        string GetInvertedTicksFromTumblrRowKey(string rowKey);

        string GetCommentPartitionKey(string invertedTicks, MediaType mediaType);
        string GetInvertedTicksFromCommentPartitionKey(string partitionKey);

        string GetFavoriteRowKey(string invertedTicks, MediaType mediaType);
        string GetInvertedTicksFromFavoriteRowKey(string rowKey);
    }
}