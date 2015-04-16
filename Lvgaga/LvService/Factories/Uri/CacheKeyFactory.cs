using System;
using System.IO;
using System.Linq;

namespace LvService.Factories.Uri
{
    public class CacheKeyFactory : ICacheKeyFactory
    {
        private readonly UriBuilder _uriBuilder;

        public CacheKeyFactory()
        {
            _uriBuilder = new UriBuilder();
        }

        public string CreateKey(string region, params string[] paths)
        {
            _uriBuilder.Scheme = region;
            _uriBuilder.Path = Path.Combine(paths.Where(p => !String.IsNullOrEmpty(p)).ToArray());
            return _uriBuilder.Uri.AbsoluteUri;
        }
    }
}