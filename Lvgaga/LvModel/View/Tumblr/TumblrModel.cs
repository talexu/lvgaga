using System;
using System.Text;

namespace LvModel.View.Tumblr
{
    public class TumblrModel
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string Id { get; set; }

        public string Base64Id
        {
            get { return Convert.ToBase64String(Encoding.UTF8.GetBytes(Id)); }
        }
        public string Uri { get; set; }
        public string MediaType { get; set; }
        public string MediaUri { get; set; }
        public string MediaLargeUri { get; set; }
        public string MediaMediumUri { get; set; }
        public string MediaSmallUri { get; set; }
        public string TumblrCategory { get; set; }
        public string Text { get; set; }
        public DateTime CreateTime { get; set; }
        public bool IsFavorited { get; set; }
    }
}