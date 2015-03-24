using System;

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
            _uriBuilder = new UriBuilder()
            {
                Scheme = Scheme,
                Port = Port,
                Host = Host,
            };
        }

        public string CreateUri(string path)
        {
            _uriBuilder.Path = path;
            return _uriBuilder.Uri.AbsoluteUri;
        }
    }
}