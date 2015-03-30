using System;
using System.Dynamic;
using System.Text.RegularExpressions;
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

        public string CreateInvertedTicksByTumblrRowKey(string rowKey)
        {
            return rowKey.SubstringByUnderline(1);
        }

        // Comment - InvertedTicks_MediaType
        public string CreateCommentPartitionKey(string invertedTicks, MediaType mediaType)
        {
            return new[] { invertedTicks, mediaType.ToString("D") }.JoinByUnderline();
        }

        public string CreateInvertedTicksByCommentPartitionKey(string partitionKey)
        {
            return partitionKey.SubstringByUnderline(0);
        }

        // Favorite - MediaType_InvertedTicks
        public string CreateFavoriteRowKey(MediaType mediaType, string invertedTicks)
        {
            return new[] { mediaType.ToString("D"), invertedTicks }.JoinByUnderline();
        }

        public string CreateInvertedTicksByFavoriteRowKey(string rowKey)
        {
            return rowKey.SubstringByUnderline(1);
        }
    }
}