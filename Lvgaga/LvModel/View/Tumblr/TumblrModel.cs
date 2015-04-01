using System;
using System.IO;

namespace LvModel.View.Tumblr
{
    public class TumblrModel
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string Id { get; set; }
        public string Uri { get; set; }
        public string MediaType { get; set; }
        public string MediaUri { get; set; }
        public string ThumbnailUri { get; set; }
        public string TumblrCategory { get; set; }
        public string Text { get; set; }
        public DateTime CreateTime { get; set; }
    }
}