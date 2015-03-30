using System;
using System.Dynamic;
using System.Text.RegularExpressions;
using LvModel.Common;
using LvModel.View.Tumblr;

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

        // Tumblr - MediaType_InvertedTicks
        public string GetTumblrRowKey(TumblrCategory category, string invertedTicks)
        {
            return CombineDoubleValueByUnderline(category.ToString("D"), invertedTicks);
        }

        public string GetInvertedTicksFromTumblrRowKey(string rowKey)
        {
            dynamic r = DoubleSplitByUnderline(rowKey);
            return r.G2;
        }

        // Comment - InvertedTicks_MediaType
        public string GetCommentPartitionKey(string invertedTicks, MediaType mediaType)
        {
            return CombineDoubleValueByUnderline(invertedTicks, mediaType.ToString("D"));
        }

        public string GetInvertedTicksFromCommentPartitionKey(string partitionKey)
        {
            dynamic r = DoubleSplitByUnderline(partitionKey);
            return r.G1;
        }

        // Favorite - InvertedTicks_MediaType
        public string GetFavoriteRowKey(string invertedTicks, MediaType mediaType)
        {
            return CombineDoubleValueByUnderline(invertedTicks, mediaType.ToString("D"));
        }

        public string GetInvertedTicksFromFavoriteRowKey(string rowKey)
        {
            dynamic r = DoubleSplitByUnderline(rowKey);
            return r.G1;
        }

        // Utilities
        private static ExpandoObject DoubleSplitByUnderline(string str)
        {
            var reg = new Regex(@"^(.+?)_(.+?)$");
            dynamic p = new ExpandoObject();

            var g = reg.Match(str);
            p.G1 = g.Groups[1].Value;
            p.G2 = g.Groups[2].Value;

            return p;
        }

        private static string CombineDoubleValueByUnderline(string v1, string v2)
        {
            return string.Format("{0}_{1}", v1, v2);
        }
    }
}