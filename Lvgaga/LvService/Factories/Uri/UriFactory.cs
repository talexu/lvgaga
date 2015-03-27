using System;
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

        public static string GetTumblrRowKey(TumblrCategory category, string invertedTicks)
        {
            return string.Format("{0}_{1}", category.ToString("D"), invertedTicks);
        }

        public static string GetInvertedTicks(string tumblrRowKey)
        {
            return tumblrRowKey.Substring(2);
        }
    }
}