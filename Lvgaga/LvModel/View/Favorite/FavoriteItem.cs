using System;

namespace LvModel.View.Favorite
{
    public class FavoriteItem
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string MediaUri { get; set; }
        public string Thumbnail { get; set; }
        public string Text { get; set; }
        public DateTime CreateTime { get; set; }
    }
}