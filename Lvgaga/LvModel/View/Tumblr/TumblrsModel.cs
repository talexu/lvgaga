using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvModel.View.Tumblr
{
    public class TumblrsModel
    {
        public List<TumblrModel> Tumblrs { get; set; }
        public TableContinuationToken ContinuationToken { get; set; }
        public string Sas { get; set; }
    }
}