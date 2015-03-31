using System;
using LvModel.Common;
using LvModel.View.Tumblr;
using LvService.Utilities;

namespace LvService.Factories.Uri
{
    public class UriFactory : IUriFactory
    {
        public string Scheme { get; set; }
        public int Port { get; set; }
        public string Host { get; set; }

        private readonly UriBuilder _uriBuilder;

        public UriFactory()
        {
            Scheme = "http";
            Port = 80;
            Host = "www.lvgaga.com";
            _uriBuilder = new UriBuilder
            {
                Scheme = Scheme,
                Port = Port,
                Host = Host
            };
        }

        public string CreateUri(string path)
        {
            _uriBuilder.Path = path;
            return _uriBuilder.Uri.AbsolutePath;
        }

        // Tumblr - TumblrCategory_InvertedTicks
        public string CreateTumblrRowKey(TumblrCategory category, string invertedTicks)
        {
            return new[] { category.ToString("D"), invertedTicks }.JoinByUnderline();
        }

        public string GetInvertedTicksFromTumblrRowKey(string rowKey)
        {
            return rowKey.SubstringByUnderline(1);
        }

        public string ReplaceTumblrCategoryOfRowKey(string rowKey, TumblrCategory category)
        {
            return CreateTumblrRowKey(category, GetInvertedTicksFromTumblrRowKey(rowKey));
        }

        // Comment - InvertedTicks_MediaType
        public string CreateCommentPartitionKey(string invertedTicks, MediaType mediaType)
        {
            return new[] { invertedTicks, mediaType.ToString("D") }.JoinByUnderline();
        }

        public string GetInvertedTicksFromCommentPartitionKey(string partitionKey)
        {
            return partitionKey.SubstringByUnderline(0);
        }

        // Favorite - MediaType_InvertedTicks
        public string CreateFavoriteRowKey(MediaType mediaType, string invertedTicks)
        {
            return CreateFavoriteRowKey(mediaType.ToString("D"), invertedTicks);
        }

        public string CreateFavoriteRowKey(string mediaType, string invertedTicks)
        {
            return new[] { mediaType, invertedTicks }.JoinByUnderline();
        }

        public string GetInvertedTicksFromFavoriteRowKey(string rowKey)
        {
            return rowKey.SubstringByUnderline(1);
        }
    }
}