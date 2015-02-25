using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvModel.Azure.StorageTable
{
    public class TumblrEntity : TableEntity
    {
        public TumblrEntity(string category, string inverseCreateTime)
        {
            PartitionKey = category;
            RowKey = inverseCreateTime;
        }

        public TumblrEntity()
        {

        }

        public string Uri { get; set; }
        public TumblrType Type { get; set; }
        public string Text { get; set; }
        public DateTime CreateTime { get; set; }
        public TumblrState State { get; set; }
        public IEnumerable<string> Tags { get; set; }

        public enum TumblrType
        {
            Image = 0,
            Gif = 1,
            Audio = 2,
            Video = 3
        }

        public enum TumblrState
        {
            Active = 0,
            Inactive = 1
        }
    }
}
