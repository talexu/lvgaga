using LvModel.Common;
using LvModel.View.Tumblr;

namespace LvService.Factories.Uri
{
    public interface IUriFactory
    {
        string CreateUri(string path);

        string CreateTumblrRowKey(TumblrCategory category, string invertedTicks);
        string GetInvertedTicksFromTumblrRowKey(string rowKey);
        string ReplaceTumblrCategoryOfRowKey(string rowKey, TumblrCategory category);

        string CreateCommentPartitionKey(string invertedTicks, MediaType mediaType);
        string GetInvertedTicksFromCommentPartitionKey(string partitionKey);

        string CreateFavoriteRowKey(MediaType mediaType, string invertedTicks);
        string CreateFavoriteRowKey(string mediaType, string invertedTicks);
        string GetInvertedTicksFromFavoriteRowKey(string rowKey);
    }
}