using System;

namespace LvModel.View.Tumblr
{
    public class TumblrModel
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string Uri { get; set; }
        public string MediaUri { get; set; }
        public string Thumbnail { get; set; }
        public string Text { get; set; }
        public DateTime CreateTime { get; set; }
    }
}